#region Namespace Sonata.Diagnostics.Logs
//	TODO
#endregion

using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;

namespace Sonata.Diagnostics.Logs
{
	[DataContract(Name = "technicalLog")]
	public class TechnicalLog : Log
	{
		#region Constructors

		public TechnicalLog(Type source, LogLevel level, string message, Exception exception = null)
			: base("", "")
			//: base(source, level, LogType.Technical, message, exception)
		{
		}

		public TechnicalLog(Type source, LogLevel level, string code, string message, Exception exception = null)
			: base("", "")
		//: base(source, level, LogType.Technical, code, message, exception)
		{
		}

		#endregion

		#region Methods

		public void Write(string alternativeUsername = null)
		{
			if (Source == null)
				throw new InvalidOperationException("Source log can not be null.");

			//Save(Source, LogType.Technical, alternativeUsername);
		}

		public static void Info(Type source, string message, Exception exception = null, string alternativeUsername = null)
		{
			//Save(source, LogLevel.Information, LogType.Technical, null, message, exception, alternativeUsername);
		}
		public static void Info(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
		{
			//Save(source, LogLevel.Information, LogType.Technical, code, message, exception, alternativeUsername);
		}

		public static void Debug(Type source, string message, Exception exception = null, string alternativeUsername = null)
		{
			//Save(source, LogLevel.Debug, LogType.Technical, null, message, exception, alternativeUsername);
		}
		public static void Debug(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
		{
			//Save(source, LogLevel.Debug, LogType.Technical, code, message, exception, alternativeUsername);
		}

		public static void Warning(Type source, string message, Exception exception = null, string alternativeUsername = null)
		{
			//Save(source, LogLevel.Warning, LogType.Technical, null, message, exception, alternativeUsername);
		}
		public static void Warning(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
		{
			//Save(source, LogLevel.Warning, LogType.Technical, code, message, exception, alternativeUsername);
		}

		public static void Error(Type source, string message, Exception exception = null, string alternativeUsername = null)
		{
			//Save(source, LogLevel.Error, LogType.Technical, null, message, exception, alternativeUsername);
		}
		public static void Error(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
		{
			//Save(source, LogLevel.Error, LogType.Technical, code, message, exception, alternativeUsername);
		}

		public static void Fatal(Type source, string message, Exception exception = null, string alternativeUsername = null)
		{
			//Save(source, LogLevel.Critical, LogType.Technical, null, message, exception, alternativeUsername);
		}
		public static void Fatal(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
		{
			//Save(source, LogLevel.Critical, LogType.Technical, code, message, exception, alternativeUsername);
		}

		#endregion
	}
}
