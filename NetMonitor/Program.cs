using NetMonitor.SimpleLogger;
using NetMonitor.Testing;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;

namespace NetMonitor
{
	internal delegate void PrintDel(string s);

	public delegate void Terminator();

	internal class Program
	{
		public static PrintDel print = delegate (string s) { Console.WriteLine(s); };
		public static Terminator terminatePing = delegate () { pingThread.Abort(); AppStatus.UpdateAppStatus(); };
		public static Terminator terminateSpeed = delegate () { speedThread.Abort(); AppStatus.UpdateAppStatus(); };
		public static Thread pingThread;
		public static Thread speedThread;

		private static void Main(string[] args)
		{
			Console.Title = "NetMonitor";

			pingThread = new Thread(Pinger);
			pingThread.IsBackground = true;

			speedThread = new Thread(SpeedTester);
			speedThread.IsBackground = true;

			string temp;

			temp = ConfigurationManager.AppSettings["autostartPing"].ToLower();
			if (temp == "t" || temp == "true") pingThread.Start();

			temp = ConfigurationManager.AppSettings["autostartSpeed"].ToLower();
			if (temp == "t" || temp == "true") speedThread.Start();

			while (true)
			{
				ConsoleControls.WaitForInput();
				AppStatus.UpdateAppStatus();
			}
		}

		public static void Pinger()
		{
			print.Invoke("Ping thread started!");
			var targets = ConfigurationManager.AppSettings["pingTargets"].Split('|');
			int pingFreq = 0;

			try
			{
				pingFreq = Convert.ToInt32(ConfigurationManager.AppSettings["pingFreq"]);
			}
			catch
			{
				print.Invoke("Invalid ping frequency, stopping ping thread!");
				terminatePing.Invoke();
			}

			var sw = new Stopwatch();

			while (true)
			{
				sw.Reset();
				sw.Start();

				foreach (var t in targets)
				{
					var it = Tests.PingAgregatedTest(t, 200);
					ConsoleOutput.ConsolePrinter.PrintShortAgregatedPingTestResult(it);
					Logging.Instance.LogAgregatedPing(it);
				}

				sw.Stop();

				if (sw.ElapsedMilliseconds < pingFreq)
				{
					Thread.Sleep(pingFreq - (int)sw.ElapsedMilliseconds);
				}
			}
		}

		public static void SpeedTester()
		{
			print.Invoke("SpeedTest thread running!");

			var sw = new Stopwatch();
			int speedFreq = 0;

			try
			{
				speedFreq = Convert.ToInt32(ConfigurationManager.AppSettings["speedFreq"]);
			}
			catch
			{
				print.Invoke("Invalid SpeedTest frequency, stopping SpeedTest thread!");
				terminateSpeed.Invoke();
			}

			while (true)
			{
				sw.Reset();
				sw.Start();

				var it = Tests.DownloadSpeedTest();
				ConsoleOutput.ConsolePrinter.PrintShortSpeedTestResult(it);

				sw.Stop();

				if (sw.ElapsedMilliseconds < speedFreq)
				{
					Thread.Sleep(speedFreq - (int)sw.ElapsedMilliseconds);
				}
			}
		}
	}
}