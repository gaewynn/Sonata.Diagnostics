#region Namespace Sonata.Diagnostics.Extensions
//	TODO
#endregion

using Microsoft.Extensions.Logging;
using Sonata.Diagnostics.Logging;
using System;

namespace Sonata.Diagnostics.Extensions
{
	/// <summary>
	/// Represents a component adding functionalities to the <see cref="ILogger"/> interface.
	/// </summary>
	public static class LoggerExtension
	{
		#region Methods

		public static void LogTrace(this ILogger instance, Type source, string message, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogTrace(source, null, message, null, formatter);
		}

		public static void LogTrace(this ILogger instance, Type source, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogTrace(source, null, message, userName, formatter);
		}

		public static void LogTrace(this ILogger instance, Type source, string code, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogTrace(source.FullName, code, message, userName, formatter);
		}

		public static void LogTrace(this ILogger instance, string source, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogTrace(source, null, message, userName, formatter);
		}

		public static void LogTrace(this ILogger instance, string source, string code, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogTrace(Log4NetProperties.Build(userName, LogLevel.Trace, source, code, message, null, null).ToString());
		}

		public static void LogTrace(this ILogger instance, ILog4NetProperties state, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogTrace<ILog4NetProperties>(state, formatter);
		}

		public static void LogTrace<T>(this ILogger instance, T state, Func<T, Exception, string> formatter = null)
		{
			instance.Log(LogLevel.Trace, new EventId(), state, null, BuildFormatter(formatter));
		}

		public static void LogDebug(this ILogger instance, Type source, string message, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogDebug(source, null, message, null, formatter);
		}

		public static void LogDebug(this ILogger instance, Type source, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogDebug(source, null, message, userName, formatter);
		}

		public static void LogDebug(this ILogger instance, Type source, string code, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogDebug(source.FullName, code, message, userName, formatter);
		}

		public static void LogDebug(this ILogger instance, string source, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogDebug(source, null, message, userName, formatter);
		}

		public static void LogDebug(this ILogger instance, string source, string code, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogDebug(Log4NetProperties.Build(userName, LogLevel.Debug, source, code, message, null, null).ToString());
		}

		public static void LogDebug(this ILogger instance, ILog4NetProperties state, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogDebug<ILog4NetProperties>(state, formatter);
		}

		public static void LogDebug<T>(this ILogger instance, T state, Func<T, Exception, string> formatter = null)
		{
			instance.Log(LogLevel.Debug, new EventId(), state, null, BuildFormatter(formatter));
		}

		public static void LogInformation(this ILogger instance, Type source, string message, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogInformation(source, null, message, null, formatter);
		}

		public static void LogInformation(this ILogger instance, Type source, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogInformation(source, null, message, userName, formatter);
		}

		public static void LogInformation(this ILogger instance, Type source, string code, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogInformation(source.FullName, code, message, userName, formatter);
		}

		public static void LogInformation(this ILogger instance, string source, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogInformation(source, null, message, userName, formatter);
		}

		public static void LogInformation(this ILogger instance, string source, string code, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogInformation(Log4NetProperties.Build(userName, LogLevel.Information, source, code, message, null, null).ToString());
		}

		public static void LogInformation(this ILogger instance, ILog4NetProperties state, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogInformation<ILog4NetProperties>(state, formatter);
		}

		public static void LogInformation<T>(this ILogger instance, T state, Func<T, Exception, string> formatter = null)
		{
			instance.Log(LogLevel.Information, new EventId(), state, null, BuildFormatter(formatter));
		}

		public static void LogWarning(this ILogger instance, Type source, string message, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogWarning(source, null, message, null, formatter);
		}

		public static void LogWarning(this ILogger instance, Type source, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogWarning(source, null, message, userName, formatter);
		}

		public static void LogWarning(this ILogger instance, Type source, string code, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogWarning(source.FullName, code, message, userName, formatter);
		}

		public static void LogWarning(this ILogger instance, string source, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogWarning(source, null, message, userName, formatter);
		}

		public static void LogWarning(this ILogger instance, string source, string code, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogWarning(Log4NetProperties.Build(userName, LogLevel.Warning, source, code, message, null, null).ToString());
		}

		public static void LogWarning(this ILogger instance, ILog4NetProperties state, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogWarning<ILog4NetProperties>(state, formatter);
		}

		public static void LogWarning<T>(this ILogger instance, T state, Func<T, Exception, string> formatter = null)
		{
			instance.Log(LogLevel.Warning, new EventId(), state, null, BuildFormatter(formatter));
		}

		public static void LogError(this ILogger instance, Type source, string message, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogError(source, null, message, null, formatter);
		}

		public static void LogError(this ILogger instance, Type source, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogError(source, null, message, userName, formatter);
		}

		public static void LogError(this ILogger instance, Type source, string code, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogError(source.FullName, code, message, userName, formatter);
		}

		public static void LogError(this ILogger instance, string source, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogError(source, null, message, userName, formatter);
		}

		public static void LogError(this ILogger instance, string source, string code, string message, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogError(Log4NetProperties.Build(userName, LogLevel.Error, source, code, message, null, null).ToString());
		}

		public static void LogError(this ILogger instance, ILog4NetProperties state, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogError<ILog4NetProperties>(state, formatter);
		}

		public static void LogError<T>(this ILogger instance, T state, Func<T, Exception, string> formatter = null)
		{
			instance.Log(LogLevel.Error, new EventId(), state, null, BuildFormatter(formatter));
		}

		public static void LogCritical(this ILogger instance, Type source, string message, Exception exception, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogCritical(source, null, message, exception, null, formatter);
		}

		public static void LogCritical(this ILogger instance, Type source, string message, Exception exception, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogCritical(source, null, message, exception, userName, formatter);
		}

		public static void LogCritical(this ILogger instance, Type source, string code, string message, Exception exception, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogCritical(exception, Log4NetProperties.Build(userName, LogLevel.Critical, source, code, message, null, exception).ToString());
		}

		public static void LogCritical(this ILogger instance, string source, string message, Exception exception, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogCritical(source, null, message, userName, exception, formatter);
		}

		public static void LogCritical(this ILogger instance, string source, string code, string message, Exception exception, string userName, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogCritical(exception, Log4NetProperties.Build(userName, LogLevel.Critical, source, code, message, null, exception).ToString());
		}

		public static void LogCritical(this ILogger instance, ILog4NetProperties state, Exception exception, Func<ILog4NetProperties, Exception, string> formatter = null)
		{
			instance.LogCritical<ILog4NetProperties>(state, exception, formatter);
		}

		public static void LogCritical<T>(this ILogger instance, T state, Exception exception, Func<T, Exception, string> formatter = null)
		{
			instance.Log(LogLevel.Critical, new EventId(), state, exception, BuildFormatter(formatter));
		}

		private static Func<T, Exception, string> BuildFormatter<T>(Func<T, Exception, string> formatter)
		{
			if (formatter != null)
				return formatter;

			if (typeof(T) == typeof(ILog4NetProperties))
				return (log4NetProperties, exception) => ((ILog4NetProperties)log4NetProperties).Message;

			return (state, exception) => state?.ToString();
		}

		#endregion
	}
}
