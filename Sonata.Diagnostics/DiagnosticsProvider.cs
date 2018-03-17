#region Namespace Sonata.Diagnostics
//	TODO: comment
#endregion

using System;
using System.Collections.Generic;
using Sonata.Diagnostics.Logs;

namespace Sonata.Diagnostics
{
	public class DiagnosticsProvider
	{
		#region Constants

		/// <summary>
		/// A key allowing to know if the <see cref="LoggerProvider"/> has been registered.
		/// The <see cref="LoggerProvider"/> can only be registered once during the application startup.
		/// </summary>
		private const string LoggerProviderKey = "RegisterLoggerProvider";

		#endregion

		#region Members

		/// <summary>
		/// A list of providers already registered. This list tracks the providers registered to ensure they are only registered once.
		/// </summary>
		private static readonly List<string> ProvidersSet = new List<string>();

		#endregion

		#region Methods

		/// <summary>
		/// Registers the <see cref="LoggerProvider"/> by configuring it with a user defined configuration.
		/// </summary>
		/// <remarks>
		/// - The configuration must be in a XML format based on the log4net specifications (see https://logging.apache.org/log4net/)
		/// - Do not trace any logs in a database.
		/// </remarks>
		public static void RegisterLoggerProvider()
		{
			if (ProvidersSet.Contains(LoggerProviderKey))
				throw new InvalidOperationException("Logger provider already configured.");

			LoggerProvider.Instance.Setup(null, false, null, null, false, null, null, null);
			RegisterLoggerProviderEnd();
		}

		public static void RegisterLoggerProvider(string applicationConfigurationFileFullName)
		{
			if (ProvidersSet.Contains(LoggerProviderKey))
				throw new InvalidOperationException("Logger provider already configured.");

			LoggerProvider.Instance.Setup(applicationConfigurationFileFullName, false, null, null, false, null, null, null);
			RegisterLoggerProviderEnd();
		}

		/// <summary>
		/// Registers the <see cref="LoggerProvider"/> by configuring it with the specified configuration.
		/// This configuration includes by default:
		///		- a <see cref="log4net.Appender.RollingFileAppender"/> for the technical logs (rolling on date and file size (10MB));
		///		- a <see cref="log4net.Appender.RollingFileAppender"/> for the business logs if the <paramref name="businessLogFileFullname"/> is specified (rolling on date and file size (10MB));
		/// </summary>
		/// <param name="technicalLogFileFullname">The full name of the file in which write the technical logs (including the file directory).</param>
		/// <param name="technicalMinimumLogLevel">The minimum level of technical logs to trace (Hierarchy is DEBUG - INFO - WARNING - ERROR - FATAL).</param>
		/// <param name="businessLogFileFullname">The full name of the file in which write the business logs (including the file directory).</param>
		/// <param name="businessMinimumLogLevel">The minimum level of business logs to trace (Hierarchy is DEBUG - INFO - WARNING - ERROR - FATAL).</param>
		/// <remarks>
		/// - All the files will be created by the Qim Library.
		/// - Do not trace any logs in a database.
		/// </remarks>
		/// <exception cref="InvalidOperationException">A call to the RegisterLoggerProvider method has already been done.</exception>
		/// <exception cref="ArgumentNullException">The given <see cref="technicalLogFileFullname"/> is null.</exception>
		public static void RegisterLoggerProvider(
			string technicalLogFileFullname, LogLevels technicalMinimumLogLevel = LogLevels.Debug,
			string businessLogFileFullname = null, LogLevels businessMinimumLogLevel = LogLevels.Debug)
		{
			if (ProvidersSet.Contains(LoggerProviderKey))
				throw new InvalidOperationException("Logger provider already configured.");
			if (String.IsNullOrWhiteSpace(technicalLogFileFullname))
				throw new ArgumentNullException("technicalLogFileFullname");

			LoggerProvider.Instance.Setup(null, true, businessLogFileFullname, technicalLogFileFullname, false, null, null, null, businessMinimumLogLevel, technicalMinimumLogLevel);
			RegisterLoggerProviderEnd();
		}

		/// <summary>
		/// Registers the <see cref="LoggerProvider"/> by configuring it only for business logs traced in a database.
		/// </summary>
		/// <param name="connectionType">The type of the SQL connection used be the Qim Library in the logs module.</param>
		/// <param name="connectionStringName">The connection string to access the logs database.</param>
		/// <param name="logTableName">Th ename of the table for the logs in the database.</param>
		/// <param name="businessMinimumLogLevel">The minimum level of business logs to trace (Hierarchy is DEBUG - INFO - WARNING - ERROR - FATAL).</param>
		/// <remarks>The database log table will be handled and created by the Qim Library if needed.</remarks>
		/// <exception cref="InvalidOperationException">A call to the RegisterLoggerProvider method has already been done.</exception>
		/// <exception cref="ArgumentNullException">The given <see cref="connectionType"/> is null.</exception>
		/// <exception cref="ArgumentNullException">The given <see cref="connectionStringName"/> is null.</exception>
		/// <exception cref="ArgumentNullException">The given <see cref="logTableName"/> is null.</exception>
		public static void RegisterLoggerProvider(Type connectionType, string connectionStringName, string logTableName, LogLevels businessMinimumLogLevel = LogLevels.Debug)
		{
			if (ProvidersSet.Contains(LoggerProviderKey))
				throw new InvalidOperationException("Logger provider already configured.");
			if (connectionType == null)
				throw new ArgumentNullException("connectionType");
			if (String.IsNullOrWhiteSpace(connectionStringName))
				throw new ArgumentNullException("connectionStringName");
			if (String.IsNullOrWhiteSpace(logTableName))
				throw new ArgumentNullException("logTableName");

			LoggerProvider.Instance.Setup(null, false, null, null, true, connectionType, connectionStringName, logTableName, businessMinimumLogLevel);
			RegisterLoggerProviderEnd();
		}

		/// <summary>
		/// Registers the <see cref="LoggerProvider"/> by configuring it to use both technical and business logs. Business logs will be traced both in file and in a database.
		/// </summary>
		/// <param name="technicalLogFileFullname">The full name of the file in which write the technical logs (including the file directory).</param>
		/// <param name="connectionType">The type of the SQL connection used be the Qim Library in the logs module.</param>
		/// <param name="connectionStringName">The connection string to access the logs database.</param>
		/// <param name="logTableName">Th ename of the table for the logs in the database.</param>
		/// <param name="businessLogFileFullname">The full name of the file in which write the business logs (including the file directory).</param>
		/// <param name="businessMinimumLogLevel">The minimum level of business logs to trace (Hierarchy is DEBUG - INFO - WARNING - ERROR - FATAL).</param>
		/// <param name="technicalMinimumLogLevel">The minimum level of technical logs to trace (Hierarchy is DEBUG - INFO - WARNING - ERROR - FATAL).</param>
		/// <remarks>The database log table will be handled and created by the Qim Library if needed.</remarks>
		/// <exception cref="InvalidOperationException">A call to the RegisterLoggerProvider method has already been done.</exception>
		/// <exception cref="ArgumentNullException">The given <see cref="technicalLogFileFullname"/> is null.</exception>
		/// <exception cref="ArgumentNullException">The given <see cref="connectionType"/> is null.</exception>
		/// <exception cref="ArgumentNullException">The given <see cref="connectionStringName"/> is null.</exception>
		/// <exception cref="ArgumentNullException">The given <see cref="logTableName"/> is null.</exception>
		public static void RegisterLoggerProvider(
			string technicalLogFileFullname, Type connectionType, string connectionStringName, string logTableName, string businessLogFileFullname = null,
			LogLevels businessMinimumLogLevel = LogLevels.Debug, LogLevels technicalMinimumLogLevel = LogLevels.Debug)
		{
			if (ProvidersSet.Contains(LoggerProviderKey))
				throw new InvalidOperationException("Logger provider already configured.");
			if (String.IsNullOrWhiteSpace(technicalLogFileFullname))
				throw new ArgumentNullException("technicalLogFileFullname");
			if (connectionType == null)
				throw new ArgumentNullException("connectionType");
			if (String.IsNullOrWhiteSpace(connectionStringName))
				throw new ArgumentNullException("connectionStringName");
			if (String.IsNullOrWhiteSpace(logTableName))
				throw new ArgumentNullException("logTableName");

			LoggerProvider.Instance.Setup(null, true, businessLogFileFullname, technicalLogFileFullname, true, connectionType, connectionStringName, logTableName, businessMinimumLogLevel, technicalMinimumLogLevel);
			RegisterLoggerProviderEnd();
		}

		/// <summary>
		/// Sets the <see cref="LoggerProvider"/> in the Qim Library and marks it as registered.
		/// </summary>
		private static void RegisterLoggerProviderEnd()
		{
			LoggerConfiguration.LoggerProvider = LoggerProvider.Instance;
			ProvidersSet.Add(LoggerProviderKey);
		}

		#endregion
	}
}
