#region Namespace Sonata.Diagnostics
//	TODO: comment
#endregion

using Sonata.Diagnostics.Logs;

namespace Sonata.Diagnostics
{
	internal class LoggerConfiguration
	{
		#region Properties

		public static LoggerProvider LoggerProvider { get; set; }

		public static bool IsBusinessLogTableCreated { get; set; }

		#endregion
	}
}
