#region Namespace Sonata.Diagnostics.Logs.Layouts
//	TODO: comment
#endregion

using log4net.Layout;
using log4net.Util;
using Sonata.Diagnostics.Logs.Converters;

namespace Sonata.Diagnostics.Logs.Layouts
{
	public class SourcePatternLayout : PatternLayout
	{
		#region Methods

		#region PatternLayout Members

		public SourcePatternLayout()
		{
			AddConverter(new ConverterInfo { Name = SourcePatternConverter.SourcePropertyName, Type = typeof(SourcePatternConverter) });
			AddConverter(new ConverterInfo { Name = CodePatternConverter.CodePropertyName, Type = typeof(CodePatternConverter) });
		}

		#endregion

		#endregion
	}
}