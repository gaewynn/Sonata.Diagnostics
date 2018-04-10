using Sonata.Diagnostics.Logging;
using System;
using Xunit;

namespace Sonata.Diagnostics.Tests.Logging
{
	public class LogTests
    {
        [Fact]
        public void ToStringReturnsCodeAndMessage()
        {
	        var log = Log.BuildDebug("CODE", "MESSAGE");
			Assert.Equal("CODE - MESSAGE", log.ToString());
        }

	    [Fact]
	    public void ToStringReturnsCodeOnlyIfMessageNullOrEmptyOrWhitespace()
	    {
		    var log = Log.BuildDebug("CODE", null);
		    Assert.Equal("CODE", log.ToString());

		    log = Log.BuildDebug("CODE", String.Empty);
		    Assert.Equal("CODE", log.ToString());

		    log = Log.BuildDebug("CODE", "  ");
		    Assert.Equal("CODE", log.ToString());
		}

	    [Fact]
	    public void ToStringReturnsMessageOnlyIfCodeNullOrEmptyOrWhitespace()
	    {
		    var log = Log.BuildDebug(null, "MESSAGE");
		    Assert.Equal("MESSAGE", log.ToString());

			log = Log.BuildDebug(String.Empty, "MESSAGE");
			Assert.Equal("MESSAGE", log.ToString());

		    log = Log.BuildDebug("    ", "MESSAGE");
			Assert.Equal("MESSAGE", log.ToString());
		}

	    [Fact]
	    public void ToStringReturnsEmptyIfCodeAndMessageNullsOrEmptiesOrWhitespaces()
	    {
		    var log = Log.BuildDebug(null, "");
		    Assert.Equal(String.Empty, log.ToString());

		    log = Log.BuildDebug(String.Empty, null);
		    Assert.Equal(String.Empty, log.ToString());

		    log = Log.BuildDebug("    ", "  ");
		    Assert.Equal(String.Empty, log.ToString());
		}
	}
}
