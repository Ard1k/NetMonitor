using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetMonitor.Testing.Results
{
	public class PingResult
	{
		public string PingedTarget { get; set; }
		public bool IsSuccess { get; set; }
		public DateTime TimeStamp { get; set; }
		public long ResponseMiliseconds { get; set; }
	}
}
