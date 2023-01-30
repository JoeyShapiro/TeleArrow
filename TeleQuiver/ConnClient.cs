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
			Console.WriteLine($"hello from {client.Client.RemoteEndPoint}");
			Connection conn = new Connection(client.GetStream(), 1000, true);

			while (client.Connected)
			{
				var received = conn.Receive();

				if (received.Length == 0)
				{
					client.Close();
					continue;
				}

				var session = client.Client.RemoteEndPoint!.ToString()!;
                var message = JsonSerializer.Deserialize<Message>(received ?? "{}") ?? new Message();
				switch (message.ID)
				{
					case Message.MSG_CONNECT:
						Game.players.Add(session, new Player());
						break;
					case Message.MSG_DISCONNECT:
						Game.players.Remove(session);
						break;
					case Message.MSG_PLAYER:
						var player = JsonSerializer.Deserialize<Player>(message.Data);
						var players = Game.UpdatePlayers(session, player);
						conn.Send(Message.MSG_PLAYER, JsonSerializer.Serialize(players));
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

