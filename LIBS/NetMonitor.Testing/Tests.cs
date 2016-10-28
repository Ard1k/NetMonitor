using NetMonitor.Testing.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;

namespace NetMonitor.Testing
{
	public class Tests
	{
		public static SpeedResult DownloadSpeedTest()
		{
			const string tempfile = "tempfile.tmp";
			const string testurl = "http://de.testmy.net/dl-19.0MB";

			File.Delete(tempfile);

			System.Net.WebClient webClient = new System.Net.WebClient() { CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore) };

			DateTime testStart = DateTime.Now;
			System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

			webClient.DownloadFile(testurl, tempfile);

			sw.Stop();

			FileInfo fileInfo = new FileInfo(tempfile);

			double speed = (fileInfo.Length / sw.Elapsed.TotalMilliseconds); //bajtu za milisekundu
			speed = speed * 1000; //bajtu za sekundu
			speed = speed * 8; // bitu za sekundu
			speed = speed / 1000000; // Mbit/s

			return new SpeedResult
			{
				TimeStamp = testStart,
				TestedFileURL = testurl,
				ElapsedTime = sw.Elapsed,
				ResultFileSizeInMB = (double)fileInfo.Length / 1000000,
				SpeedResultMbit = speed
			};
		}

		public static PingResult PingTest(string target)
		{
			Ping pinger = new Ping();
			var result = new PingResult { IsSuccess = false, PingedTarget = target, TimeStamp = DateTime.Now};

			try
			{
				var reply = pinger.Send(target);
				result.IsSuccess = reply.Status == IPStatus.Success;
				result.ResponseMiliseconds = reply.RoundtripTime;
			}
			catch (PingException)
			{
				// Discard PingExceptions and return false;
			}
			return result;
		}

		public static PingAgregatedResult PingAgregatedTest(string target, int count)
		{
			if (count <= 0) throw new ArgumentException("count must be greater than zero");

			var result = new PingAgregatedResult { TimeStamp = DateTime.Now, PingedTarget = target, TestTotalCount = count, TestFailedCount = 0, TestSuccessfulCount = 0, Pings = new List<long>() };

			for (int i = 0; i < count; i++)
			{
				var temp = PingTest(target);
				result.Pings.Add(temp.ResponseMiliseconds);

				if (temp.IsSuccess)
				{
					result.TestSuccessfulCount++;
				}
				else
				{
					result.TestFailedCount++;
				}
			}

			return result;
		}
	}
}