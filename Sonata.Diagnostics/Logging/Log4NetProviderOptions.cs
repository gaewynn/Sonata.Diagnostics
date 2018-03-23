#region Namespace Sonata.Diagnostics.Logging
//	TODO
#endregion

using System;

namespace Sonata.Diagnostics.Logging
{
	public class Log4NetProviderOptions
	{
		#region Members

		internal static Log4NetProviderOptions Default;

		#endregion

		#region Properties

		public Func<object, ILog4NetProperties> PropertiesAccessor { get; set; }

		public Func<object, Exception, string> ExceptionFormatter { get; set; }

		public string ConfigurationFileFullName { get; set; }

		public Schema.log4net Log4NetConfiguration { get; internal set; }

		public bool IncludeScopes { get; set; }

		#endregion

		#region Constructors

		static Log4NetProviderOptions()
		{
			Default = new Log4NetProviderOptions
			{
				IncludeScopes = false,
				ConfigurationFileFullName = "log4net.config",
				PropertiesAccessor = state => new Log4NetProperties
				{
					Code = null,
					Source = null,
					Message = null
				},
				ExceptionFormatter = (state, exception) => $"{state.ToString()} - {exception.Message}",
			};
		}

		#endregion
	}
}
