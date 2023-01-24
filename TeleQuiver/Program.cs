// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Telekinesis;
using TeleQuiver;

Console.WriteLine("Hello, World!");

var ipEndPoint = new IPEndPoint(IPAddress.Any, 8787);
TcpListener listener = new(ipEndPoint);
var player = new Player();
player.x = 10;
player.y = 50;
player.health = 512;
var json = JsonSerializer.Serialize(player);
Console.WriteLine(json);

const int MAX_CLIENTS = 8;
var players = new List<Player>();

listener.Start();

var clients = new List<ConnClient>();

Task<TcpClient> handlerAsync = listener.AcceptTcpClientAsync();
NetworkStream stream;

while (true)
{
    handleConnections(ref clients, ref handlerAsync);

	//foreach (var client in clients)
 //   {
 //       stream = client.client.GetStream();
 //       var buffer = new byte[1_024];
 //       int cnt = await stream.ReadAsync(buffer);
 //       Console.WriteLine($"{client.client.Client.RemoteEndPoint} sent {cnt}B");
 //       var received = Encoding.UTF8.GetString(buffer, 0, cnt);
 //       if (received.Length == 0)
 //       {
 //           client.client.Close();
 //           continue;
 //       }

 //       var message = JsonSerializer.Deserialize<Message>(received ?? "{}") ?? new Message();

 //       switch (message.ID)
 //       {
 //           case Message.MSG_CONNECT:
 //               players.Add(new Player());
 //               break;
 //           case Message.MSG_UNKNOWN:
 //               Console.WriteLine($"Recieved an unknown message");
 //               break;
 //           default:
 //               Console.WriteLine($"Received a weird message {received}");
 //               break;
 //       }
 //   }
}

void handleConnections(ref List<ConnClient> clients, ref Task<TcpClient> handlerAsync) // should just use it i guess, but i feel i normally didnt
{
	TcpClient? handler = null;

	// remove dead connections
	foreach (var client in clients.ToList())
	{
		if (client.client.Connected)
			continue;

		Console.WriteLine("someone left");
		clients.Remove(client);
	}

	// if someone connects
	if (handlerAsync.IsCompleted && handler == null)
	{
		handler = handlerAsync.Result;
		clients.Add(new ConnClient(handler));

		Console.WriteLine($"someone connectied, no longer alone :D {handler.Client.LocalEndPoint} <=> {handler.Client.RemoteEndPoint}");
	}

	// see if someone else can connect
	if (clients.Count < MAX_CLIENTS && handlerAsync.IsCompleted) // clients.Capacity = MAX_CLIENTS;
	{
		Console.WriteLine("waiting for player...");
		handlerAsync = listener.AcceptTcpClientAsync();
		Console.WriteLine("tired of waiting :P");
	}
}
