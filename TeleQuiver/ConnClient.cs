using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Telekinesis;

namespace TeleQuiver
{
	public class ConnClient
	{
		public TcpClient client;
		public Thread thread;

		public ConnClient()
		{

		}

		public ConnClient(TcpClient c)
		{
			client = c;
			thread = new Thread(() => Handle());

			thread.Start();
		}

		public void Handle()
		{
			
            while (client.Connected)
            {
                //Console.WriteLine($"hello from {client.Client.RemoteEndPoint}");
                //Thread.Sleep(1000);
                var stream = client.GetStream();
                var buffer = new byte[1_024];
                int cnt = stream.Read(buffer);
                Console.WriteLine($"{client.Client.RemoteEndPoint} sent {cnt}B");
                var received = Encoding.UTF8.GetString(buffer, 0, cnt);
                if (received.Length == 0)
                {
                    client.Close();
                    continue;
                }

                var message = JsonSerializer.Deserialize<Message>(received ?? "{}") ?? new Message();

                switch (message.ID)
                {
                    case Message.MSG_CONNECT:
                        //players.Add(new Player());
                        break;
                    case Message.MSG_UNKNOWN:
                        Console.WriteLine($"Recieved an unknown message");
                        break;
                    default:
                        Console.WriteLine($"Received a weird message {received}");
                        break;
                }
            }
            Console.WriteLine("end");
        }

		public void Destroy()
		{
			client.Dispose();
		}
	}
}

