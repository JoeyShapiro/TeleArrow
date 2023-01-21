// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using Telekinesis;

Console.WriteLine("Hello, World!");

var ipEndPoint = new IPEndPoint(IPAddress.Any, 8787);
TcpListener listener = new(ipEndPoint);
var player = new Player();
player.x = 10;
player.y = 50;
player.health = 512;
var json = JsonSerializer.Serialize(player);
Console.WriteLine(json);

var players = new List<Player>();

listener.Start();

var clients = new List<TcpClient>();

var handler = await listener.AcceptTcpClientAsync();
NetworkStream stream = handler.GetStream();

while (true)
{
	if (!handler.Connected)
	{
		handler = await listener.AcceptTcpClientAsync();
		continue;
	}

	stream = handler.GetStream();
	var buffer = new byte[1_024];
	int cnt = await stream.ReadAsync(buffer);
	var received = Encoding.UTF8.GetString(buffer, 0, cnt);
	if (received.Length == 0)
	{
		handler.Close();
		continue;
	}

	var message = JsonSerializer.Deserialize<Message>(received ?? "{}") ?? new Message();

	switch (message.ID)
	{
		case Message.MSG_CONNECT:
			players.Add(new Player());
			break;
		case Message.MSG_UNKNOWN:
			Console.WriteLine($"Recieved an unknown message");
			break;
		default:
			Console.WriteLine($"Received a weird message {received}");
			break;
	}
}

//try
//{
//	listener.Start();

//	using TcpClient handler = await listener.AcceptTcpClientAsync();
//	await using NetworkStream stream = handler.GetStream();

//	//await stream.WriteAsync(player.Serialize());
//	await stream.WriteAsync(Encoding.UTF8.GetBytes(json));

//	Console.WriteLine($"Sent message: \"{json}\"");
//}
//finally
//{
//	listener.Stop();
//}