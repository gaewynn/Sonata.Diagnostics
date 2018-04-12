#region Namespace Sonata.Diagnostics.Logging
//	TODO
#endregion

using System;
using System.Reflection;
using System.Xml.Linq;

namespace Sonata.Diagnostics.Logging
{
	public class Log4NetProviderOptions
	{
		#region Members

		internal static Log4NetProviderOptions Default;

		#endregion

		#region Properties

		public Assembly RepositoryAssembly { get; set; }

		public Func<object, ILog4NetProperties> PropertiesAccessor { get; set; }

		public Func<object, string> UserNameAccessor { get; set; }

		public Func<object, Exception, string> ExceptionFormatter { get; set; }

		/// <summary>
		/// Gets or sets an <see cref="Action"/> allowing to configure log4net manually.
		/// The action provides a <see cref="XElement"/> containing the provided log4net configuration.
		/// </summary>
		public Action<XElement> Configure { get; set; }

		/// <summary>
		/// Gets or sets the log4net configuration file fullname (must be in a XML format).
		/// </summary>
		/// <remarks>
		/// If both <see cref="ConfigurationFileFullName"/> and <see cref="JsonConfiguration"/> are set, the <see cref="JsonConfiguration"/> will be used.
		/// </remarks>
		public string ConfigurationFileFullName { get; set; }

		/// <summary>
		/// Gets or sets the log4net configuration in a JSON format.
		/// </summary>
		/// <remarks>
		/// If both <see cref="ConfigurationFileFullName"/> and <see cref="JsonConfiguration"/> are set, the <see cref="JsonConfiguration"/> will be used.
		/// </remarks>
		public string JsonConfiguration { get; set; }

		public Schema.log4net Log4NetConfiguration { get; internal set; }

		public bool IncludeScopes { get; set; }

		#endregion

		#region Constructors

		static Log4NetProviderOptions()
		{
			Default = new Log4NetProviderOptions
			{
				RepositoryAssembly = null,
				IncludeScopes = false,
				ConfigurationFileFullName = "log4net.config",
				JsonConfiguration = null,
				PropertiesAccessor = state => new Log4NetProperties
				{
					Code = null,
					Source = null,
					Message = null
				},
				UserNameAccessor = state => null,
				Configure = null,
				ExceptionFormatter = (state, exception) => $"{state.ToString()} - {exception.Message}",
			};
		}

		#endregion
	}
}
