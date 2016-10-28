using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMonitor.Testing;
using NetMonitor.SimpleLogger;
using System.Threading;
using System.Diagnostics;

namespace NetMonitor
{
	class Program
	{
		public static Thread pingThread;
		public string exitString;

		static void Main(string[] args)
		{
						
			pingThread = new Thread(PingTargets);
			pingThread.IsBackground = true;

			Console.WriteLine("Running background PingTest...");
			pingThread.Start();

			
			
			while (true)
			{
				var exitString = Console.ReadLine();
				if (exitString == "exit") Environment.Exit(0);
				Thread.Sleep(500);
			}
		}

		public static void PingTargets()
		{
			Console.WriteLine("Ping thread running!");

			var targets = new List<string> { "www.cz", "8.8.8.8", "88.208.111.251", "192.168.1.1" };
			var logger = new Logging("log/testlog.txt");
			var sw = new Stopwatch();

			while (true)
			{
				sw.Reset();
				sw.Start();

				foreach (var t in targets)
				{

					var it = Tests.PingAgregatedTest(t, 200);
					ConsoleOutput.ConsolePrinter.PrintAgregatedPingTestResult(it);
					logger.LogAgregatedPing(it);
				}

				sw.Stop();

				if (sw.ElapsedMilliseconds < 60000)
				{
					Thread.Sleep(60000 - (int)sw.ElapsedMilliseconds);
				}
			}

		}
	}
}
