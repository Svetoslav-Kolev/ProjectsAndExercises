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

public class Server
{
	public IPAddress address { get; private set; }

	public int port { get; private set; }

	public EndPoint endPoint { get; private set; }

	public bool IsRunning { get; private set; }

	public Socket socket { get; private set; }

	public Server(IPAddress address,int port)
	{
		this.address = address; 

		this.port = port;

		this.endPoint = new IPEndPoint(address, port);

		this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		socket.ReceiveTimeout(5000);

	}
	public void OpenServer()
    {
		this.socket.Bind(endPoint);

		this.socket.Listen(10);
	}
	public void ReceiveAndReturnMessage()
    {
		Socket handler = this.socket.Accept();

		string data = null;
		byte[] bytes = null;


		while (true)
		{
			bytes = new byte[1024];
			int bytesRec = handler.Receive(bytes);
			data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
			if (data.IndexOf("<EOF>") > -1)
			{
				break;
			}
		}

		byte[] msg = Encoding.ASCII.GetBytes(data);
		handler.Send(msg);
		handler.Shutdown(SocketShutdown.Both);
		handler.Close();
	}
}
