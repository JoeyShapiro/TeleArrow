// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

Console.WriteLine("Hello, World!");

using var client = new TcpClient();
client.Connect("localhost", 8787);
await using NetworkStream stream = client.GetStream();

var buffer = new byte[1_024];
int received = await stream.ReadAsync(buffer);
Console.WriteLine(string.Join("", buffer));
Console.WriteLine(BitConverter.ToInt32(buffer, 0));

var message = Encoding.UTF8.GetString(buffer, 0, received);
Console.WriteLine($"Message received: \"{message}\"");
// Sample output:
//     Message received: "📅 8/22/2022 9:07:17 AM 🕛"