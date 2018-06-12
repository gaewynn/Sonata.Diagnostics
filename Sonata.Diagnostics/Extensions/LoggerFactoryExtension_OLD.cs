#region Namespace Sonata.Diagnostics.Extensions
//	TODO
#endregion

using Microsoft.Extensions.Logging;
using Sonata.Diagnostics.Logs;
using System;

namespace Sonata.Diagnostics.Extensions
{
	public static class LoggerFactoryExtension_OLD
	{
		#region Constants

		/// <summary>
		/// The default log4net config file name.
		/// </summary>
		private const string DefaultLog4netConfigurationFileName = "log4net.config";

		#endregion

		#region Methods
		
		/// <summary>
		/// Registers the <see cref="LoggerProvider"/> by configuring it with a user defined configuration.
		/// </summary>
		/// <param name="instance">An instance of an <see cref="ILoggerFactory"/> on which configure the log engine.</param>
		/// <param name="exceptionFormatter">A function defining how an <see cref="Exception"/> has to be formatted in a <see cref="string"/> format.</param>
		/// <remarks>
		/// - The configuration must be in a XML format based on the log4net specifications (see https://logging.apache.org/log4net/)
		/// - Do not trace any logs in a database.
		/// </remarks>
		public static ILoggerFactory AddLog4Net(this ILoggerFactory instance,
			Func<object, Exception, string> exceptionFormatter = null)
		{
			return AddLog4Net(instance, DefaultLog4netConfigurationFileName, exceptionFormatter);
		}

		/// <summary>
		/// Registers the <see cref="LoggerProvider"/> by configuring it with a user defined configuration.
		/// </summary>
		/// <param name="instance">An instance of an <see cref="ILoggerFactory"/> on which configure the log engine.</param>
		/// <param name="log4netConfigurationFileFullName">The full name of the configuration file to use to configure the log engine.</param>
		/// <param name="exceptionFormatter">A function defining how an <see cref="Exception"/> has to be formatted in a <see cref="string"/> format.</param>
		/// <remarks>
		/// - The configuration must be in a XML format based on the log4net specifications (see https://logging.apache.org/log4net/)
		/// - Do not trace any logs in a database.
		/// </remarks>
		public static ILoggerFactory AddLog4Net(this ILoggerFactory instance, string log4netConfigurationFileFullName,
			Func<object, Exception, string> exceptionFormatter = null)
		{
			instance?.AddProvider(new Log4NetProvider(log4netConfigurationFileFullName, exceptionFormatter));
			return instance;
		}

		/// <summary>
		/// Registers the <see cref="LoggerProvider"/> by configuring it with the specified configuration.
		/// This configuration includes by default:
		///		- a <see cref="log4net.Appender.RollingFileAppender"/> for the technical logs (rolling on date and file size (10MB));
		///		- a <see cref="log4net.Appender.RollingFileAppender"/> for the business logs if the <paramref name="businessLogFileFullname"/> is specified (rolling on date and file size (10MB));
		/// </summary>
		/// <param name="instance">An instance of an <see cref="ILoggerFactory"/> on which configure the log engine.</param>
		/// <param name="technicalLogFileFullname">The full name of the file in which write the technical logs (including the file directory).</param>
		/// <param name="technicalMinimumLogLevel">The minimum level of technical logs to trace (Hierarchy is DEBUG - INFO - WARNING - ERROR - FATAL).</param>
		/// <param name="businessLogFileFullname">The full name of the file in which write the business logs (including the file directory).</param>
		/// <param name="businessMinimumLogLevel">The minimum level of business logs to trace (Hierarchy is DEBUG - INFO - WARNING - ERROR - FATAL).</param>
		/// <param name="exceptionFormatter">A function defining how an <see cref="Exception"/> has to be formatted in a <see cref="string"/> format.</param>
		/// <remarks>
		/// - All the files will be created by the Qim Library.
		/// - Do not trace any logs in a database.
		/// </remarks>
		/// <exception cref="InvalidOperationException">A call to the RegisterLoggerProvider method has already been done.</exception>
		/// <exception cref="ArgumentNullException">The given <see cref="technicalLogFileFullname"/> is null.</exception>
		public static ILoggerFactory AddLog4Net(this ILoggerFactory instance,
			string technicalLogFileFullname, LogLevel technicalMinimumLogLevel = LogLevel.Trace,
			string businessLogFileFullname = null, LogLevel businessMinimumLogLevel = LogLevel.Trace,
			Func<object, Exception, string> exceptionFormatter = null)
		{
			instance?.AddProvider(new Log4NetProvider(
				technicalLogFileFullname, technicalMinimumLogLevel,
				businessLogFileFullname, businessMinimumLogLevel,
				exceptionFormatter));

			return instance;
		}

		/// <summary>
		/// Registers the <see cref="LoggerProvider"/> by configuring it only for business logs traced in a database.
		/// </summary>
		/// <param name="instance">An instance of an <see cref="ILoggerFactory"/> on which configure the log engine.</param>
		/// <param name="connectionType">The type of the SQL connection used be the Qim Library in the logs module.</param>
		/// <param name="connectionStringName">The connection string to access the logs database.</param>
		/// <param name="logTableName">Th ename of the table for the logs in the database.</param>
		/// <param name="businessMinimumLogLevel">The minimum level of business logs to trace (Hierarchy is DEBUG - INFO - WARNING - ERROR - FATAL).</param>
		/// <param name="exceptionFormatter">A function defining how an <see cref="Exception"/> has to be formatted in a <see cref="string"/> format.</param>
		/// <remarks>The database log table will be handled and created by the Qim Library if needed.</remarks>
		/// <exception cref="InvalidOperationException">A call to the RegisterLoggerProvider method has already been done.</exception>
		/// <exception cref="ArgumentNullException">The given <see cref="connectionType"/> is null.</exception>
		/// <exception cref="ArgumentNullException">The given <see cref="connectionStringName"/> is null.</exception>
		/// <exception cref="ArgumentNullException">The given <see cref="logTableName"/> is null.</exception>
		public static ILoggerFactory AddLog4Net(this ILoggerFactory instance,
			Type connectionType, string connectionStringName, string logTableName,
			LogLevel businessMinimumLogLevel = LogLevel.Trace,
			Func<object, Exception, string> exceptionFormatter = null)
		{
			instance?.AddProvider(new Log4NetProvider(connectionType, connectionStringName, logTableName,
				businessMinimumLogLevel, 
				exceptionFormatter));

			return instance;
		}

		/// <summary>
		/// Registers the <see cref="LoggerProvider"/> by configuring it to use both technical and business logs. Business logs will be traced both in file and in a database.
		/// </summary>
		/// <param name="instance">An instance of an <see cref="ILoggerFactory"/> on which configure the log engine.</param>
		/// <param name="technicalLogFileFullname">The full name of the file in which write the technical logs (including the file directory).</param>
		/// <param name="connectionType">The type of the SQL connection used be the Qim Library in the logs module.</param>
		/// <param name="connectionStringName">The connection string to access the logs database.</param>
		/// <param name="logTableName">Th ename of the table for the logs in the database.</param>
		/// <param name="businessLogFileFullname">The full name of the file in which write the business logs (including the file directory).</param>
		/// <param name="businessMinimumLogLevel">The minimum level of business logs to trace (Hierarchy is DEBUG - INFO - WARNING - ERROR - FATAL).</param>
		/// <param name="technicalMinimumLogLevel">The minimum level of technical logs to trace (Hierarchy is DEBUG - INFO - WARNING - ERROR - FATAL).</param>
		/// <param name="exceptionFormatter">A function defining how an <see cref="Exception"/> has to be formatted in a <see cref="string"/> format.</param>
		/// <remarks>The database log table will be handled and created by the Qim Library if needed.</remarks>
		/// <exception cref="InvalidOperationException">A call to the RegisterLoggerProvider method has already been done.</exception>
		/// <exception cref="ArgumentNullException">The given <see cref="technicalLogFileFullname"/> is null.</exception>
		/// <exception cref="ArgumentNullException">The given <see cref="connectionType"/> is null.</exception>
		/// <exception cref="ArgumentNullException">The given <see cref="connectionStringName"/> is null.</exception>
		/// <exception cref="ArgumentNullException">The given <see cref="logTableName"/> is null.</exception>
		public static ILoggerFactory AddLog4Net(this ILoggerFactory instance,
			string technicalLogFileFullname, Type connectionType, string connectionStringName, string logTableName, string businessLogFileFullname = null,
			LogLevel businessMinimumLogLevel = LogLevel.Trace, LogLevel technicalMinimumLogLevel = LogLevel.Trace,
			Func<object, Exception, string> exceptionFormatter = null)
		{
			instance?.AddProvider(new Log4NetProvider(
				technicalLogFileFullname, connectionType, connectionStringName, logTableName, businessLogFileFullname,
				businessMinimumLogLevel, technicalMinimumLogLevel,
				exceptionFormatter));

			return instance;
		}

		#endregion
	}
}
