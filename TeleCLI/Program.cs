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
var message = new Message { ID = Message.MSG_PLAYER };
var test = new Player();
test.username = "test";
var msgConnect = new Message { ID = Message.MSG_CONNECT };
var json = JsonSerializer.Serialize(msgConnect);


using var client = new TcpClient();
client.Connect("localhost", 8787);

var i = 0;
await using NetworkStream stream = client.GetStream();

await stream.WriteAsync(Encoding.UTF8.GetBytes(json));

json = JsonSerializer.Serialize(message);
Console.WriteLine($"Sent message: \"{json}\"");
Thread.Sleep(1000);

while (i != 3)
{
	await stream.WriteAsync(Encoding.UTF8.GetBytes(json));
	Console.WriteLine($"Sent message: \"{json}\"");

    var buffer = new byte[1_024];
    int cnt = stream.Read(buffer);
    var received = Encoding.UTF8.GetString(buffer, 0, cnt);
    Console.WriteLine(received);

    Thread.Sleep(1000);
	i++;
}

message = new Message { ID = Message.MSG_DISCONNECT };
json = JsonSerializer.Serialize(message);
await stream.WriteAsync(Encoding.UTF8.GetBytes(json));
Console.WriteLine($"Sent message: \"{json}\"");
Thread.Sleep(1000);

client.Close();