using System;
using Telekinesis;

namespace TeleQuiver
{
	public class Game
	{
		//public static List<Player> players;
		public static Dictionary<string, Player> players;

		public Game()
		{
			players = new Dictionary<string, Player>();
			// TODO thread here
		}

		public void Run()
		{
			while (true)
			{
				Console.WriteLine($"players in game: {players.Count}");
				Thread.Sleep(1000);
			}
		}
	}
}

