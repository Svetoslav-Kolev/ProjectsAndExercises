using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCPClientServer;


public class Server
{
    public IPAddress address { get; private set; }

    public int port { get; private set; }

    public EndPoint endPoint { get; private set; }

    public bool IsRunning { get; private set; }

    public Socket socket { get; private set; }
    public static Dictionary<string, Guid> connectedClients { get; private set; }
    public static List<Client> connections { get; private set; }

    public static ManualResetEvent allDone = new ManualResetEvent(false); //signals thread to stop or continue

    CancellationTokenSource source;

    public Server(IPAddress address, int port)
    {
        this.address = address;

        this.port = port;

        this.endPoint = new IPEndPoint(address, port);

        this.socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        connectedClients = new Dictionary<string, Guid>();

        connections = new List<Client>();
    }
    public void OpenServer()
    {
        this.socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        this.socket.Bind(endPoint);
        this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        this.socket.Listen(10);
        Task.Run(() => acceptConnections());

        this.IsRunning = true;
    }
    public void StopServer()
    {
        if (this.IsRunning == true)
        {
            this.IsRunning = false;

            foreach (var client in connections)
            {
                lock (client)
                {
                    DisconnectionPackage stopServerPackage = new DisconnectionPackage(client.Username, "Server has been closed");
                    TrySendObject(stopServerPackage, client.socket);
                    client.socket.Shutdown(SocketShutdown.Both);
                    client.socket.Close();
                }

            }

            connectedClients.Clear();
            connections.Clear();
            this.socket.Close();
            //this.socket = null;
        }
    }
    public void acceptConnections()
    {
        while (IsRunning)
        {

            allDone.Reset(); // set event to nonsignaled state

            this.socket.BeginAccept(new AsyncCallback(AcceptCallback), this.socket);

            allDone.WaitOne(); // waits for a connection
        }
    }
    public async void AcceptCallback(IAsyncResult ar)
    {
        Socket handler = null;

        if (IsRunning != false)
        {

            allDone.Set(); //signals main thread to continue

            Socket listener = (Socket)ar.AsyncState;
            handler = listener.EndAccept(ar); // accepts connection and lets the handler socket deal with communications

        }
        else
        {
            allDone.Set();
            return;
        }
        var ConnectionPacket = tryReadObject(handler);

        if (ConnectionPacket is ConnectionPackage packet)
        {

            if (!connectedClients.Keys.Contains(packet.sender))
            {
                connectedClients[packet.sender] = packet.userId;
                Client newClient = new Client() { id = packet.userId, Username = packet.sender };
                newClient.socket = handler;
                connections.Add(newClient);


                await newClient.sendMessage("You have been Connected!");

                Task.Run(() => receiveMessagesTask(newClient.socket));
                Task.Run(() => UpdateUsersTask(newClient.socket));
            }
            else
            {
                DisconnectionPackage userTakenPacket = new DisconnectionPackage(packet.sender, "Username Taken");
                await TrySendObject(userTakenPacket, handler);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
        else
        {
            return;
        }

    }
    public async Task receiveMessagesTask(Socket clientSocket)
    {

        while (clientSocket.Connected)
        {
            if (clientSocket.Available != 0)
            {
                var receivedPackage = tryReadObject(clientSocket);

                if (receivedPackage is MessagePacket package)
                {
                    if (package.isPersonal == true)
                    {
                        Client targetClient = connections.Where(c => c.Username == package.targetUsername).FirstOrDefault();
                        if (targetClient != null)
                        {
                            await TrySendObject(receivedPackage, targetClient.socket);
                        }
                        else
                        {
                            await TrySendObject(new MessagePacket("User you are trying to reach is not currently online!", package.sender, true), clientSocket);
                        }
                    }
                    else
                    {
                        foreach (var connection in connections)
                        {
                            await TrySendObject(receivedPackage, connection.socket);
                        }
                    }
                }
                else if (receivedPackage is DisconnectionPackage dcPackage)
                {
                    connectedClients.Remove(dcPackage.sender);

                    RemoveConnection(clientSocket);
                }
                else if (receivedPackage is ImagePacket ImagePacket)
                {

                    if (ImagePacket.isPersonal == true)
                    {
                        Client targetClient = connections.Where(c => c.Username == ImagePacket.targetUsername).FirstOrDefault();
                        if (targetClient != null)
                        {
                            await TrySendObject(ImagePacket, targetClient.socket);
                        }
                    }
                    else
                    {
                        foreach (var client in connections)
                        {
                            await TrySendObject(ImagePacket, client.socket);
                        }
                    }


                }

            }
            await Task.Delay(10);
        }

    }

    private async Task UpdateUsersTask(Socket clientSocket)
    {
        while (clientSocket.Connected)
        {
            List<string> usernames = connectedClients.Keys.OrderBy(username => username).ToList();
            UsersPacket usersPacket = new UsersPacket(usernames);
            await TrySendObject(usersPacket, clientSocket);
            await Task.Delay(5000);
        }

    }
    private static object tryReadObject(Socket clientSocket)
    {
        byte[] lengthBuffer = new byte[4];
        try
        {
            using (NetworkStream stream = new NetworkStream(clientSocket))
            {

                stream.Read(lengthBuffer, 0, lengthBuffer.Length);
                int length = BitConverter.ToInt32(lengthBuffer, 0);
                byte[] data = new byte[length];

                int bytesRead = 0;
                while (bytesRead < length)
                {
                    bytesRead += stream.Read(data, bytesRead, data.Length-bytesRead);
                }

                MemoryStream memory =  new MemoryStream(data, 0, data.Length);
                memory.Position = 0;

                BinaryFormatter formatter = new BinaryFormatter();
                var obj = formatter.Deserialize(memory);

                return obj;
            }
        }
        catch (Exception)
        {

            return null;
        }
    }
    public static async Task TrySendObject(object obj, Socket clientSocket)
    {
        try
        {

            using (Stream stream = new NetworkStream(clientSocket))
            {
                var memory = new MemoryStream();
                var formatter = new BinaryFormatter();
                formatter.Serialize(memory, obj);

                byte[] lengthPrefix = BitConverter.GetBytes(Convert.ToInt32(memory.Length));

                var newObj = memory.ToArray();
                byte[] ret = new byte[newObj.Length + lengthPrefix.Length];

                lengthPrefix.CopyTo(ret, 0);
                newObj.CopyTo(ret, lengthPrefix.Length);

                memory.Position = 0;
                stream.Write(ret, 0, ret.Length);
                //stream.Flush();
            }

        }
        catch (IOException)
        {
            return;
        }
    }
    public void RemoveConnection(Socket handler)
    {
        Client client = connections.Where(c => c.socket == handler).FirstOrDefault();
        connections.Remove(client);
        handler.Close();
        handler.Shutdown(SocketShutdown.Both);

    }

}
