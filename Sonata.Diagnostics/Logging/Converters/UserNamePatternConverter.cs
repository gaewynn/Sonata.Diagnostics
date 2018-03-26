#region Namespace Sonata.Diagnostics.Logging.Converters
//	TODO
#endregion

using log4net;
using log4net.Core;
using log4net.Layout.Pattern;
using System.IO;

namespace Sonata.Diagnostics.Logging.Converters
{
	public class UserNamePatternConverter : PatternLayoutConverter
	{
		#region Methods

		#region PatternLayoutConverter Members

		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.Write(LogicalThreadContext.Properties[PatternConverter.UserNamePropertyName]);
		}

		#endregion

		#endregion
	}
}