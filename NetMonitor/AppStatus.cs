using System;
using System.Text;

namespace NetMonitor
{
	public static class AppStatus
	{
		public static void UpdateAppStatus()
		{
			var sb = new StringBuilder();

			sb.Append("NetMonitor - Pinger: ");
			if (Program.pingThread.IsAlive) sb.Append("running"); else sb.Append("inactive");
			sb.Append(" SpeedTester: ");
			if (Program.speedThread.IsAlive) sb.Append("running"); else sb.Append("inactive");

			Console.Title = sb.ToString();
		}
	}
}