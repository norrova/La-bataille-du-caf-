using System;
using System.Net.Sockets;
using ServerCore;
using GameCore;
using System.Net;
using System.Threading;
using System.Drawing;

namespace Labatailleducafe
{
    class Program
    {

        static void Main(string[] args)
        {
            Server v_host = new Server("localhost", 1213);
            do
            {
                // Test if server is available
                if (v_host.Available()) ThreadPool.QueueUserWorkItem(Play, v_host);
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }

        /// <summary>
        /// Method to play the game
        /// </summary>
        /// <param name="p_host"></param>
        private static void Play(object p_host)
        {
            Server v_host = p_host as Server;
            Socket v_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            int[,] v_integerArray = new int[10, 10];
            Char[,] v_mapArray = new Char[10, 10];

            v_host.Connection(v_socket);

            Decoder.WeftSplit(v_host.ReceiveData(v_socket, 600), v_integerArray);
            Decoder.Decode(v_integerArray, ref v_mapArray);
            Decoder.Display(ref v_mapArray);

            Map v_map = new Map(ref v_mapArray);
            v_map.Create();

            IA v_ia = new IA(ref v_map);

            Player v_p1 = new Player("CLIENT", (byte)Seed.Me);
            Player v_p2 = new Player("SERVER", (byte)Seed.Server);

            Game v_game = new Game(v_host, v_socket, v_ia, ref v_p1, ref v_p2);
            v_game.Play();

            v_map.Show();
            
            v_p1.ShowScore();
            v_p2.ShowScore();

            v_game.ShowWinner();
            v_host.Deconnection(v_socket);
        }
    }
}
