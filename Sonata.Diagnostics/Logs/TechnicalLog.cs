#region Namespace Sonata.Diagnostics.Logs
//	TODO: comment
#endregion

using System;
using System.Runtime.Serialization;

namespace Sonata.Diagnostics.Logs
{
	[DataContract(Name = "technicalLog")]
	public class TechnicalLog : Log
	{
		#region Constructors

		public TechnicalLog(Type source, LogLevels level, string message, Exception exception = null)
			: base(source, level, LogTypes.Technical, message, exception)
		{
		}

		public TechnicalLog(Type source, LogLevels level, string code, string message, Exception exception = null)
			: base(source, level, LogTypes.Technical, code, message, exception)
		{
		}

		#endregion

		#region Methods

		public void Write(string alternativeUsername = null)
		{
			if (Source == null)
				throw new InvalidOperationException("Source log can not be null.");

			Save(Source, LogTypes.Technical, alternativeUsername);
		}

		public static void Info(Type source, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Info, LogTypes.Technical, message, exception, alternativeUsername);
		}
		public static void Info(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Info, LogTypes.Technical, code, message, exception, alternativeUsername);
		}

		public static void Debug(Type source, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Debug, LogTypes.Technical, message, exception, alternativeUsername);
		}
		public static void Debug(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Debug, LogTypes.Technical, code, message, exception, alternativeUsername);
		}

		public static void Warning(Type source, string message,  Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Warning, LogTypes.Technical, message, exception, alternativeUsername);
		}
		public static void Warning(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Warning, LogTypes.Technical, code, message, exception, alternativeUsername);
		}

		public static void Error(Type source, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Error, LogTypes.Technical, message, exception, alternativeUsername);
		}
		public static void Error(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Error, LogTypes.Technical, code, message, exception, alternativeUsername);
		}

		public static void Fatal(Type source, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Fatal, LogTypes.Technical, message, exception, alternativeUsername);
		}
		public static void Fatal(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Fatal, LogTypes.Technical, code, message, exception, alternativeUsername);
		}

		#endregion
	}
}
