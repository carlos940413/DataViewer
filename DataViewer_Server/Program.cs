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
			IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), PORT);
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
				while (stream.DataAvailable)
				{
					DateTime acquireOn = DateTime.Now;
					int count = stream.ReadByte();
					if (count == 0)
					{
						connected = false;
					}
					else
					{
						byte[] data = new byte[count * 6];
						stream.Read(data, 0, data.Length);
						Dictionary<int, short> concentrations = new Dictionary<int, short>();
						for (int i = 0; i < data.Length; i += 6)
						{
							int nodeID = BitConverter.ToInt32(data.ToList<byte>().GetRange(i, 4).Reverse<byte>().ToArray<byte>(), 0);
							short amount = BitConverter.ToInt16(data.ToList<byte>().GetRange(i + 4, 2).Reverse<byte>().ToArray<byte>(), 0);
							concentrations.Add(nodeID, amount);
						}
						Console.Write(acquireOn.ToString() + " => ");
						foreach (var key in concentrations.Keys)
						{
							Console.Write(key.ToString() + " : " + concentrations[key].ToString() + ", ");
						}
						Console.WriteLine();
					}
				}
			}
			(client as TcpClient).Close();
			Console.WriteLine("Connection Closed");
		}
	}
}
