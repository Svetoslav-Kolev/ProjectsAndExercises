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

public class StateObject // State object for reading client data asynchronously
{
    // Size of receive buffer.  
    public const int BufferSize = 1024;

    // Receive buffer.  
    public byte[] buffer = new byte[BufferSize];

    // Received data string.
    public StringBuilder sb = new StringBuilder();

    // Client socket.
    public Socket workSocket = null;
}
public class Server
{
    public IPAddress address { get; private set; }

    public int port { get; private set; }

    public EndPoint endPoint { get; private set; }

    public bool IsRunning { get; private set; }

    public Socket socket { get; private set; }

    public static string DisconnectID { get; private set; }

    public static List<Socket> connections { get; private set; }

    public static ManualResetEvent allDone = new ManualResetEvent(false); //signals thread to stop or continue
    public Server(IPAddress address, int port)
    {
        this.address = address;

        this.port = port;

        this.endPoint = new IPEndPoint(address, port);

        this.socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        connections = new List<Socket>();
    }
    public void OpenServer()
    {
        this.socket.Bind(endPoint);

        this.socket.Listen(10);

        CreateDisconnectId();

        Task.Run(() => acceptConnections());

    }
    public void StopServer()
    {
        byte[] dcMessage = Encoding.ASCII.GetBytes(DisconnectID);
        foreach (var socket in connections)
        {
            lock (socket)
            {
                socket.Send(dcMessage);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }           
        }
        DisconnectID = null;
    }
    public void CreateDisconnectId()
    {
        DisconnectID = ChatServer.KeyGenerator.GetUniqueKey(30);
    }
    public void acceptConnections()
    {
        while (true)
        {
            allDone.Reset(); // set event to nonsignaled state
                             //Socket handler = this.socket.Accept();

            this.socket.BeginAccept(new AsyncCallback(AcceptCallback), this.socket);

            allDone.WaitOne(); // waits for a connection
        }
    }
    public void AcceptCallback(IAsyncResult ar)
    {
        allDone.Set(); //signals main thread to continue

        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar); //accepts connection and lets the handler socket deal with communications

        if (!connections.Contains(handler))
        {
            connections.Add(handler);
            byte[] dID = Encoding.ASCII.GetBytes(DisconnectID);
            handler.Send(dID);
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }
        else
        {
            return;
        }
    }
    public static void ReadCallback(IAsyncResult ar)
    {
        String content = String.Empty;

        // Retrieve the state object and the handler socket  
        // from the asynchronous state object.  
        StateObject state = (StateObject)ar.AsyncState;
        Socket handler = state.workSocket;

        // Read data from the client socket.
        int bytesRead = 0;
        try
        {
           bytesRead = handler.EndReceive(ar);
        }
        catch (SocketException e)
        {
            if(e.Message == "An existing connection was forcibly closed by the remote host")
            {
                connections.Remove(handler);
                handler.Close();
                handler.Shutdown(SocketShutdown.Both);
                return;
            }
        }
       

        if (bytesRead > 0)
        {
            // There  might be more data, so store the data received so far.  
            state.sb.Append(Encoding.ASCII.GetString(
                state.buffer, 0, bytesRead));

            // Check for end-of-file tag. If it is not there, read
            // more data.  
            content = state.sb.ToString();
            if (content == DisconnectID)
            {
                connections.Remove(handler);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                return;
            }
            else
            {
                if (content != null)
                {
                    // All the data has been read from the
                    // client.
                    // Echo the data back to the client.  
                    Send(handler, content);
                }
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }

        }

    }
    private static void Send(Socket handler, String data)
    {
        // Convert the string data to byte data using ASCII encoding.  
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        byte[] byteDataTest = new byte[0];

        // Begin sending the data to the remote device.
        foreach (var socket in connections)
        {
            if (socket.Connected)
            {
                try
                {
                    socket.BeginSend(byteData, 0, byteData.Length, 0,
          new AsyncCallback(SendCallback), socket);
                }
                catch (Exception)
                {

                }

            }
        }
    }

    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket handler = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = handler.EndSend(ar);

            StateObject state = new StateObject();
            state.workSocket = handler;

            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state); // waits for another message from the same client

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
