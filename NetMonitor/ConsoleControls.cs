using System;

namespace NetMonitor
{
	public static class ConsoleControls
	{
		public static void WaitForInput()
		{
			var opt = Console.ReadLine().ToLower();

			switch (opt)
			{
				case "exit": Environment.Exit(0); break;
				case "help": Help(); break;
				case "start ping": Program.pingThread.Start(); break;
				default: break;
			}
		}

		private static void Help()
		{
			Console.WriteLine("help!!!! wohooo");
		}
	}
}