#region Namespace Sonata.Diagnostics.Logs.Converters
//	TODO: comment
#endregion

using System.IO;
using log4net;
using log4net.Core;
using log4net.Layout.Pattern;

namespace Sonata.Diagnostics.Logs.Converters
{
	public class SourcePatternConverter : PatternLayoutConverter
	{
		#region Constants

		internal const string SourcePropertyName = "source";

		#endregion

		#region Methods

		#region PatternLayoutConverter Members

		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.Write(LogicalThreadContext.Properties[SourcePropertyName]);
		}

		#endregion

		#endregion
	}
}