using NetMonitor.Testing.Results;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace NetMonitor.SimpleLogger
{
	public class Logging
	{
		#region singleton

		private static Logging _instance;

		public static Logging Instance
		{
			get
			{
				if (_instance == null) _instance = new Logging();
				return _instance;
			}
		}

		public Logging()
		{
			this.Path = ConfigurationManager.AppSettings["logPath"];
			this.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd_hh-mm");
			EnsurePath(Path);
		}

		#endregion

		public string TimeStamp { get; set; }
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

			File.AppendAllLines(Path + "/pinglog" + TimeStamp + ".txt", lines);
		}

		public void LogSpeedTest(SpeedResult result)
		{
			var lines = new List<string>();

			var sb = new StringBuilder();

			sb.Append(result.TimeStamp.ToString("dd.MM.yyyy"));
			sb.Append("\t");
			sb.Append(result.TimeStamp.ToString("hh:mm:ss"));
			sb.Append("\t");
			sb.Append(result.TestedFileURL);
			sb.Append("\t");
			if (string.IsNullOrEmpty(result.Error)) sb.Append(result.ResultFileSizeInMB);
			sb.Append("\t");
			if (string.IsNullOrEmpty(result.Error)) sb.Append(result.SpeedResultMbit);
			sb.Append("\t");
			if (string.IsNullOrEmpty(result.Error)) sb.Append(result.ElapsedTime);
			sb.Append("\t");
			if (result.Error != null) sb.Append(result.Error);
			sb.Append("\t");

			lines.Add(sb.ToString());

			File.AppendAllLines(Path + "/speedlog" + TimeStamp + ".txt", lines);
		}

		private void EnsurePath(string path)
		{
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
		}
	}
}