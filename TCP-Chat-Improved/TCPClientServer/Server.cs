using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
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
    public Dictionary<string, Guid> connectedClients { get; private set; }
    private List<Client> connections { get; set; }

    private ManualResetEvent allDone = new ManualResetEvent(false); //signals thread to stop or continue

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
        try
        {
            this.socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            this.socket.Bind(endPoint);
            this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.socket.Listen(10);
            Task.Run(() => acceptConnections());
            this.IsRunning = true;
        }
        catch (Exception)
        {
            this.IsRunning = false;
            this.socket.Shutdown(SocketShutdown.Both);
            this.socket.Close();
            connectedClients.Clear();
            connections.Clear();
        }

    }
    public async Task StopServer()
    {
        if (this.IsRunning == true)
        {
            this.IsRunning = false;

            foreach (var client in connections)
            {
                DisconnectionPackage stopServerPackage = new DisconnectionPackage(client.Username, "Server has been closed");
                await TrySendObject(stopServerPackage, client.socket);
            }

            connectedClients.Clear();
            connections.Clear();
            this.socket.Close();
            //this.socket = null;
        }
    }
    private void acceptConnections()
    {
        while (IsRunning)
        {

            allDone.Reset(); // set event to nonsignaled state

            this.socket.BeginAccept(new AsyncCallback(AcceptCallback), this.socket);

            allDone.WaitOne(); // waits for a connection
        }
    }
    private Task updateUsers { get; set; }
    private async void AcceptCallback(IAsyncResult ar)
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
        var ConnectionPacket = await tryReadObject(handler);

        if (ConnectionPacket is ConnectionPackage packet)
        {

            if (!connectedClients.Keys.Contains(packet.sender) && packet.sender.ToLower() != "server")
            {
                connectedClients[packet.sender] = packet.userId;
                Client newClient = new Client() { id = packet.userId, Username = packet.sender };
                newClient.socket = handler;
                connections.Add(newClient);


                await newClient.sendMessage("You have been Connected!");

                updateUsers = UpdateUsersTask(newClient.socket);
                await receiveMessagesTask(newClient.socket);

            }
            else if (packet.sender.ToLower() == "server")
            {
                await HandleWrongCredentials("You cannot be named server!", handler, packet.sender);
            }
            else
            {
                await HandleWrongCredentials("Username Taken", handler, packet.sender);
            }
        }
        else
        {
            return;
        }

    }
    private async Task HandleWrongCredentials(string returnMessage, Socket handler, string sender)
    {
        DisconnectionPackage userTakenPacket = new DisconnectionPackage(sender, returnMessage);
        await TrySendObject(userTakenPacket, handler);
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
    }
    private async Task receiveMessagesTask(Socket clientSocket)
    {

        while (clientSocket.Connected)
        {

            Package receivedPackage = await tryReadObject(clientSocket);

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
                        MessagePacket targetDC = new MessagePacket("User you are trying to reach is offline!");
                        targetDC.isPersonal = true;
                        targetDC.targetUsername = package.sender;
                        targetDC.sender = package.targetUsername;
                        await TrySendObject(targetDC, clientSocket);
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
                    else
                    {
                        MessagePacket targetDC = new MessagePacket("User you are trying to reach is offline!");
                        targetDC.isPersonal = true;
                        targetDC.targetUsername = ImagePacket.sender;
                        targetDC.sender = ImagePacket.targetUsername;
                        await TrySendObject(targetDC, clientSocket);
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

    private async Task<Package> tryReadObject(Socket clientSocket)
    {
        byte[] lengthBuffer = new byte[4];
        try
        {

            using (NetworkStream stream = new NetworkStream(clientSocket))
            {

                int lengthOffset = 0;
                while (lengthOffset < 4)
                {
                    lengthOffset += await stream.ReadAsync(lengthBuffer, lengthOffset, lengthBuffer.Length - lengthOffset);
                }
                int length = BitConverter.ToInt32(lengthBuffer, 0);
                byte[] data = new byte[length];

                int bytesRead = 0;
                while (bytesRead < length)
                {
                    bytesRead += await stream.ReadAsync(data, bytesRead, data.Length - bytesRead);
                }

                MemoryStream memory = new MemoryStream(data, 0, data.Length);
                memory.Position = 0;

                BinaryFormatter formatter = new BinaryFormatter();
                var obj = formatter.Deserialize(memory);
                Package receivedPackage = (Package)obj;

                return receivedPackage;
            }
        }
        catch (IOException)
        {

            RemoveConnection(clientSocket);
            return null;

        }
    }
    bool SocketConnected(Socket s)
    {
        bool part1 = s.Poll(1000, SelectMode.SelectRead);
        bool part2 = (s.Available == 0);
        if (part1 && part2)
            return false;
        else
            return true;
    }
    public async Task TrySendObject(object obj, Socket clientSocket)
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
                await stream.WriteAsync(ret, 0, ret.Length);
            }

        }
        catch (IOException)
        {
            RemoveConnection(clientSocket);
            return;
        }
    }
    private void RemoveConnection(Socket handler)
    {
        Client client = connections.Where(c => c.socket == handler).FirstOrDefault();
        if (client != null)
        {
            connectedClients.Remove(client.Username);
            connections.Remove(client);
        }
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
    }

}
