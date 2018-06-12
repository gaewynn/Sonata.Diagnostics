//#region Namespace Sonata.Diagnostics.Logs
////	TODO
//#endregion

//using Microsoft.Extensions.Logging;
//using System;
//using System.Runtime.Serialization;

//namespace Sonata.Diagnostics.Logs
//{
//	[DataContract(Name = "businessLog")]
//	public class BusinessLog : Log
//	{
//		#region Constructors

//		public BusinessLog(Type source, LogLevel level, string message, Exception exception = null)
//			: base(source, level, LogType.Business, message, exception)
//		{
//		}

//		public BusinessLog(Type source, LogLevel level, string code, string message, Exception exception = null)
//			: base(source, level, LogType.Business, code, message, exception)
//		{
//		}

//		#endregion

//		#region Methods

//		public void Write(string alternativeUsername = null)
//		{
//			if (Source == null)
//				throw new InvalidOperationException("Source log can not be null.");

//			Save(Source, LogType.Business, alternativeUsername);
//		}

//		public static void Info(Type source, string message, Exception exception = null, string alternativeUsername = null)
//		{
//			Save(source, LogLevel.Information, LogType.Business, null, message, exception, alternativeUsername);
//		}
//		public static void Info(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
//		{
//			Save(source, LogLevel.Information, LogType.Business, code, message, exception, alternativeUsername);
//		}

//		public static void Debug(Type source, string message, Exception exception = null, string alternativeUsername = null)
//		{
//			Save(source, LogLevel.Debug, LogType.Business, null, message, exception, alternativeUsername);
//		}
//		public static void Debug(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
//		{
//			Save(source, LogLevel.Debug, LogType.Business, code, message, exception, alternativeUsername);
//		}

//		public static void Warning(Type source, string message, Exception exception = null, string alternativeUsername = null)
//		{
//			Save(source, LogLevel.Warning, LogType.Business, null, message, exception, alternativeUsername);
//		}
//		public static void Warning(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
//		{
//			Save(source, LogLevel.Warning, LogType.Business, code, message, exception, alternativeUsername);
//		}

//		public static void Error(Type source, string message, Exception exception = null, string alternativeUsername = null)
//		{
//			Save(source, LogLevel.Error, LogType.Business, null, message, exception, alternativeUsername);
//		}
//		public static void Error(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
//		{
//			Save(source, LogLevel.Error, LogType.Business, code, message, exception, alternativeUsername);
//		}

//		public static void Fatal(Type source, string message, Exception exception = null, string alternativeUsername = null)
//		{
//			Save(source, LogLevel.Critical, LogType.Business, null, message, exception, alternativeUsername);
//		}
//		public static void Fatal(Type source, string code, string message, Exception exception = null, string alternativeUsername = null)
//		{
//			Save(source, LogLevel.Critical, LogType.Business, code, message, exception, alternativeUsername);
//		}

//		#endregion
//	}
//}
