﻿#region Namespace Sonata.Diagnostics.Logging
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

		public ILog4NetProperties Build(Level level, Type source, string message, string thread = null, Exception exception = null)
		{
			return Build(level, source, null, message, thread, exception);
		}

		public ILog4NetProperties Build(Level level, Type source, string code, string message, string thread = null, Exception exception = null)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return Build(level, source.FullName, code, message, thread, exception);
		}

		public ILog4NetProperties Build(Level level, string source, string message, string thread = null,  Exception exception = null)
		{
			return Build(level, source, null, message, thread, exception);
		}

		public ILog4NetProperties Build(Level level, string source, string code, string message, string thread = null, Exception exception = null)
		{
			return new Log4NetProperties
			{
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