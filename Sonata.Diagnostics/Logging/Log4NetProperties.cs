#region Namespace Sonata.Diagnostics.Logging
//	TODO
#endregion

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace Sonata.Diagnostics.Logging
{
	public class Log4NetProperties : ILog4NetProperties
	{
		#region Properties

		[JsonProperty("userName")]
		public string UserName { get; set; }

		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("thread")]
		public string Thread { get; set; }

		[JsonProperty("level")]
		public LogLevel Level { get; set; }

		[JsonProperty("source")]
		public string Source { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("exception")]
		public Exception Exception { get; set; }

		#endregion

		#region Methods

		#region Object Members

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}

		#endregion

		public static ILog4NetProperties Build(string userName, LogLevel level, Type source, string code, string message, string thread, Exception exception)
		{
			return Build(userName, level, source.FullName, code, message, thread, exception);
		}

		public static ILog4NetProperties Build(string userName, LogLevel level, string source, string code, string message, string thread, Exception exception)
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
