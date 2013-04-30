using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace DataViewer_Server
{
	class Program
	{
		private static int PORT = 5050;

		static void Main(string[] args)
		{
			IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("42.121.120.41"), PORT);
			TcpListener listener = new TcpListener(ipEndPoint);
			listener.Start();
			while (true)
			{
				Console.WriteLine("Waiting Connection...");
				Task task = new Task(ClientTask, listener.AcceptTcpClient());
				task.Start();
			}
		}

		static void ClientTask(object client)
		{
			bool connected = true;
			NetworkStream stream = (client as TcpClient).GetStream();
			while (connected)
			{
				List<byte> data_all = new List<byte>();
				while (stream.DataAvailable)
				{
					byte[] data = new byte[1024];
					stream.Read(data, 0, data.Length);
					foreach (byte item in data)
					{
						if (item != 0)
							data_all.Add(item);
						else
							break;
					}
				}
				if (data_all.Count != 0)
				{
					byte[] data = data_all.ToArray();
					string data_string = new string(Encoding.UTF8.GetChars(data));
					//foreach (byte item in data)
					//{
					//	data_string += item.ToString();
					//}
					Console.WriteLine(data_string);
					if (data_string == "disconnect")
						connected = false;
				}
			}
			(client as TcpClient).Close();
			Console.WriteLine("Connection Closed");
		}
	}
}
