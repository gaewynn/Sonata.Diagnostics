#region Namespace Sonata.Diagnostics.Logging
//	TODO
#endregion

using log4net;
using log4net.Repository;
using System.Collections.Concurrent;
using System.Reflection;

namespace Sonata.Diagnostics.Logging
{
	internal class Log4NetLoggerProvider : Microsoft.Extensions.Logging.ILoggerProvider
	{
		#region Members

		/// <summary>
		/// The loggers collection.
		/// </summary>
		private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new ConcurrentDictionary<string, Log4NetLogger>();

		/// <summary>
		/// The log4net repository.
		/// </summary>
		private readonly ILoggerRepository _loggerRepository;

		#endregion

		#region Properties

		/// <summary>
		/// NLog options
		/// </summary>
		public Log4NetProviderOptions Options { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// New provider with default options, see <see cref="Options"/>
		/// </summary>
		public Log4NetLoggerProvider()
			: this(null)
		{ }

		/// <summary>
		/// New provider with options
		/// </summary>
		/// <param name="options"></param>
		public Log4NetLoggerProvider(Log4NetProviderOptions options)
		{
			_loggerRepository = LogManager.CreateRepository(options.RepositoryAssembly ?? Assembly.GetEntryAssembly(),
				typeof(log4net.Repository.Hierarchy.Hierarchy));

			Options = MergeOptions(options ?? Log4NetProviderOptions.Default);
		}

		#endregion

		#region Methods

		#region ILoggerProvider Members

		/// <summary>
		/// Create a logger with the name <paramref name="name"/>.
		/// </summary>
		/// <param name="name">Name of the logger to be created.</param>
		/// <returns>New Logger</returns>
		public Microsoft.Extensions.Logging.ILogger CreateLogger(string name)
		{
			if (_loggers.TryGetValue(name, out var existingLogger))
				return existingLogger;

			var logger = CreateLoggerImplementation(name);

			return _loggers.GetOrAdd(name, logger);
		}

		/// <summary>
		/// Cleanup
		/// </summary>
		public void Dispose()
		{
			_loggers.Clear();
		}

		#endregion

		/// <summary>
		/// Creates the logger implementation.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>The <see cref="Log4NetLogger"/> instance.</returns>
		private Log4NetLogger CreateLoggerImplementation(string name)
			=> new Log4NetLogger(_loggerRepository.Name, name, Options)
				.UsingExceptionFormatter(Options.ExceptionFormatter);

		private Log4NetProviderOptions MergeOptions(Log4NetProviderOptions options)
		{
			if (options == null)
				return Log4NetProviderOptions.Default;

			if (options.ExceptionFormatter == null)
				options.ExceptionFormatter = Log4NetProviderOptions.Default.ExceptionFormatter;

			if (options.PropertiesAccessor == null)
				options.PropertiesAccessor = Log4NetProviderOptions.Default.PropertiesAccessor;

			return options;
		}

		#endregion
	}
}
