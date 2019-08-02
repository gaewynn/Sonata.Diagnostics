#region Namespace Sonata.Diagnostics.Logs
//	TODO
#endregion

using log4net;
using Microsoft.Extensions.Logging;
using Sonata.Core.Extensions;
using Sonata.Diagnostics.Logs.Converters;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Sonata.Diagnostics.Logs
{
    [DataContract(Name = "log")]
	public class Log : ILogger
	{
		#region Members

		private readonly ILog _log;
		private Func<object, Exception, string> _exceptionDetailsFormatter;
		private ILog _technicalLogger = Log.GetTechnicalLogger(typeof(Log));

		#endregion

		#region Properties

		[Column("LOG_id")]
		[DataMember(Name = "logId")]
		public int LogId { get; set; }

		[Column("LOG_code")]
		[DataMember(Name = "code")]
		public string Code { get; set; }

		[Column("LOG_thread")]
		[DataMember(Name = "thread")]
		public string Thread { get; set; }

		[Column("LOG_levelName")]
		[DataMember(Name = "levelName")]
		public string LevelName { get; set; }

		[Column("LOG_typeName")]
		[DataMember(Name = "typeName")]
		public string TypeName { get; set; }

		public Type Source { get; set; }

		[Column("LOG_sourceName")]
		[DataMember(Name = "sourceName")]
		public string SourceName { get; set; }

		[Column("LOG_message")]
		[DataMember(Name = "message")]
		public string Message { get; set; }

		[Column("LOG_exception")]
		[DataMember(Name = "exception")]
		public string Exception { get; set; }

		[NotMapped]
		public LogLevel Level
		{
			get
			{
				try
				{
					return LevelName.GetEnumStringValue<LogLevel>();
				}
				catch (InvalidOperationException ioex)
				{
					var message =
						String.Format("The log level '{0}' is not valid. Please check in the log levels that the value belongs to the following list: [{1}]",
							LevelName,
							String.Join(" / ", typeof(LogLevel).GetAllStringValues()));

					
					//Save(GetType(), LogLevel.Critical, LogType.Technical, null, message, ioex);
				}

				return new LogLevel();
			}
		}

		[NotMapped]
		public LogType Type
		{
			get
			{
				try
				{
					return TypeName.GetEnumStringValue<LogType>();
				}
				catch (InvalidOperationException ioex)
				{
					var message =
						String.Format("The log type '{0}' is not valid. Please check in the log types that the value belongs to the following list: [{1}]",
							TypeName,
							String.Join(" / ", typeof(LogType).GetAllStringValues()));

					//Save(GetType(), LogLevel.Critical, LogType.Technical, null, message, ioex);
				}

				return new LogType();
			}
		}

		#endregion

		#region Constructors

		internal Log(string loggerRepository, string name)
		{
			_log = LogManager.GetLogger(loggerRepository, name);
			_technicalLogger = GetTechnicalLogger(typeof(Log));
		}

		internal Log(string loggerRepository, LogType type)
			: this(loggerRepository, type.GetStringValue())
		{ }

		internal Log(MemberInfo loggerRepository, string name)
			: this(loggerRepository.Name, name)
		{ }

		internal Log(MemberInfo loggerRepository, LogType type)
			: this(loggerRepository.Name, type.GetStringValue())
		{ }

		//internal Log(Type source, LogLevel level, LogType type, string message, Exception exception)
		//	: this(source, level, type, null, message, exception)
		//{ }

		//internal Log(Type source, LogLevel level, LogType type, string code, string message, Exception exception)
		//{
		//	_log = LogManager.GetLogger(typeof(Log).Assembly, type.GetStringValue());

		//	LevelName = level.GetStringValue();
		//	TypeName = type.GetStringValue();
		//	Source = source;
		//	Code = code;
		//	Message = message;
		//	Exception = exception.GetFullMessage();
		//}

		#endregion

		#region Methods

		#region ILogger Members

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
				case LogLevel.None:
					return false;
				default:
					throw new ArgumentOutOfRangeException(nameof(logLevel));
			}
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			return null;
		}

		void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
				return;

			if (formatter == null)
				throw new ArgumentNullException(nameof(formatter));

			var message = formatter(state, exception);

			if (exception != null && _exceptionDetailsFormatter != null)
				message = _exceptionDetailsFormatter(message, exception);

			if (String.IsNullOrEmpty(message) && exception == null)
				return;

			switch (logLevel)
			{
				case LogLevel.Debug:
				case LogLevel.Trace:
					_log.Debug(message);
					break;
				case LogLevel.Information:
					_log.Info(message);
					break;
				case LogLevel.Warning:
					_log.Warn(message);
					break;
				case LogLevel.Error:
					_log.Error(message);
					break;
				case LogLevel.Critical:
					_log.Fatal(message, exception);
					break;
				default:
					_log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
					_log.Info(message, exception);
					break;
			}
		}

		#endregion

		public Log UsingCustomExceptionFormatter(Func<object, Exception, string> formatter)
		{
			_exceptionDetailsFormatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
			return this;
		}

		public void Save( /*Type source, */ LogLevel level, /*LogType type, string code, */string message,
				Exception exception = null/*,  string alternativeUsername = null, ILog logger = null*/)
		{
			Save(level, null, message, exception);
		}

		public void Save(/*Type source, */LogLevel level, /*LogType type, */string code, string message, Exception exception = null/*, string alternativeUsername = null, ILog logger = null*/)
		//public void Save(Type source, LogType type, string alternativeUsername = null)
		{
			//Save(source, Level, type, Code, Message, String.IsNullOrWhiteSpace(Exception) ? null : new Exception(Exception), alternativeUsername);





			Task.Run(() =>
			{
				if (String.IsNullOrWhiteSpace(Message))
					throw new ArgumentNullException(nameof(Message));

				if (!String.IsNullOrWhiteSpace(Code))
					LogicalThreadContext.Properties[CodePatternConverter.CodePropertyName] = Code;

				//LogicalThreadContext.Properties[SourcePatternConverter.SourcePropertyName] = source.FullName;

				/*if (!String.IsNullOrEmpty(alternativeUsername))
					LogicalThreadContext.Properties["alternativeUsername"] = alternativeUsername;*/

				//var exception = String.IsNullOrWhiteSpace(Exception) ? null : new Exception(Exception);
				switch (Level)
				{
					case LogLevel.Information:
						_log.Info(Message, exception);
						break;

					case LogLevel.Debug:
						_log.Debug(Message, exception);
						break;

					case LogLevel.Warning:
						_log.Warn(Message, exception);
						break;

					case LogLevel.Error:
						_log.Error(Message, exception);
						break;

					case LogLevel.Critical:
						_log.Fatal(Message, exception);
						break;

					default:
						_log.Warn($"Encountered unknown log level {Level}, writing out as Info.");
						_log.Info(Message, exception);
						break;
				}
			});
		}

		//internal static void Save(Type source, LogLevel level, LogType type, string code, string message, Exception exception = null, string alternativeUsername = null, ILog logger = null)
		//{
		//	Task.Run(() =>
		//	{
		//		if (String.IsNullOrWhiteSpace(message))
		//			throw new ArgumentNullException(nameof(message));

		//		if (!String.IsNullOrWhiteSpace(code))
		//			LogicalThreadContext.Properties[CodePatternConverter.CodePropertyName] = code;

		//		LogicalThreadContext.Properties[SourcePatternConverter.SourcePropertyName] = source.FullName;

		//		if (!String.IsNullOrEmpty(alternativeUsername))
		//			LogicalThreadContext.Properties["alternativeUsername"] = alternativeUsername;

		//		logger = logger ?? LogManager.GetLogger(typeof(Log4NetProvider).Assembly, type.GetStringValue());
		//		switch (level)
		//		{
		//			case LogLevel.Information:
		//				logger.Info(message, exception);
		//				break;

		//			case LogLevel.Debug:
		//				logger.Debug(message, exception);
		//				break;

		//			case LogLevel.Warning:
		//				logger.Warn(message, exception);
		//				break;

		//			case LogLevel.Error:
		//				logger.Error(message, exception);
		//				break;

		//			case LogLevel.Critical:
		//				logger.Fatal(message, exception);
		//				break;

		//			default:
		//				logger.Warn($"Encountered unknown log level {level}, writing out as Info.");
		//				logger.Info(message, exception);
		//				break;
		//		}
		//	});
		//}

		public static ILog GetTechnicalLogger(Type source)
		{
			return GetLogger(source, LogType.Technical.GetStringValue());
		}

		public static ILog GetBusinessLogger(Type source)
		{
			return GetLogger(source, LogType.Business.GetStringValue());
		}

		public static ILog GetLogger(Type source, string name)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name));

			return LogManager.GetLogger(source.Name, name);
		}

		#endregion
	}
}
