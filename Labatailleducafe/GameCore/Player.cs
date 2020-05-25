using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GameCore
{
    public class Player
    {
        private String m_name;
        private byte m_numberOwner;
        private Score m_score = new Score(0, 0);
     
        public string Name { get => m_name; set => m_name = value; }
        public byte NumberOwner { get => m_numberOwner;}
        public Score Score { get => m_score; set => m_score = value; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_name"></param>
        /// <param name="p_numberOwner"></param>
        public Player(string p_name, byte p_numberOwner)
        {
            this.Name = p_name;
            this.m_numberOwner = p_numberOwner;
        }

        /// <summary>
        /// Show scores
        /// </summary>
        public void ShowScore()
        {
            Console.Write($"\nPlayer : ");
            Console.ForegroundColor = this.NumberOwner == (byte)Seed.Me ? ConsoleColor.Green : this.NumberOwner == (byte)Seed.Server ? ConsoleColor.Red : ConsoleColor.Gray;
            Console.WriteLine(this.Name);
            Console.ResetColor();
            Console.WriteLine($"Points de parcelles : {this.Score.ParcelPoint}");
            Console.WriteLine($"Points de graines adjacentes : {this.Score.SeedAdjacent}");
            Console.WriteLine($"Total : {this.Score.ParcelPoint + this.Score.SeedAdjacent}");
        }
    }
}
