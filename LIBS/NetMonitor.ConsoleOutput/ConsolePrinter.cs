using NetMonitor.Testing.Results;
using System;

namespace NetMonitor.ConsoleOutput
{
	public static class ConsolePrinter
	{
		public static void PrintSpeedTestResult(SpeedResult result)
		{
			Console.WriteLine();
			Console.WriteLine("===== Speed Test Result: {0} ===== ", result.TimeStamp);
			Console.WriteLine("File URL: {0}", result.TestedFileURL);
			Console.WriteLine("Download duration: {0}", result.ElapsedTime);
			Console.WriteLine("File size: {0} MB", result.ResultFileSizeInMB.ToString("N2"));
			Console.WriteLine("Speed: {0} Mbit/s ", result.SpeedResultMbit.ToString("N5"));
		}

		public static void PrintPingTestResult(PingResult result)
		{
			Console.WriteLine();
			Console.WriteLine("===== Ping Test Result: {0} ===== ", result.TimeStamp);
			Console.WriteLine("Ping target: {0}", result.PingedTarget);

			if (!result.IsSuccess)
			{
				Console.WriteLine("Ping failed!!!!!!!");
				return;
			}

			Console.WriteLine("Ping response in: {0} ms", result.ResponseMiliseconds);
		}

		public static void PrintAgregatedPingTestResult(PingAgregatedResult result)
		{
			Console.WriteLine();
			Console.WriteLine("===== Ping Agregated Test Result: {0} ===== ", result.TimeStamp);
			Console.WriteLine("Ping target: {0}", result.PingedTarget);
			Console.WriteLine();
			Console.WriteLine("Number of subtests: {0}", result.TestTotalCount);
			Console.WriteLine("  Failed tests: {0}", result.TestFailedCount);
			Console.WriteLine("  SuccessfulTests: {0}", result.TestSuccessfulCount);
			Console.WriteLine("  Success rate: {0:N2}%", ((double)result.TestSuccessfulCount / result.TestTotalCount) * 100);
			Console.WriteLine();
			Console.WriteLine("Average ping: {0:N2} ms", result.AveragePing);
		}
	}
}