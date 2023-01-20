// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using TeleQuiver;
using System.Text.Json;

Console.WriteLine("Hello, World!");

var ipEndPoint = new IPEndPoint(IPAddress.Any, 8787);
TcpListener listener = new(ipEndPoint);
var player = new Player();
player.x = 10;
player.y = 50;
player.health = 512;
var json = JsonSerializer.Serialize(player);
Console.WriteLine(json);


try
{
	listener.Start();

	using TcpClient handler = await listener.AcceptTcpClientAsync();
	await using NetworkStream stream = handler.GetStream();

	var message = $"📅 {DateTime.Now} 🕛";
	var dateTimeBytes = Encoding.UTF8.GetBytes(message);

	//await stream.WriteAsync(player.Serialize());
	await stream.WriteAsync(Encoding.UTF8.GetBytes(json));

	Console.WriteLine($"Sent message: \"{json}\"");
	// Sample output:
	//     Sent message: "📅 8/22/2022 9:07:17 AM 🕛"
}
finally
{
	listener.Stop();
}