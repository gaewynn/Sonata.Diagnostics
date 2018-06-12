using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.Logging;
using Sonata.Core.Extensions;
using Sonata.Diagnostics.Logs.Appenders;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Sonata.Diagnostics.Logs
{
	/// <inheritdoc />
	/// <summary>
	/// The log4net provider class.
	/// </summary>
	internal class Log4NetProvider : ILoggerProvider
	{
		#region Nested Classes

		/// <summary>
		/// Convert a base data type to another base data type
		/// </summary>
		private static class TypeConvertor
		{
			#region Nested Structs

			private struct DbTypeMapEntry
			{
				public readonly DbType DbType;
				public readonly SqlDbType SqlDbType;

				public DbTypeMapEntry(DbType dbType, SqlDbType sqlDbType)
				{
					DbType = dbType;
					SqlDbType = sqlDbType;
				}
			};

			#endregion

			#region Members

			private static readonly ArrayList DbTypeList = new ArrayList();

			#endregion

			#region Constructors

			static TypeConvertor()
			{
				var dbTypeMapEntry = new DbTypeMapEntry(DbType.Boolean, SqlDbType.Bit);
				DbTypeList.Add(dbTypeMapEntry);

				dbTypeMapEntry = new DbTypeMapEntry(DbType.Double, SqlDbType.TinyInt);
				DbTypeList.Add(dbTypeMapEntry);

				dbTypeMapEntry = new DbTypeMapEntry(DbType.Binary, SqlDbType.Image);
				DbTypeList.Add(dbTypeMapEntry);

				dbTypeMapEntry = new DbTypeMapEntry(DbType.DateTime, SqlDbType.DateTime);
				DbTypeList.Add(dbTypeMapEntry);

				dbTypeMapEntry = new DbTypeMapEntry(DbType.Decimal, SqlDbType.Decimal);
				DbTypeList.Add(dbTypeMapEntry);

				dbTypeMapEntry = new DbTypeMapEntry(DbType.Double, SqlDbType.Float);
				DbTypeList.Add(dbTypeMapEntry);

				dbTypeMapEntry = new DbTypeMapEntry(DbType.Guid, SqlDbType.UniqueIdentifier);
				DbTypeList.Add(dbTypeMapEntry);

				dbTypeMapEntry = new DbTypeMapEntry(DbType.Int16, SqlDbType.SmallInt);
				DbTypeList.Add(dbTypeMapEntry);

				dbTypeMapEntry = new DbTypeMapEntry(DbType.Int32, SqlDbType.Int);
				DbTypeList.Add(dbTypeMapEntry);

				dbTypeMapEntry = new DbTypeMapEntry(DbType.Int64, SqlDbType.BigInt);
				DbTypeList.Add(dbTypeMapEntry);

				dbTypeMapEntry = new DbTypeMapEntry(DbType.Object, SqlDbType.Variant);
				DbTypeList.Add(dbTypeMapEntry);

				dbTypeMapEntry = new DbTypeMapEntry(DbType.String, SqlDbType.VarChar);
				DbTypeList.Add(dbTypeMapEntry);
			}

			#endregion

			#region Methods

			/// <summary>
			/// Convert DbType type to TSQL data type
			/// </summary>
			/// <param name="dbType"></param>
			/// <returns></returns>
			public static SqlDbType ToSqlDbType(DbType dbType)
			{
				var entry = Find(dbType);
				return entry.SqlDbType;
			}

			private static DbTypeMapEntry Find(DbType dbType)
			{
				object retObj = null;
				foreach (var currentDbType in DbTypeList)
				{
					var entry = (DbTypeMapEntry)currentDbType;
					if (entry.DbType != dbType)
						continue;

					retObj = entry;
					break;
				}

				if (retObj == null)
					throw new ApplicationException("Referenced an unsupported DbType");

				return (DbTypeMapEntry)retObj;
			}

			#endregion
		}

		#endregion

		#region Members

		/// <summary>
		/// The log4net repository.
		/// </summary>
		private readonly ILoggerRepository _loggerRepository;

		/// <summary>
		/// The exception formatter Func.
		/// </summary>
		private readonly Func<object, Exception, string> _exceptionFormatter;

		/// <summary>
		/// The loggers collection.
		/// </summary>
		private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new ConcurrentDictionary<string, Log4NetLogger>();

		#endregion

		#region Properties

		public static bool IsBusinessLogTableCreated { get; set; }

		public bool UseInternalBusinessRollingFileConfiguration { get; set; }

		public bool UseInternalTechnicalRollingFileConfiguration { get; set; }

		public bool UseInternalBusinessAdoNetConfiguration { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Log4NetProvider"/> class.
		/// </summary>
		/// <param name="log4NetConfigFile">The log4 net configuration file.</param>
		public Log4NetProvider(string log4NetConfigFile)
			: this(log4NetConfigFile, null)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="Log4NetProvider"/> class.
		/// </summary>
		/// <param name="log4NetConfigFile">The log4NetConfigFile.</param>
		public Log4NetProvider(string log4NetConfigFile, Func<object, Exception, string> exceptionFormatter = null)
		{
			_exceptionFormatter = exceptionFormatter ?? FormatExceptionByDefault;
			_loggerRepository = LogManager.CreateRepository(Assembly.GetEntryAssembly(),
				typeof(log4net.Repository.Hierarchy.Hierarchy));

			if (!String.IsNullOrWhiteSpace(log4NetConfigFile))
				Setup(
					null, false, null, null, false, null, null, null, LogLevel.Trace, LogLevel.Trace,
					exceptionFormatter);
		}

		public Log4NetProvider(
			string technicalLogFileFullname, LogLevel technicalMinimumLogLevel,
			string businessLogFileFullname, LogLevel businessMinimumLogLevel,
			Func<object, Exception, string> exceptionFormatter = null)
			: this(null, exceptionFormatter)
		{
			if (String.IsNullOrWhiteSpace(technicalLogFileFullname))
				throw new ArgumentNullException(nameof(technicalLogFileFullname));

			Setup(
				null,
				true, businessLogFileFullname, technicalLogFileFullname,
				false, null, null, null,
				businessMinimumLogLevel,
				technicalMinimumLogLevel,
				exceptionFormatter);
		}

		public Log4NetProvider(
			Type connectionType, string connectionStringName, string logTableName, LogLevel businessMinimumLogLevel,
			Func<object, Exception, string> exceptionFormatter = null)
			: this(null, exceptionFormatter)
		{
			if (connectionType == null)
				throw new ArgumentNullException(nameof(connectionType));
			if (String.IsNullOrWhiteSpace(connectionStringName))
				throw new ArgumentNullException(nameof(connectionStringName));
			if (String.IsNullOrWhiteSpace(logTableName))
				throw new ArgumentNullException(nameof(logTableName));

			Setup(
				null, false, null, null,
				true, connectionType, connectionStringName, logTableName, businessMinimumLogLevel, LogLevel.Trace,
				exceptionFormatter);
		}

		public Log4NetProvider(
			string technicalLogFileFullname, Type connectionType, string connectionStringName, string logTableName, string businessLogFileFullname,
			LogLevel businessMinimumLogLevel, LogLevel technicalMinimumLogLevel,
			Func<object, Exception, string> exceptionFormatter = null)
		{
			if (String.IsNullOrWhiteSpace(technicalLogFileFullname))
				throw new ArgumentNullException(nameof(technicalLogFileFullname));
			if (connectionType == null)
				throw new ArgumentNullException(nameof(connectionType));
			if (String.IsNullOrWhiteSpace(connectionStringName))
				throw new ArgumentNullException(nameof(connectionStringName));
			if (String.IsNullOrWhiteSpace(logTableName))
				throw new ArgumentNullException(nameof(logTableName));

			Setup(
				null, true, businessLogFileFullname, technicalLogFileFullname, true,
				connectionType, connectionStringName, logTableName, businessMinimumLogLevel, technicalMinimumLogLevel,
				exceptionFormatter);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Creates the logger.
		/// </summary>
		/// <param name="categoryName">The category name.</param>
		/// <returns>The <see cref="ILogger"/> instance.</returns>
		public ILogger CreateLogger(string categoryName)
		{
			return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		internal void Setup(
			string applicationConfigurationFileFullName,
			bool useInternalRollingFileConfiguration, string businessLogFileFullname, string technicalLogFileFullname,
			bool useInternalAdoNetConfiguration, Type connectionType, string connectionStringName, string logTableName,
			LogLevel businessMinimumLogLevel = LogLevel.Trace,
			LogLevel technicalMinimumLogLevel = LogLevel.Trace,
			Func<object, Exception, string> exceptionFormatter = null)
		{
			UseInternalBusinessRollingFileConfiguration = useInternalRollingFileConfiguration && !String.IsNullOrWhiteSpace(businessLogFileFullname);
			UseInternalTechnicalRollingFileConfiguration = useInternalRollingFileConfiguration && !String.IsNullOrWhiteSpace(technicalLogFileFullname);
			UseInternalBusinessAdoNetConfiguration = useInternalAdoNetConfiguration;

			if (!UseInternalBusinessRollingFileConfiguration && !UseInternalTechnicalRollingFileConfiguration && !UseInternalBusinessAdoNetConfiguration)
			{
				if (!String.IsNullOrWhiteSpace(applicationConfigurationFileFullName))
					XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()), Parselog4NetConfigFile(applicationConfigurationFileFullName));
				else
					XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()));
			}
			else
			{
				var log4NetElement = GetDefaultConfiguration(
					businessLogFileFullname ?? String.Empty,
					businessMinimumLogLevel,
					technicalLogFileFullname ?? String.Empty,
					technicalMinimumLogLevel,
					connectionType ?? GetType(),
					connectionStringName ?? String.Empty,
					logTableName ?? String.Empty);

				if (!UseInternalTechnicalRollingFileConfiguration)
				{
					var technicalRollingFileAppenderElement = log4NetElement.DescendantsAndSelf().First(e => e.HasAttributes && e.Attributes("name").Any() && e.Attributes("name").ElementAt(0).Value == "TechnicalRollingFileAppender");
					technicalRollingFileAppenderElement.Remove();

					var technicalLoggerElement = log4NetElement.DescendantsAndSelf().First(e => e.HasAttributes && e.Attributes("ref").Any() && e.Attributes("ref").ElementAt(0).Value == "TechnicalRollingFileAppender");
					technicalLoggerElement.Remove();
				}

				if (!UseInternalBusinessRollingFileConfiguration)
				{
					var businessRollingFileAppenderElement = log4NetElement.DescendantsAndSelf().First(e => e.HasAttributes && e.Attributes("name").Any() && e.Attributes("name").ElementAt(0).Value == "BusinessRollingFileAppender");
					businessRollingFileAppenderElement.Remove();

					var businessLoggerElement = log4NetElement.DescendantsAndSelf().First(e => e.HasAttributes && e.Attributes("ref").Any() && e.Attributes("ref").ElementAt(0).Value == "BusinessRollingFileAppender");
					businessLoggerElement.Remove();
				}

				if (!UseInternalBusinessAdoNetConfiguration)
				{
					var businessRollingFileAppenderElement = log4NetElement.DescendantsAndSelf().First(e => e.HasAttributes && e.Attributes("name").Any() && e.Attributes("name").ElementAt(0).Value == "BusinessAdoNetAppender");
					businessRollingFileAppenderElement.Remove();

					var businessLoggerElement = log4NetElement.DescendantsAndSelf().First(e => e.HasAttributes && e.Attributes("ref").Any() && e.Attributes("ref").ElementAt(0).Value == "BusinessAdoNetAppender");
					businessLoggerElement.Remove();
				}

				XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), log4NetElement.ToXmlElement());
			}

			var loggers = LogManager.GetCurrentLoggers(Assembly.GetCallingAssembly());
			if (!IsBusinessLogTableCreated)
			{
				AdoNetAppender adoNetAppender = null;
				if (loggers.Any())
				{
					foreach (var adoAppender in loggers.Select(
						logger => logger.Logger.Repository.GetAppenders().FirstOrDefault(a => a.GetType() == typeof(AdoNetAppender)))
						.Where(adoAppender => adoAppender != null).OfType<AdoNetAppender>())
					{
						adoNetAppender = adoAppender;
						break;
					}
				}

				if (adoNetAppender != null)
				{
					var connectionString = ConfigurationManager.ConnectionStrings[adoNetAppender.ConnectionStringName].ConnectionString;
					var field = typeof(AdoNetAppender).GetField("m_parameters", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
					if (field == null)
					{
						Debug.WriteLine("Can not find field name 'm_parameters' in AdoNetAppender class using reflection.");
						throw new InvalidOperationException("Can not find field name 'm_parameters' in AdoNetAppender class using reflection.", new NullReferenceException("field"));
					}

					if (field.GetValue(adoNetAppender) is ArrayList parameters)
					{
						var fields = adoNetAppender.CommandText.Split(' ')[3].Split(',').Select(e => e.Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "")).ToList();
						logTableName = adoNetAppender.CommandText.Split(' ')[2];

						var tableFieldsList = new List<string> { "[LOG_id] [bigint] IDENTITY(1,1) NOT NULL" };
						for (var i = 0; i < parameters.Count; i++)
						{
							var parameter = (AdoNetAppenderParameter)parameters[i];
							var tableField = new StringBuilder();

							tableField.AppendFormat("[{0}] [{1}]{2} {3}",
								fields[i],
								TypeConvertor.ToSqlDbType(parameter.DbType),
								parameter.DbType == DbType.AnsiString || parameter.DbType == DbType.AnsiStringFixedLength ||
								parameter.DbType == DbType.String || parameter.DbType == DbType.StringFixedLength ||
								parameter.DbType == DbType.Xml
									? String.Format("({0})", parameter.Size)
									: String.Empty,
								"NULL");

							tableFieldsList.Add(tableField.ToString());
						}

						using (var connection = new SqlConnection(connectionString))
						{
							try
							{
								connection.Open();
								using (var command = connection.CreateCommand())
								{
									var nameParameter = command.CreateParameter();
									nameParameter.DbType = DbType.String;
									nameParameter.ParameterName = "name";
									nameParameter.Value = logTableName.ToLower();
									command.Parameters.Add(nameParameter);

									command.CommandText = "IF NOT EXISTS (SELECT * FROM sys.tables WHERE LOWER(name) = @name) " + Environment.NewLine;
									command.CommandText += "BEGIN " + Environment.NewLine;
									command.CommandText += String.Format("CREATE TABLE {0} (", logTableName) + Environment.NewLine;

									foreach (var tableField in tableFieldsList)
										command.CommandText += tableField + ", " + Environment.NewLine;

									command.CommandText += String.Format("CONSTRAINT [PK_dbo.{0}] PRIMARY KEY CLUSTERED ", logTableName) + Environment.NewLine;
									command.CommandText += "( " + Environment.NewLine;
									command.CommandText += "[LOG_id] ASC " + Environment.NewLine;
									command.CommandText += ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " + Environment.NewLine;
									command.CommandText += ") ON [PRIMARY] " + Environment.NewLine;
									command.CommandText += "END";

									Debug.WriteLine("Creating logs table: " + command.CommandText);

									command.ExecuteNonQuery();
								}
							}
							finally
							{
								if (connection.State == ConnectionState.Open)
									connection.Close();
							}
						}
					}
					else
						Console.WriteLine(@"dbCommand is null.");
				}
			}

			IsBusinessLogTableCreated = true;
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{ }

			_loggers.Clear();
		}

		/// <summary>
		/// Parses log4net config file.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <returns>The <see cref="XmlElement"/> with the log4net XML element.</returns>
		private static XmlElement Parselog4NetConfigFile(string filename)
		{
			using (var configurationFile = File.OpenRead(filename))
			{
				var settings = new XmlReaderSettings
				{
					DtdProcessing = DtdProcessing.Prohibit
				};

				var log4netConfig = new XmlDocument();
				using (var reader = XmlReader.Create(configurationFile, settings))
					log4netConfig.Load(reader);

				return log4netConfig["log4net"];
			}
		}

		/// <summary>
		/// Creates the logger implementation.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>The <see cref="Log4NetLogger"/> instance.</returns>
		private Log4NetLogger CreateLoggerImplementation(string name)
			=> new Log4NetLogger(_loggerRepository.Name, name)
					.UsingCustomExceptionFormatter(_exceptionFormatter);

		/// <summary>
		/// Formats an exception by default.
		/// </summary>
		/// <typeparam name="TState">The type of the state.</typeparam>
		/// <param name="state">The state of the logged object.</param>
		/// <param name="exception">The exception to be logged.</param>
		/// <returns>The text with the formatted message.</returns>
		private static string FormatExceptionByDefault<TState>(TState state, Exception exception)
		{
			var builder = new StringBuilder();
			builder.Append(state);
			builder.Append(" - ");

			if (exception != null)
				builder.Append(exception);

			return builder.ToString();
		}

		private static XElement GetDefaultConfiguration(
			string businessLogFileFullname, LogLevel businessMinimumLogLevel,
			string technicalLogFileFullname, LogLevel technicalMinimumLogLevel,
			Type connectionType, string connectionStringName, string logTableName)
		{
			return new XElement("log4net",

					new XElement("appender", new XAttribute("name", "DebugAppender"), new XAttribute("type", "log4net.Appender.DebugAppender"),
						new XElement("immediateFlush", new XAttribute("value", "true")),
						new XElement("layout", new XAttribute("type", "Sonata.Diagnostics.Logs.Layouts.SourcePatternLayout, Sonata.Diagnostics"),
							new XElement("conversionPattern", new XAttribute("value", "%utcdate{ISO8601} %10.10level %20.20identity %20.20username %50.50source - [%20.20code]: %message%newline")))),

					new XElement("appender", new XAttribute("name", "BusinessRollingFileAppender"), new XAttribute("type", "log4net.Appender.RollingFileAppender"),
						new XElement("file", new XAttribute("value", businessLogFileFullname)),
						new XElement("appendToFile", new XAttribute("value", "true")),
						new XElement("rollingStyle", new XAttribute("value", "Composite")),
						new XElement("maxSizeRollBackups", new XAttribute("value", "10")),
						new XElement("maximumFileSize", new XAttribute("value", "10MB")),
						new XElement("staticLogFileName", new XAttribute("value", "false")),
						new XElement("layout", new XAttribute("type", "Sonata.Diagnostics.Logs.Layouts.SourcePatternLayout, Sonata.Diagnostics"),
							new XElement("conversionPattern", new XAttribute("value", "%utcdate{ISO8601} %10.10level %20.20identity %20.20username %50.50source - [%20.20code]: %message%newline")))),

					new XElement("appender", new XAttribute("name", "TechnicalRollingFileAppender"), new XAttribute("type", "log4net.Appender.RollingFileAppender"),
						new XElement("file", new XAttribute("value", technicalLogFileFullname)),
						new XElement("appendToFile", new XAttribute("value", "true")),
						new XElement("rollingStyle", new XAttribute("value", "Composite")),
						new XElement("maxSizeRollBackups", new XAttribute("value", "10")),
						new XElement("maximumFileSize", new XAttribute("value", "10MB")),
						new XElement("staticLogFileName", new XAttribute("value", "false")),
						new XElement("layout", new XAttribute("type", "Sonata.Diagnostics.Diagnostics.Logs.Layouts.SourcePatternLayout, Sonata.Diagnostics"),
							new XElement("conversionPattern", new XAttribute("value", "%utcdate{ISO8601} %20.20level %20.20identity %20.20username %50.50source - [%20.20code]: %message %exception%newline")))),

					new XElement("appender", new XAttribute("name", "BusinessAdoNetAppender"), new XAttribute("type", "Sonata.Diagnostics.Logs.Appenders.AdoNetAppender"),
						new XElement("bufferSize", new XAttribute("value", "1")),
						new XElement("connectionType", new XAttribute("value", connectionType == null || connectionType.AssemblyQualifiedName == null ? String.Empty : connectionType.AssemblyQualifiedName)),
						new XElement("connectionStringName", new XAttribute("value", connectionStringName)),
						new XElement("commandText", new XAttribute("value", String.Format("INSERT INTO {0} ([LOG_date],[LOG_level],[LOG_identity],[LOG_username],[LOG_source],[LOG_code],[LOG_message]) VALUES (@log_date, @log_level, @identity, @username, @source, @code, @message)", logTableName))),
						new XElement("parameter",
							new XElement("parameterName", new XAttribute("value", "@log_date")),
							new XElement("dbType", new XAttribute("value", "DateTime")),
							new XElement("layout", new XAttribute("type", "log4net.Layout.RawUtcTimeStampLayout"))),

						new XElement("parameter",
							new XElement("parameterName", new XAttribute("value", "@log_level")),
							new XElement("dbType", new XAttribute("value", "String")),
							new XElement("size", new XAttribute("value", "20")),
							new XElement("layout", new XAttribute("type", "log4net.Layout.PatternLayout"),
								new XElement("conversionPattern", new XAttribute("value", "%level")))),

						new XElement("parameter",
							new XElement("parameterName", new XAttribute("value", "@identity")),
							new XElement("dbType", new XAttribute("value", "String")),
							new XElement("size", new XAttribute("value", "100")),
							new XElement("layout", new XAttribute("type", "log4net.Layout.PatternLayout"),
								new XElement("conversionPattern", new XAttribute("value", "%identity")))),

						new XElement("parameter",
							new XElement("parameterName", new XAttribute("value", "@username")),
							new XElement("dbType", new XAttribute("value", "String")),
							new XElement("size", new XAttribute("value", "100")),
							new XElement("layout", new XAttribute("type", "log4net.Layout.PatternLayout"),
								new XElement("conversionPattern", new XAttribute("value", "%username")))),

						new XElement("parameter",
							new XElement("parameterName", new XAttribute("value", "@source")),
							new XElement("dbType", new XAttribute("value", "String")),
							new XElement("size", new XAttribute("value", "200")),
							new XElement("layout", new XAttribute("type", "Sonata.Diagnostics.Logs.Layouts.SourcePatternLayout, Sonata.Diagnostics"),
								new XElement("conversionPattern", new XAttribute("value", "%source")))),

						new XElement("parameter",
							new XElement("parameterName", new XAttribute("value", "@code")),
							new XElement("dbType", new XAttribute("value", "String")),
							new XElement("size", new XAttribute("value", "200")),
							new XElement("layout", new XAttribute("type", "Sonata.Diagnostics.Logs.Layouts.SourcePatternLayout, Sonata.Diagnostics"),
								new XElement("conversionPattern", new XAttribute("value", "%code")))),

						new XElement("parameter",
							new XElement("parameterName", new XAttribute("value", "@message")),
							new XElement("dbType", new XAttribute("value", "String")),
							new XElement("size", new XAttribute("value", "4000")),
							new XElement("layout", new XAttribute("type", "log4net.Layout.PatternLayout"),
								new XElement("conversionPattern", new XAttribute("value", "%message"))))),

					new XElement("root",
						new XElement("level", new XAttribute("value", "ALL")),
						new XElement("appender-ref", new XAttribute("ref", "DebugAppender"))),

					new XElement("logger", new XAttribute("name", "Technical"),
						new XElement("level", new XAttribute("value", technicalMinimumLogLevel.GetStringValue())),
						new XElement("appender-ref", new XAttribute("ref", "TechnicalRollingFileAppender"))),

					new XElement("logger", new XAttribute("name", "Business"),
						new XElement("level", new XAttribute("value", businessMinimumLogLevel.GetStringValue())),
						new XElement("appender-ref", new XAttribute("ref", "BusinessAdoNetAppender")),
						new XElement("appender-ref", new XAttribute("ref", "BusinessRollingFileAppender"))));
		}

		#endregion
	}
}
