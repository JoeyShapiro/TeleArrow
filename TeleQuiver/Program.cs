// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
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

//var handler = await listener.AcceptTcpClientAsync();
TcpClient handler = null;
Task<TcpClient> handlerAsync = listener.AcceptTcpClientAsync();
NetworkStream stream;// = handler.GetStream();

while (true)
{
	if (!handlerAsync.IsCompleted)
		continue;
	else if (handlerAsync.IsCompleted && handler == null)
	{
		handler = handlerAsync.Result;
		Console.WriteLine($"someone connectied, no longer alone :D {handler.Client.LocalEndPoint} <=> {handler.Client.RemoteEndPoint}");
	}
	//else
	//{
	//	handler = handlerAsync.Result;
	//	Console.WriteLine($"someone connectied, no longer alone :D {handler.Client.LocalEndPoint} <=> {handler.Client.RemoteEndPoint}");
	//	handlerAsync = listener.AcceptTcpClientAsync();
	//}

	if (!handler.Connected)
	{
		handler = null;
		Console.WriteLine("waiting for player...");
		//handler = await listener.AcceptTcpClientAsync();
		handlerAsync = listener.AcceptTcpClientAsync();
		Console.WriteLine("tired of waiting :P");
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
