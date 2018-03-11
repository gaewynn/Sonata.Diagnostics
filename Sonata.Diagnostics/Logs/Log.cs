#region Namespace Sonata.Diagnostics.Logs
//	TODO: comment
# endregion

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using log4net;
using Sonata.Core.Extensions;
using Sonata.Diagnostics.Logs.Converters;

namespace Sonata.Diagnostics.Logs
{
	[DataContract(Name = "log")]
	[KnownType(typeof(TechnicalLog))]
	[KnownType(typeof(BusinessLog))]
	public class Log
	{
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
		public LogLevels Level
		{
			get
			{
				try
				{
					return LevelName.GetEnumStringValue<LogLevels>();
				}
				catch (InvalidOperationException ioex)
				{
					var message =
						String.Format("The log level '{0}' is not valid. Please check in the log levels that the value belongs to the following list: [{1}]",
							LevelName,
							String.Join(" / ", typeof(LogLevels).GetAllStringValues()));

					Save(GetType(), LogLevels.Fatal, LogTypes.Technical, message, ioex);
				}

				return new LogLevels();
			}
		}

		[NotMapped]
		public LogTypes Type
		{
			get
			{
				try
				{
					return TypeName.GetEnumStringValue<LogTypes>();
				}
				catch (InvalidOperationException ioex)
				{
					var message =
						String.Format("The log type '{0}' is not valid. Please check in the log types that the value belongs to the following list: [{1}]",
							TypeName,
							String.Join(" / ", typeof(LogTypes).GetAllStringValues()));

					Save(GetType(), LogLevels.Fatal, LogTypes.Technical, message, ioex);
				}

				return new LogTypes();
			}
		}

		#endregion

		#region Constructors

		public Log()
		{ }

		internal Log(Type source, LogLevels level, LogTypes type, string message, Exception exception)
		{
			LevelName = level.GetStringValue();
			TypeName = type.GetStringValue();
			Source = source;
			Message = message;
			Exception = exception.GetFullMessage();
		}

		internal Log(Type source, LogLevels level, LogTypes type, string code, string message, Exception exception)
		{
			LevelName = level.GetStringValue();
			TypeName = type.GetStringValue();
			Source = source;
			Code = code;
			Message = message;
			Exception = exception.GetFullMessage();
		}

		#endregion

		#region Methods
		
		internal void Save(Type source, LogTypes type, string alternativeUsername = null)
		{
			if (String.IsNullOrWhiteSpace(Code))
				Save(source, Level, type, Message, String.IsNullOrWhiteSpace(Exception) ? null : new Exception(Exception), alternativeUsername);
			else
				Save(source, Level, type, Code, Message, String.IsNullOrWhiteSpace(Exception) ? null : new Exception(Exception), alternativeUsername);
		}

		internal static void Save(Type source, LogLevels level, LogTypes type, string message, Exception exception = null, string alternativeUsername = null)
		{
			Task.Run(() =>
			{
				if (String.IsNullOrWhiteSpace(message))
					throw new ArgumentNullException(nameof(message));

				LogicalThreadContext.Properties[SourcePatternConverter.SourcePropertyName] = source.FullName;

				if (!String.IsNullOrEmpty(alternativeUsername))
					LogicalThreadContext.Properties["alternativeUsername"] = alternativeUsername;

				var logger = LoggerConfiguration.LoggerProvider.Get(type);
				switch (level)
				{
					case LogLevels.Info:
						logger.Info(message, exception);
						break;

					case LogLevels.Debug:
						logger.Debug(message, exception);
						break;

					case LogLevels.Warning:
						logger.Warn(message, exception);
						break;

					case LogLevels.Error:
						logger.Error(message, exception);
						break;

					case LogLevels.Fatal:
						logger.Fatal(message, exception);
						break;
				}
			});
		}

		internal static void Save(Type source, LogLevels level, LogTypes type, string code, string message, Exception exception = null, string alternativeUsername = null)
		{
			Task.Run(() =>
			{
				if (String.IsNullOrWhiteSpace(code))
					throw new ArgumentNullException(nameof(code));
				if (String.IsNullOrWhiteSpace(message))
					throw new ArgumentNullException(nameof(message));

				LogicalThreadContext.Properties[CodePatternConverter.CodePropertyName] = code;
				LogicalThreadContext.Properties[SourcePatternConverter.SourcePropertyName] = source.FullName;

				if (!String.IsNullOrEmpty(alternativeUsername))
					LogicalThreadContext.Properties["alternativeUsername"] = alternativeUsername;

				var logger = LoggerConfiguration.LoggerProvider.Get(type);
				switch (level)
				{
					case LogLevels.Info:
						logger.Info(message, exception);
						break;

					case LogLevels.Debug:
						logger.Debug(message, exception);
						break;

					case LogLevels.Warning:
						logger.Warn(message, exception);
						break;

					case LogLevels.Error:
						logger.Error(message, exception);
						break;

					case LogLevels.Fatal:
						logger.Fatal(message, exception);
						break;
				}
			});
		}

		#endregion
	}
}
