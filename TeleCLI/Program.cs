// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Text.Json;
using Telekinesis;

Console.WriteLine("Hello, World!");

using var client = new TcpClient();
client.Connect("localhost", 8787);
Connection conn = new Connection(client.GetStream(), 1000, true);

var message = new Message { ID = Message.MSG_PLAYER };
var test = new Player();
test.username = "test";
var msgConnect = new Message { ID = Message.MSG_CONNECT };
var json = JsonSerializer.Serialize(msgConnect);

conn.Send(Message.MSG_CONNECT, "");

var i = 0;
while (i != 3)
{
    conn.Send(Message.MSG_PLAYER, JsonSerializer.Serialize(test));

    var received = conn.Receive();
    Console.WriteLine(received);
	i++;
}

conn.Send(Message.MSG_DISCONNECT, "");

client.Close();