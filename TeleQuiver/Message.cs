using System;
namespace TeleQuiver
{
	public class Message
	{
		public static readonly int MSG_UNKNOWN = -1;
		public static readonly int MSG_ERROR = 0;
		public static readonly int MSG_PLAYERS = 1;
		public static readonly int MSG_PLAYER = 2;
		public static readonly int MSG_CONNECT = 3;
		public static readonly int MSG_DISCONNECT = 4;

		public int Type { get; set; }
		public string Data { get; set; }
		public string Error { get; set; }


		public Message()
		{
		}


	}
}

