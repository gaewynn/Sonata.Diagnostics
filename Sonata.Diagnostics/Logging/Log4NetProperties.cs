#region Namespace Sonata.Diagnostics.Logging
//	TODO
#endregion

using log4net.Core;
using System;
using System.Runtime.Serialization;

namespace Sonata.Diagnostics.Logging
{
	[DataContract(Name = "properties")]
	public class Log4NetProperties : ILog4NetProperties
	{
		#region Properties

		[DataMember(Name = "userName")]
		public string UserName { get; set; }

		[DataMember(Name = "code")]
		public string Code { get; set; }

		[DataMember(Name = "thread")]
		public string Thread { get; set; }

		[DataMember(Name = "level")]
		public Level Level { get; set; }

		[DataMember(Name = "source")]
		public string Source { get; set; }

		[DataMember(Name = "message")]
		public string Message { get; set; }

		[DataMember(Name = "exception")]
		public Exception Exception { get; set; }

		#endregion

		#region Methods

		public static ILog4NetProperties Build(string userName, Level level, Type source, string message, string thread = null, Exception exception = null)
		{
			return Build(userName, level, source, null, message, thread, exception);
		}

		public static ILog4NetProperties Build(string userName, Level level, Type source, string code, string message, string thread = null, Exception exception = null)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return Build(userName, level, source.FullName, code, message, thread, exception);
		}

		public static ILog4NetProperties Build(string userName, Level level, string source, string message, string thread = null,  Exception exception = null)
		{
			return Build(userName, level, source, null, message, thread, exception);
		}

		public static ILog4NetProperties Build(string userName, Level level, string source, string code, string message, string thread = null, Exception exception = null)
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
