#region Namespace Sonata.Diagnostics.Logging
//	TODO
#endregion

using Microsoft.Extensions.Logging;
using System.Runtime.Serialization;

namespace Sonata.Diagnostics.Logging
{
	[DataContract(Name = "log")]
	public class Log
	{
		#region Properties

		[DataMember(Name = "level")]
		public LogLevel Level { get; set; }

		[DataMember(Name = "code")]
		public string Code { get; set; }

		[DataMember(Name = "message")]
		public string Message { get; set; }

		#endregion

		#region Methods

		public static Log BuildTrace(string message)
		{
			return BuildTrace(null, message);
		}

		public static Log BuildTrace(string code, string message)
		{
			return Build(LogLevel.Trace, code, message);
		}

		public static Log BuildDebug(string message)
		{
			return BuildDebug(null, message);
		}

		public static Log BuildDebug(string code, string message)
		{
			return Build(LogLevel.Debug, code, message);
		}

		public static Log BuildInformation(string message)
		{
			return BuildInformation(null, message);
		}

		public static Log BuildInformation(string code, string message)
		{
			return Build(LogLevel.Information, code, message);
		}

		public static Log BuildWarning(string message)
		{
			return BuildWarning(null, message);
		}

		public static Log BuildWarning(string code, string message)
		{
			return Build(LogLevel.Warning, code, message);
		}

		public static Log BuildError(string message)
		{
			return BuildError(null, message);
		}

		public static Log BuildError(string code, string message)
		{
			return Build(LogLevel.Error, code, message);
		}

		public static Log BuildCritical(string message)
		{
			return BuildCritical(null, message);
		}

		public static Log BuildCritical(string code, string message)
		{
			return Build(LogLevel.Critical, code, message);
		}

		public static Log Build(LogLevel level, string message)
		{
			return Build(level, null, message);
		}

		public static Log Build(LogLevel level, string code, string message)
		{
			return new Log
			{
				Level = level,
				Code = code,
				Message = message
			};
		}

		#endregion
	}
}
