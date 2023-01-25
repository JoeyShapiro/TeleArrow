using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Telekinesis
{
	public class Connection
	{
        NetworkStream stream;
        int sleep;
        bool debug;

		public Connection()
		{

		}

        public Connection(NetworkStream stream, int sleep, bool debug)
        {
            this.stream = stream;
            this.sleep = sleep;
            this.debug = debug;
        }

		public void Send(int ID, string payload)
		{
            var message = new Message { ID = ID, Data = payload };
            var json = JsonSerializer.Serialize(message);

            this.stream.Write(Encoding.UTF8.GetBytes(json));
            if (debug)
                Console.WriteLine($"Sent message: \"{json}\"");
            Thread.Sleep(this.sleep);
        }

        public string Receive()
        {
            var buffer = new byte[1_024];
            int cnt = this.stream.Read(buffer);
            if (debug)
                Console.WriteLine($"Received {cnt}B");

            return Encoding.UTF8.GetString(buffer, 0, cnt);
        }
	}
}

