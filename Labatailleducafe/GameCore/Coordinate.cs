using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public class Coordinate
    {
        #region Attribute
        private String m_zone;
        private byte m_x;
        private byte m_y;
        #endregion

        #region Get Set
        public String Zone
        {
            get => m_zone;
            set
            {
                value = value.Trim().ToUpper();
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException();
                }
                m_zone = value;
            }
        }
        public byte X { get => m_x; set => m_x = value; }
        public byte Y { get => m_y; set => m_y = value; }
        #endregion

        #region Constructor Method
        /// <summary>
        /// Constructor to initialize Point with coordinate (zone,x,y)
        /// </summary>
        /// <param name="p_zone"></param>
        /// <param name="p_x"></param>
        /// <param name="p_y"></param>
        ///

        public Coordinate()
        {
        }

        public Coordinate(byte p_x, byte p_y)
        {
            this.X = p_x;
            this.Y = p_y;
        }

        public Coordinate(String p_zone, byte p_x, byte p_y)
        {
            this.Zone = p_zone;
            this.X = p_x;
            this.Y = p_y;
        }

        /// <summary>
        /// Get coordinate
        /// </summary>
        /// <returns></returns>
        public String GetCoordinates()
        {
            return $"{Zone}:{X}{Y}";
        }
        #endregion

    }
}
