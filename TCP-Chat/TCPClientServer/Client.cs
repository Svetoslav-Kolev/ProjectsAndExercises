using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
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

        public Client()
        {
            //this.ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[2]; // only needed if you wish to send your ip as well

            this.isConnected = false;

            this.id = Guid.NewGuid();
        }
        public void setEndPoint(IPAddress ip, int port)
        {
            this.socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.socket.ReceiveBufferSize = 8192;

            this.endPoint = new IPEndPoint(ip, port);

            try
            {
                this.socket.Connect(endPoint);
                TrySendObject(new ConnectionPackage(this.id, this.Username));
                this.isConnected = true;
            }
            catch (SocketException e)
            {

                throw e;
            }

        }
        public void TrySendObject(object obj)
        {
            try
            {
                using (NetworkStream stream = new NetworkStream(this.socket))
                {
                    var memory = new MemoryStream();
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(memory, obj);
                    var newObj = memory.ToArray();

                    memory.Position = 0;
                    stream.Write(newObj, 0, newObj.Length);
                }
            }
            catch (IOException)
            {
                return;
            }
        }
        public void sendMessage(string message)
        {
            try
            {
                MessagePacket newMessage = new MessagePacket(message);
                newMessage.sender = this.Username;
                TrySendObject(newMessage);
            }
            catch (IOException)
            {
                return;
            }
        }
        public async Task<object> receiveMessageAsync()
        {
            return await Task.Run(() => TryReceiveMessage());
        }
        private async Task<object> TryReceiveMessage()
        {

            if (!isConnected)
                return null;

            object receivedObject = null;
            while (receivedObject == null)
            {
                if (this.socket.Available != 0)
                {
                    try
                    {
                        byte[] data = new byte[this.socket.ReceiveBufferSize];
                        using (NetworkStream stream = new NetworkStream(socket))
                        {
                            //int receivedSize = 0;
                            //while (this.socket.Available != 0)
                            //{
                            //    receivedSize += stream.Read(data, 0, data.Length);
                            //}

                            //MemoryStream memory = new MemoryStream(data, 0, receivedSize);

                            stream.Read(data, 0, data.Length);
                            MemoryStream memory = new MemoryStream(data, 0, data.Length);       
                            memory.Position = 0;


                            BinaryFormatter formatter = new BinaryFormatter();
                            try
                            {
                                receivedObject = formatter.Deserialize(memory);
                            }
                            catch (Exception)
                            {

                                receivedObject = null;
                            }
                            if (receivedObject is DisconnectionPackage package && package.username == Username)
                            {
                                this.socket.Shutdown(SocketShutdown.Both);
                                this.socket.Close();
                                this.isConnected = false;
                                return package.reason;
                            }
                            else if (receivedObject is MessagePacket messagePacket)
                            {
                                return messagePacket;
                            }
                            else if (receivedObject is UsersPacket usersPacket)
                            {
                                return usersPacket.Usernames;
                            }
                            else if (receivedObject is ImagePacket imagePacket)
                            {
                                return imagePacket.Imagebmp;
                            }
                            else
                            {
                                receivedObject = null;
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }

                }
                await Task.Delay(20);
            }
            return receivedObject;
        }
        public void TryDisconnect()
        {
            if (this.isConnected)
            {
                try
                {
                    DisconnectionPackage dcPackage = new DisconnectionPackage(Username, "Request disconnection");
                    TrySendObject(dcPackage);
                    this.socket.Shutdown(SocketShutdown.Both);
                    this.socket.Close();
                    this.isConnected = false;
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }

    }
}
