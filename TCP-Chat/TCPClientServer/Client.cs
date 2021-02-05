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
            this.ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1]; // only needed if you wish to send your ip as well

            this.isConnected = false;

            this.id = Guid.NewGuid();


        }
        public async Task<bool> setEndPoint(IPAddress ip, int port)
        {
            this.endPoint = new IPEndPoint(ip, port);
            try
            {
                Thread.Sleep(3);
                this.socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this.socket.Connect(endPoint);
                if (SocketConnected(this.socket))
                {
                    this.isConnected = true;
                    await TrySendObject(new ConnectionPackage(this.id, this.Username));
                    return true;
                }
                else
                {
                    this.isConnected = false;
                    this.socket.Shutdown(SocketShutdown.Both);
                    this.socket.Close();
                    return false;
                }

            }
            catch (SocketException e)
            {
                this.isConnected = false;
                throw e;
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
                    
                    stream.Write(ret, 0, ret.Length);
                    //stream.Flush();
                }
            }
            catch (IOException)
            {
                throw;
            }
        }
        public async Task sendMessage(string message)
        {
            try
            {
                MessagePacket newMessage = new MessagePacket(message);
                newMessage.sender = this.Username;
                await TrySendObject(newMessage);
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
                        receivedObject = await tryReadObject();

                        receivedObject = InterpretPackage(receivedObject);
                        if (receivedObject is DisconnectionPackage package)
                        {
                            return package.reason;
                        }
                        else if (receivedObject is UsersPacket usersPacket)
                        {
                            return usersPacket.Usernames;
                        }
                        else if (receivedObject != null)
                        {
                            return receivedObject;
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }

                }
                await Task.Delay(10);
            }
            return receivedObject;
        }
        private async Task<object> tryReadObject()
        {
            byte[] lengthBuffer = new byte[4];
            //byte[] data = new byte[clientSocket.ReceiveBufferSize];
            try
            {
                using (NetworkStream stream = new NetworkStream(this.socket))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    int lengthOffset = 0;
                    while (lengthOffset<4)
                    {
                        lengthOffset +=stream.Read(lengthBuffer, lengthOffset, lengthBuffer.Length-lengthOffset);
                    }
                  
                    int length = BitConverter.ToInt32(lengthBuffer, 0);
                    byte[] data = new byte[length];
                    int bytesRead = 0;
                    while (bytesRead < length)
                    {
                        bytesRead += stream.Read(data, bytesRead, data.Length-bytesRead);
                    }

                    MemoryStream memory = new MemoryStream(data, 0, data.Length);
                    memory.Position = 0;

                    var obj = formatter.Deserialize(memory);

                    return obj;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }
        private Package InterpretPackage(object receivedObject)
        {
            if (receivedObject is DisconnectionPackage package)
            {
                this.socket.Shutdown(SocketShutdown.Both);
                this.socket.Close();
                this.isConnected = false;
                return package;
            }
            else if (receivedObject is MessagePacket messagePacket)
            {
                return messagePacket;
            }
            else if (receivedObject is UsersPacket usersPacket)
            {
                return usersPacket;
            }
            else if (receivedObject is ImagePacket imgPacket)
            {

                return imgPacket;
            }
            else
            {
                return null;
            }
        }
        public async Task TryDisconnect()
        {
            if (this.isConnected)
            {
                try
                {
                    DisconnectionPackage dcPackage = new DisconnectionPackage(Username, "Request disconnection");
                    await TrySendObject(dcPackage);
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
