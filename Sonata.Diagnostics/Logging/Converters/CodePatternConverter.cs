#region Namespace Sonata.Diagnostics.Logging.Converters
//	TODO
#endregion

using log4net;
using log4net.Core;
using log4net.Layout.Pattern;
using System.IO;

namespace Sonata.Diagnostics.Logging.Converters
{
	public class CodePatternConverter : PatternLayoutConverter
	{
		#region Constants

		internal const string CodePropertyName = "code";

		#endregion

		#region Methods

		#region PatternLayoutConverter Members

		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.Write(LogicalThreadContext.Properties[CodePropertyName]);
		}

		#endregion

		#endregion
	}
}