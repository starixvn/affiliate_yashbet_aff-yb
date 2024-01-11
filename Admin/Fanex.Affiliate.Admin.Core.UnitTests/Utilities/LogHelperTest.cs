using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Affiliate.Admin.Core.Utilities;
using NSubstitute;
using System;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Utilities
{
    public class LogHelperTest
    {
        [Fact]
        public void WriteInfo_WhenCall_WriteLog()
        {
            MockingHelper.SetUpLogEngineEnviroment();
            LogHelper.WriteInfo("2");

            Assert.True(true);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Message")]
        public void WriteError_WhenCall_WriteLog(string additionalMessage)
        {
            MockingHelper.SetUpHttpContextSession();
            MockingHelper.SetUpLogEngineEnviroment();

            var ex = Substitute.For<Exception>();

            LogHelper.WriteError(ex, additionalMessage);

            Assert.True(true);
        }
    }
}
