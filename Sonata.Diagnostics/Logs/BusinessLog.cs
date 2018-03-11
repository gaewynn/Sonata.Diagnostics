#region Namespace Sonata.Diagnostics.Logs
//	TODO: comment
# endregion

using System;
using System.Runtime.Serialization;

namespace Sonata.Diagnostics.Logs
{
	[DataContract(Name = "businessLog")]
	public class BusinessLog : Log
	{
		#region Constructors

		public BusinessLog(Type source, LogLevels level, string message, Exception exception = null)
			: base(source, level, LogTypes.Business, message, exception)
		{
		}

		public BusinessLog(Type source, LogLevels level, string code, string message, Exception exception = null)
			: base(source, level, LogTypes.Business, code, message, exception)
		{
		}

		#endregion

		#region Methods

		public void Write(string alternativeUsername = null)
		{
			if (Source == null)
				throw new InvalidOperationException("Source log can not be null.");

			Save(Source, LogTypes.Business, alternativeUsername);
		}

		public static void Info(Type source, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Info, LogTypes.Business, message, exception, alternativeUsername);
		}
		public static void Info(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Info, LogTypes.Business, code, message, exception, alternativeUsername);
		}

		public static void Debug(Type source, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Debug, LogTypes.Business, message, exception, alternativeUsername);
		}
		public static void Debug(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Debug, LogTypes.Business, code, message, exception, alternativeUsername);
		}

		public static void Warning(Type source, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Warning, LogTypes.Business, message, exception, alternativeUsername);
		}
		public static void Warning(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Warning, LogTypes.Business, code, message, exception, alternativeUsername);
		}

		public static void Error(Type source, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Error, LogTypes.Business, message, exception, alternativeUsername);
		}
		public static void Error(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Error, LogTypes.Business, code, message, exception, alternativeUsername);
		}

		public static void Fatal(Type source, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Fatal, LogTypes.Business, message, exception, alternativeUsername);
		}
		public static void Fatal(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
		{
			Save(source, LogLevels.Fatal, LogTypes.Business, code, message, exception, alternativeUsername);
		}

		#endregion
	}
}
