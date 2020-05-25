using System;
using System.Text;
using System.Net.Sockets;

namespace ServerCore
{
    /// <summary>
    /// The main Server class.
    /// Contains all methods for connection to a server 
    /// </summary>
    public class Server
    {
        // IP server
        private string _ip;
        // Port server
        private int _port;

        private ushort m_sizeMap = 259;
        private byte m_sizeResponse = 4;

        // Getter IP
        public string GetIp => _ip;
        // Getter Port
        public int GetPort => _port;
        public byte SizeResponse { get => m_sizeResponse;}
        public ushort SizeMap { get => m_sizeMap;}


        /// <summary>
        /// The constructor Server
        /// </summary>
        /// <param name="ip">Take string</param>
        /// <param name="port">Take int</param>
        public Server(string ip, int port)
        {
            this._ip = ip;
            this._port = port;
        }

        /// <summary>
        /// Test avaibility of server
        /// </summary>
        /// <returns></returns>
        public bool Available()
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    string v_ip = this.GetIp == "localhost" ? "127.0.0.1" : this.GetIp;
                    tcpClient.Connect(v_ip, this.GetPort);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{DateTime.Now} : Server Online");
                    Console.ResetColor();
                    return true;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{DateTime.Now} : Serveur Offline");
                    Console.ResetColor();
                    return false;
                }
            }
        }

        ///<summary>
        /// Connection to the server
        /// </summary>
        /// <param name="socket">Take Socket</param>
        public Socket Connection(Socket socket)
        {
            try
            {
                socket.Connect(this._ip, this._port);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{DateTime.Now} : Server Connected");
                Console.ResetColor();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException : {0}", e.ToString());
                System.Environment.Exit(1);
                throw;
            }
            return socket;
        }

        /// <summary>
        /// This method send data to the server
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="p_response"></param>
        public void SendData(Socket socket, String p_response)
        {
            try
            {
                byte[] v_response = Encoding.UTF8.GetBytes(p_response);
                socket.Send(v_response);
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
                System.Environment.Exit(2);
                throw;
            }
        }

        ///<summary>
        /// This method receive data from server
        /// </summary>
        /// <param name="socket">Take Socket</param>
        /// <param name="WeftSize">Take int</param>
        /// <returns>Return buffer encoding in UTF8</returns>
        public string ReceiveData(Socket socket,int WeftSize)
        {
            try
            {
                byte[] buffer = new byte[WeftSize];
                socket.Receive(buffer);
                return Encoding.UTF8.GetString(buffer);
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
                System.Environment.Exit(2);
                throw;
            }
        }

        /// <summary>
        /// This method disconnect the connection from server
        /// </summary>
        /// <param name="socket">Take Socket</param>
        public Boolean Deconnection(Socket socket)
        {
            try
            {
                // Disable send and receives on this socket
                socket.Shutdown(SocketShutdown.Both);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n{DateTime.Now} : Server Disconnected");
                Console.ResetColor();
                // Close this socket and releases all associated ressources
                socket.Close();
                return true;
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
                System.Environment.Exit(3);
                throw;
            }
        }

        /// <summary>
        /// This method return variables of class
        /// </summary>
        /// <returns>GetIP and GetPort</returns>
        public override string ToString()
        {
            return "Ip : " + GetIp + ", Port : " + GetPort;
        }
    }
}
