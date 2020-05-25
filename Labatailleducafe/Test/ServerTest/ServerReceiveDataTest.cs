using System;
using Xunit;
using System.Net.Sockets;
using ServerCore;

namespace LabatailleducafeTest.ServerTest
{
    public class ServerReceiveDataTest
    {
        private Server host;
        private Socket socket;

        [Fact]
        public void TestReceiveData()
        {
            host = new Server("51.91.120.237", 1212);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            host.Connection(socket);
            Assert.IsType<String>(host.ReceiveData(socket, 259));
            Assert.NotEmpty(host.ReceiveData(socket, 259));
            host.Deconnection(socket);
        }
    }
}
