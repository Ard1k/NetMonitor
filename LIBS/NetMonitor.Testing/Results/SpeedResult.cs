﻿using System;

namespace NetMonitor.Testing.Results
{
	public class SpeedResult
	{
		public DateTime TimeStamp { get; set; }
		public string TestedFileURL { get; set; }
		public TimeSpan ElapsedTime { get; set; }
		public double SpeedResultMbit { get; set; }
		public double ResultFileSizeInMB { get; set; }
		public string Error { get; set; }
	}
}