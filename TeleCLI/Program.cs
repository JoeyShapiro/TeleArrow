// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Telekinesis;
using System.Threading;

Console.WriteLine("Hello, World!");

//using var client = new TcpClient();
//client.Connect("localhost", 8787);
//await using NetworkStream stream = client.GetStream();

//var buffer = new byte[1_024];
//int received = await stream.ReadAsync(buffer);
//Console.WriteLine(string.Join("", buffer));
//Console.WriteLine(BitConverter.ToInt32(buffer, 0));

//var message = Encoding.UTF8.GetString(buffer, 0, received);
//Console.WriteLine($"Message received: \"{message}\"");
var message = new Message();
var test = new Player();
test.username = "test";
var json = JsonSerializer.Serialize(test);

using var client = new TcpClient();
client.Connect("localhost", 8787);

var i = 0;
await using NetworkStream stream = client.GetStream();



while (i != 3)
{
	await stream.WriteAsync(Encoding.UTF8.GetBytes(json));

	Console.WriteLine($"Sent message: \"{json}\"");
	Thread.Sleep(1000);
	i++;
}
client.Close();