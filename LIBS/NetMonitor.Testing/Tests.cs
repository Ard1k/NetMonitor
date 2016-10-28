using NetMonitor.Testing.Results;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.NetworkInformation;

namespace NetMonitor.Testing
{
	public class Tests
	{
		public static SpeedResult DownloadSpeedTest()
		{
			string testurl = ConfigurationManager.AppSettings["speedTarget"];

			System.Net.WebClient webClient = new System.Net.WebClient() { CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore) };

			DateTime testStart = DateTime.Now;
			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			byte[] byteFile = new byte[0];

			try
			{
				sw.Start();
				byteFile = webClient.DownloadData(testurl);
				sw.Stop();
			}
			catch (Exception ex)
			{
				return new SpeedResult
				{
					ElapsedTime = TimeSpan.MinValue,
					ResultFileSizeInMB = 0,
					SpeedResultMbit = 0,
					TestedFileURL = testurl,
					TimeStamp = testStart,
					Error = ex.Message
				};
			}

			double speed = 0;

			if (sw.ElapsedMilliseconds >= 0)
			{
				speed = (byteFile.Length / (sw.Elapsed.TotalMilliseconds)); //bajtu za milisekundu
				speed = speed * 1000; //bajtu za sekundu
				speed = speed * 8; // bitu za sekundu
				speed = (speed / 1024) / 1024; // Mbit/s
			}

			return new SpeedResult
			{
				TimeStamp = testStart,
				TestedFileURL = testurl,
				ElapsedTime = sw.Elapsed,
				ResultFileSizeInMB = ((double)byteFile.Length / 1024) / 1024,
				SpeedResultMbit = speed
			};
		}

		public static PingResult PingTest(string target)
		{
			Ping pinger = new Ping();
			var result = new PingResult { IsSuccess = false, PingedTarget = target, TimeStamp = DateTime.Now };

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