using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCPClientServer;

namespace TCPClientServer
{
    public class Client
    {
        public IPAddress ip { get; private set; }
        public Guid id { get; set; }
        public Socket socket { get; set; }
        public IPEndPoint endPoint { get; private set; }
        public bool isConnected { get; private set; }
        public string Username { get; set; }
        public bool requestDisconnection { get; set; }
        public Client()
        {
            this.ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1]; // only needed if you wish to send your ip as well

            this.isConnected = false;

            this.id = Guid.NewGuid();

            requestDisconnection = false;
        }
        public async Task<bool> setEndPoint(IPAddress ip, int port)
        {
            this.endPoint = new IPEndPoint(ip, port);
            try
            {
                this.socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this.socket.Connect(endPoint);
                await TrySendObject(new ConnectionPackage(this.id, this.Username));
                this.isConnected = true;
                return true;
            }
            catch (Exception)
            {
                if (this.socket.Connected)
                {
                    this.socket.Shutdown(SocketShutdown.Both);
                    this.socket.Close();
                }
                this.isConnected = false;
                return false;
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

        public async Task TrySendObject(object obj)
        {
            try
            {
                using (NetworkStream stream = new NetworkStream(this.socket))
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
                Disconnect(); //Disconnection and reconnection is later handled in MainWindowViewModel
                throw;
            }
        }
        public async Task sendMessage(string message)
        {

            MessagePacket newMessage = new MessagePacket(message);
            newMessage.sender = this.Username;
            await TrySendObject(newMessage);

        }
        public async Task<Package> receiveMessageAsync()
        {
            return await TryReceiveMessage();
        }
        private async Task<Package> TryReceiveMessage()
        {

            if (!isConnected)
                return null;

            Package receivedObject = null;

            while (receivedObject == null)
            {

                receivedObject = await tryReadObject();

            }
            return receivedObject;
        }
        private async Task<Package> tryReadObject()
        {
            byte[] lengthBuffer = new byte[4];
            try
            {
                using (NetworkStream stream = new NetworkStream(this.socket))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
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

                    var obj = formatter.Deserialize(memory);

                    Package receivedPackage = (Package)obj;
                    return receivedPackage;
                }
            }
            catch (IOException)
            {
                Disconnect(); //Disconnection and reconnection is later handled in MainWindowViewModel
                throw;
            }
        }
        public async Task TryDisconnect()
        {
            if (this.isConnected)
            {
                requestDisconnection = true;
                try
                {
                    DisconnectionPackage dcPackage = new DisconnectionPackage(Username, "Request disconnection");
                    await TrySendObject(dcPackage);
                    Disconnect();
                }
                catch (Exception)
                {
                    if (this.socket.Connected)
                    {
                        Disconnect();
                    }
                   
                }
            }
        }
        public void Disconnect()
        {
            this.socket.Shutdown(SocketShutdown.Both);
            this.socket.Close();
            this.isConnected = false;
        }
    }
}
