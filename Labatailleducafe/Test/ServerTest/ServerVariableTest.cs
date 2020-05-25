using System;
using Xunit;
using ServerCore;

namespace LabatailleducafeTest
{
    public class ServerVariableTest
    {
        private Server host;

        [Fact]
        public void TestVariableIp()
        {
            host = new Server("51.91.120.237", 1212);

            Assert.IsType<String>(host.GetIp);
            Assert.NotEmpty(host.GetIp);
            Assert.Equal(host.GetIp, "51.91.120.237");
        }

        [Fact]
        public void TestVariablePort()
        {
            host = new Server("51.91.120.237", 1212);

            Assert.IsType<int>(host.GetPort);
            Assert.Equal(host.GetPort, 1212);
        }
    }
}
