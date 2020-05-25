using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace GameCore
{
    public class Map
    {
        private Char[,] m_originalMap;
        private Dictionary<string, Parcel> m_parcel = new Dictionary<string, Parcel>();
        private Unit[,] m_actualMap = new Unit[10, 10];

        public Unit[,] ActualMap { get => m_actualMap; set => m_actualMap = value; }
        public Dictionary<string, Parcel> Parcel { get => m_parcel; set => m_parcel = value; }

        public Map(ref Char[,] mapArray) => this.m_originalMap = mapArray;

        /// <summary>
        /// Create the new Map with properties
        /// </summary>
        public void Create()
        {
            for (byte s_row = 0; s_row < 10; s_row++)
            {
                for (byte s_col = 0; s_col < 10; s_col++)
                {
                    string v_letter = this.m_originalMap[s_row, s_col].ToString();
                    if (!this.Parcel.ContainsKey(v_letter) && !SeaOrForest(v_letter))
                    {
                        Parcel v_tmpParcel = new Parcel(0);
                        v_tmpParcel.Coordinates.Add(new Coordinate(v_letter.ToUpper(), s_row, s_col));
                        m_parcel.Add(v_letter, v_tmpParcel);
                    }
                    else if (this.Parcel.ContainsKey(v_letter))
                    {
                        this.Parcel[v_letter].Coordinates.Add(new Coordinate(v_letter.ToUpper(), s_row, s_col));
                    }
                    if(!SeaOrForest(v_letter))
                        this.ActualMap[s_row, s_col] = new Unit(0, this.m_originalMap[s_row, s_col], true);
                    else
                        this.ActualMap[s_row, s_col] = new Unit(0, this.m_originalMap[s_row, s_col], false);
                }
            }
        }

        /// <summary>
        /// Check if letter is Sea or Forest
        /// </summary>
        /// <param name="p_letter"></param>
        /// <returns></returns>
        public bool SeaOrForest(string p_letter)
        {
            return p_letter.Equals("M") || p_letter.Equals("F") ? true : false; 
        }

        /// <summary>
        /// Get letter of parcel
        /// </summary>
        /// <param name="p_x"></param>
        /// <param name="p_y"></param>
        /// <returns></returns>
        public String GetLetterParcel(byte p_x, byte p_y)
        {
            return this.ActualMap[p_x, p_y].LetterParcel.ToString().ToUpper();
        }

        /// <summary>
        /// Show the map
        /// </summary>
        public void Show() 
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n{DateTime.Now} : Final map");
            Console.ResetColor();
            for (int s_row = 0; s_row < 10; s_row++)
            {
                for (int s_col = 0; s_col < 10; s_col++)
                {
                    byte v_owner = this.ActualMap[s_row, s_col].Owner;
                    Console.ForegroundColor = v_owner == (byte)Seed.Me ? ConsoleColor.Green : v_owner == (byte)Seed.Server ? ConsoleColor.Red : ConsoleColor.Gray;
                    Console.Write($"{this.ActualMap[s_row, s_col].LetterParcel}:{v_owner} ");
                }
                Console.WriteLine("");
            }
        }
    }
}
