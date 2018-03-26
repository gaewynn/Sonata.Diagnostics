#region Namespace Sonata.Diagnostics.Logging
//	TODO
#endregion

using Microsoft.Extensions.Logging;
using System;

namespace Sonata.Diagnostics.Logging
{
	public class Log4NetProperties : ILog4NetProperties
	{
		#region Properties

		public string UserName { get; set; }

		public string Code { get; set; }

		public string Thread { get; set; }

		public LogLevel Level { get; set; }

		public string Source { get; set; }

		public string Message { get; set; }

		public Exception Exception { get; set; }

		#endregion

		#region Methods

		public static ILog4NetProperties Build(string userName, LogLevel level, Type source, string message, string thread = null, Exception exception = null)
		{
			return Build(userName, level, source, null, message, thread, exception);
		}

		public static ILog4NetProperties Build(string userName, LogLevel level, Type source, string code, string message, string thread = null, Exception exception = null)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return Build(userName, level, source.FullName, code, message, thread, exception);
		}

		public static ILog4NetProperties Build(string userName, LogLevel level, string source, string message, string thread = null,  Exception exception = null)
		{
			return Build(userName, level, source, null, message, thread, exception);
		}

		public static ILog4NetProperties Build(string userName, LogLevel level, string source, string code, string message, string thread = null, Exception exception = null)
		{
			return new Log4NetProperties
			{
				UserName = userName,
				Code = code,
				Thread = thread,
				Level = level,
				Source = source,
				Message = message,
				Exception = exception
			};
		}

		#endregion
	}
}
