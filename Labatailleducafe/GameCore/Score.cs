using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace GameCore
{
    public class Score
    {
        private byte m_parcelPoint;
        private byte m_seedAdjacent;
        private IA m_ia;

        public byte ParcelPoint { get => m_parcelPoint; set => m_parcelPoint = value; }
        public byte SeedAdjacent { get => m_seedAdjacent; set => m_seedAdjacent = value; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Score() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public Score(ref IA p_ia)
        {
            this.m_ia = p_ia;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Score(byte p_parcelPoint, byte p_seedAdjacent)
        {
            this.m_parcelPoint = p_parcelPoint;
            this.m_seedAdjacent = p_seedAdjacent;
        }

        /// <summary>
        /// Calculate parcel score of all players
        /// </summary>
        /// <param name="p_p1"></param>
        /// <param name="p_p2"></param>
        public void ParcelScorePlayer(ref Player p_p1, ref Player p_p2)
        {
            foreach (KeyValuePair<string, Parcel> s_parcel in this.m_ia.Map.Parcel)
            {
                if (s_parcel.Value.Owner == (byte)Seed.Me)
                    p_p1.Score.m_parcelPoint += (byte)s_parcel.Value.Coordinates.Count;
                if (s_parcel.Value.Owner == (byte)Seed.Server)
                    p_p2.Score.m_parcelPoint += (byte)s_parcel.Value.Coordinates.Count;
            }
        }

        /// <summary>
        /// Calculate Adjacent score player
        /// </summary>
        /// <param name="p_p1"></param>
        /// <param name="p_p2"></param>
        public void AdjacentScorePlayer(ref Player p_p1, ref Player p_p2)
        {
            foreach (byte s_owner in new Byte[] { (byte)Seed.Me, (byte)Seed.Server })
            {
                for (int s_row = 0; s_row < 10; s_row++)
                {
                    for (int s_col = 0; s_col < 10; s_col++)
                    {
                        if (this.m_ia.Map.ActualMap[s_row, s_col].Owner == s_owner){

                            byte v_result = DepthFirstSearch(s_owner, new Coordinate((byte)s_row, (byte)s_col));
                            if (s_owner == (byte)Seed.Me && v_result > p_p1.Score.SeedAdjacent)
                                p_p1.Score.SeedAdjacent = v_result;
                            if (s_owner == (byte)Seed.Server && v_result > p_p2.Score.SeedAdjacent)
                                p_p2.Score.SeedAdjacent = v_result;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get score by algorithm DepthFirstSearch (not recursive)
        /// </summary>
        /// <param name="p_owner"></param>
        /// <param name="p_coord"></param>
        /// <returns></returns>
        private byte DepthFirstSearch(byte p_owner, Coordinate p_coord)
        {
            List<Coordinate> v_coordChecked = new List<Coordinate>();
            List<Coordinate> v_coordNotChecked = new List<Coordinate>(){
                p_coord
            };
            do
            {
                List<Coordinate> v_tmpList = new List<Coordinate>(v_coordNotChecked);
                foreach (Coordinate s_coord in v_tmpList)
                {
                    int v_y = (int)s_coord.Y;
                    int v_x = (int)s_coord.X;
                    List<Coordinate> v_locations = new List<Coordinate>()
                    {
                        new Coordinate((byte)(v_x - 1), (byte)v_y), // North
                        new Coordinate((byte)v_x, (byte)(v_y +1)),   // East                    
                        new Coordinate((byte)(v_x + 1), (byte)v_y), // South
                        new Coordinate((byte)v_x, (byte)(v_y -1)),   // West
                    };
                    foreach (Coordinate s_location in v_locations)
                    {
                        try
                        {
                            if (this.m_ia.Map.ActualMap[s_location.X, s_location.Y].Owner.Equals(p_owner)
                                && !Exists(v_coordChecked, s_location) && !Exists(v_coordNotChecked, s_location))
                            {
                                v_coordNotChecked.Add(s_location);
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            continue;
                        }
                    }
                    v_coordNotChecked.Remove(s_coord);
                    v_coordChecked.Add(s_coord);
                }
            } while (v_coordNotChecked.Count > 0);
            return (byte)v_coordChecked.Count;
        }

        /// <summary>
        /// Check if coordinate exists in List
        /// </summary>
        /// <param name="p_list"></param>
        /// <param name="p_coord"></param>
        /// <returns></returns>
        private bool Exists(List<Coordinate> p_list, Coordinate p_coord)
        {
            bool v_bool = false;
            foreach(Coordinate s_tmp in p_list)
            {
                if (s_tmp.X == p_coord.X && s_tmp.Y == p_coord.Y)
                {
                    v_bool = true;
                    break;
                }
            }
            return v_bool;
        }
    }
}
