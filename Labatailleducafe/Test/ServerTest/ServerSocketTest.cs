using System;
using Xunit;
using System.Net.Sockets;
using ServerCore;

namespace LabatailleducafeTest.ServerTest
{
    public class ServerSocketTest
    {
        private Server host;
        private Socket socket;

        [Fact]
        public void TestConnectionSocket()
        {
            host = new Server("51.91.120.237", 1212);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Assert.IsType<Socket>(host.Connection(socket));
            Assert.Equal(socket.Available, 0);
            Assert.False(socket.Poll(1, SelectMode.SelectRead));
        }

        [Fact]
        public void TestDeconnectionSocket()
        {
            host = new Server("51.91.120.237", 1212);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            host.Connection(socket);
            Assert.True(host.Deconnection(socket));
        }
    }
}
