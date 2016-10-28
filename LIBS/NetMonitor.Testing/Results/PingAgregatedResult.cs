using System;
using System.Collections.Generic;

namespace NetMonitor.Testing.Results
{
	public class PingAgregatedResult
	{
		public string PingedTarget { get; set; }
		public DateTime TimeStamp { get; set; }
		public int TestTotalCount { get; set; }
		public int TestSuccessfulCount { get; set; }
		public int TestFailedCount { get; set; }

		public List<long> Pings { get; set; }

		public double AveragePing
		{
			get
			{
				if (TestTotalCount <= 0) return -1;

				double result = 0;
				foreach (var it in Pings) { result += it; }
				return result / TestSuccessfulCount;
			}
		}
	}
}