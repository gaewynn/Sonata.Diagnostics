#region Namespace Sonata.Diagnostics.Logging
//	TODO
#endregion

using log4net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using Newtonsoft.Json;
using Sonata.Diagnostics.Extensions;
using Sonata.Diagnostics.Logging.Converters;
using System;
using System.Text;

namespace Sonata.Diagnostics.Logging
{
	internal class Log4NetLogger : ILogger
	{
		#region Members

		private static readonly string _loglevelPadding = ": ";
		private static readonly string MessagePadding;
		private readonly ILog _log;
		private readonly Log4NetProviderOptions _options;

		[ThreadStatic]
		private static StringBuilder _logBuilder;

		private Func<object, Exception, string> _exceptionFormatter;

		#endregion

		#region Properties

		internal IExternalScopeProvider ScopeProvider { get; set; }

		#endregion

		#region Constructors

		static Log4NetLogger()
		{
			var logLevelString = LogLevel.Information.GetStringValue();
			MessagePadding = new string(' ', logLevelString.Length + _loglevelPadding.Length);
		}

		public Log4NetLogger(string loggerRepository, string name, Log4NetProviderOptions options)
			: this(loggerRepository, name, options, options.IncludeScopes ? new LoggerExternalScopeProvider() : null)
		{ }

		internal Log4NetLogger(string loggerRepository, string name, Log4NetProviderOptions options, IExternalScopeProvider scopeProvider)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			_options = options;
			_log = LogManager.GetLogger(loggerRepository, name);

			ScopeProvider = scopeProvider;
		}

		#endregion

		#region Methods

		#region ILogger Members

		public IDisposable BeginScope<TState>(TState state) => ScopeProvider?.Push(state) ?? NullScope.Instance;

		public bool IsEnabled(LogLevel logLevel)
		{
			switch (logLevel)
			{
				case LogLevel.Critical:
					return _log.IsFatalEnabled;
				case LogLevel.Debug:
				case LogLevel.Trace:
					return _log.IsDebugEnabled;
				case LogLevel.Error:
					return _log.IsErrorEnabled;
				case LogLevel.Information:
					return _log.IsInfoEnabled;
				case LogLevel.Warning:
					return _log.IsWarnEnabled;
				default:
					throw new ArgumentOutOfRangeException(nameof(logLevel));
			}
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (state == null)
				return;
			if (!IsEnabled(logLevel))
				return;

			var log4NetProperties = FromState(state.ToString()) ?? _options.PropertiesAccessor(state);
			if (log4NetProperties == null || (String.IsNullOrEmpty(log4NetProperties.Message) && exception == null))
				return;

			var logBuilder = _logBuilder;
			_logBuilder = null;

			if (logBuilder == null)
				logBuilder = new StringBuilder();

			GetScopeInformation(logBuilder);

			if (!String.IsNullOrEmpty(log4NetProperties.Message))
			{
				//logBuilder.Append(MessagePadding);
				logBuilder.Append(log4NetProperties.Message);
			}

			if (!String.IsNullOrWhiteSpace(log4NetProperties.Code))
				LogicalThreadContext.Properties[PatternConverter.CodePropertyName] = log4NetProperties.Code;

			if (!String.IsNullOrWhiteSpace(log4NetProperties.Thread))
				LogicalThreadContext.Properties[PatternConverter.ThreadNamePropertyName] = log4NetProperties.Thread;

			if (!String.IsNullOrWhiteSpace(log4NetProperties.Source))
				LogicalThreadContext.Properties[PatternConverter.SourcePropertyName] = log4NetProperties.Source;

			LogicalThreadContext.Properties[PatternConverter.UserNamePropertyName] =
				!String.IsNullOrWhiteSpace(log4NetProperties.UserName)
					? log4NetProperties.UserName
					: _options.UserNameAccessor(state);

			switch (logLevel)
			{
				case LogLevel.Debug:
				case LogLevel.Trace: _log.Debug(logBuilder.ToString()); break;
				case LogLevel.Information: _log.Info(logBuilder.ToString()); break;
				case LogLevel.Warning: _log.Warn(logBuilder.ToString()); break;
				case LogLevel.Error: _log.Error(logBuilder.ToString()); break;
				case LogLevel.Critical: _log.Fatal(logBuilder.ToString(), exception); break;
				default:
					_log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
					_log.Info(logBuilder.ToString(), exception);
					break;
			}
		}

		internal Log4NetLogger UsingExceptionFormatter(Func<object, Exception, string> exceptionFormatter)
		{
			_exceptionFormatter = exceptionFormatter;
			return this;
		}

		#endregion

		private void GetScopeInformation(StringBuilder stringBuilder)
		{
			var scopeProvider = ScopeProvider;
			if (scopeProvider == null)
				return;

			var initialLength = stringBuilder.Length;

			scopeProvider.ForEachScope((scope, state) =>
			{
				var (builder, length) = state;
				var first = length == builder.Length;
				builder.Append(first ? "=> " : " => ").Append(scope);
			}, (stringBuilder, initialLength));

			if (stringBuilder.Length <= initialLength)
				return;

			stringBuilder.Insert(initialLength, MessagePadding);
			stringBuilder.AppendLine();
		}

		private ILog4NetProperties FromState(string state)
		{
			try
			{
				return JsonConvert.DeserializeObject<Log4NetProperties>(state);
			}
			catch (Exception)
			{
				return null;
			}
		}

		#endregion
	}
}
