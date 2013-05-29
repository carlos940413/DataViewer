using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Xml;
using DataViewer_Entity;

namespace DataViewer_Server
{
	class Program
	{
		static void Main(string[] args)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load("Host.xml");
			string ip = doc.GetElementsByTagName("host")[0].InnerText;
			int port = Int32.Parse(doc.GetElementsByTagName("port")[0].InnerText);
			IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
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
			StringBuilder dataStringBuilder = new StringBuilder();
			while (connected)
			{
				List<byte> data_all = new List<byte>();
				// Read all data from buffer
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
					dataStringBuilder.Append(new string(Encoding.ASCII.GetChars(data_all.ToArray())));
					string data = dataStringBuilder.ToString();
					data = data.Trim();
					Console.WriteLine("{0} -> {1} ^ {2}", data, data.Length, data[data.Length - 1]);

					if (data[data.Length - 1].CompareTo(';') == 0)
					{
						connected = false;
						string pairs = data.TrimEnd(';').Trim(' ');
						if (pairs != "")
						{
							ProcessData(pairs);
						}
					}
				}
			}
			(client as TcpClient).Close();
			Console.WriteLine("Connection Closed");
		}

		static void ProcessData(string data)
		{
			string[] keyValuePairs = data.Split(' ');
			DateTime loggingOn = DateTime.Now;
			foreach (string pair in keyValuePairs)
			{
				int nodeID = Int32.Parse(new string(pair.Split(':')[0].Reverse<char>().ToArray<char>()));
				double concentration = ConvertToConcentration(Int32.Parse(new string(pair.Split(':')[1].Reverse<char>().ToArray<char>())));
				Concentration.SubmitConcentration(nodeID, loggingOn, concentration);
				//Console.WriteLine("{0} : {1:0.00}", nodeID, concentration);
			}
		}

		static double ConvertToConcentration(int data)
		{
			double ratio = ((double)data) / 30000 * 100;
			double concentration;
			if (ratio < 0.6)
				concentration = 0;
			else if (ratio < 2.2)
				concentration = 0.2 * (ratio - 0.6) / (2.2 - 0.6);
			else if (ratio < 4.2)
				concentration = 0.2 * (ratio - 2.2) / (4.2 - 2.2) + 0.2;
			else if (ratio < 6.1)
				concentration = 0.2 * (ratio - 4.2) / (6.1 - 4.2) + 0.4;
			else if (ratio < 7.9)
				concentration = 0.2 * (ratio - 6.1) / (7.9 - 6.1) + 0.6;
			else if (ratio < 9.3)
				concentration = 0.2 * (ratio - 7.9) / (9.3 - 7.9) + 0.8;
			else if (ratio < 10.6)
				concentration = 0.2 * (ratio - 9.3) / (10.6 - 9.3) + 1.0;
			else if (ratio < 12.1)
				concentration = 0.2 * (ratio - 10.6) / (12.1 - 10.6) + 1.2;
			else
				concentration = -1;
			return concentration * 1000;
		}
	}
}
