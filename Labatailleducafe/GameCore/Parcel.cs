using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public class Parcel
    {
        private byte m_owner;
        private List<Coordinate> m_coordinates = new List<Coordinate>();
 
        public byte Owner { get => m_owner; set => m_owner = value; }
        public List<Coordinate> Coordinates { get => m_coordinates; set => m_coordinates = value; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_owner"></param>
        public Parcel(byte p_owner)
        {
            this.Owner = p_owner;
        }
    }
}
