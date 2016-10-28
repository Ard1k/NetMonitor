using NetMonitor.Testing.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetMonitor.SimpleLogger
{
	public class Logging
	{
		public Logging(string path)
		{
			this.Path = path;
		}

		public string Path { get; set; }

		public void LogAgregatedPing(PingAgregatedResult result)
		{
			var lines = new List<string>();

			var sb = new StringBuilder();

			sb.Append(result.TimeStamp.ToString("dd.MM.yyyy"));
			sb.Append("\t");
			sb.Append(result.TimeStamp.ToString("hh:mm:ss"));
			sb.Append("\t");
			sb.Append(result.PingedTarget);
			sb.Append("\t");
			sb.Append(result.AveragePing);
			sb.Append("\t");
			sb.Append(result.TestTotalCount);
			sb.Append("\t");
			sb.Append(result.TestSuccessfulCount);
			sb.Append("\t");
			sb.Append(result.TestFailedCount);
			sb.Append("\t");

			lines.Add(sb.ToString());

			File.AppendAllLines(Path, lines);
		}
	}
}
