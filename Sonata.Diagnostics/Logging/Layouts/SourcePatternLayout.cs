#region Namespace Sonata.Diagnostics.Logging.Layouts
//	TODO
#endregion

using log4net.Layout;
using log4net.Util;
using Sonata.Diagnostics.Logging.Converters;

namespace Sonata.Diagnostics.Logging.Layouts
{
	public class SourcePatternLayout : PatternLayout
	{
		#region Methods

		#region PatternLayout Members

		public SourcePatternLayout()
		{
			AddConverter(new ConverterInfo { Name = Converters.PatternConverter.SourcePropertyName, Type = typeof(SourcePatternConverter) });
			AddConverter(new ConverterInfo { Name = Converters.PatternConverter.CodePropertyName, Type = typeof(CodePatternConverter) });
			AddConverter(new ConverterInfo { Name = Converters.PatternConverter.UserNamePropertyName, Type = typeof(UserNamePatternConverter) });
			AddConverter(new ConverterInfo { Name = Converters.PatternConverter.ThreadNamePropertyName, Type = typeof(ThreadNamePatternConverter) });
		}

		#endregion

		#endregion
	}
}