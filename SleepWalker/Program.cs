using System;

namespace Sleepwalker
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			using (SleepwalkerGame game = new SleepwalkerGame())
			{
				game.Run();
			}
		}
	}
}

