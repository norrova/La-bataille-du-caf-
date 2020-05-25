using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public class Unit
    {
        private byte m_owner;
        private char m_letterParcel;
        private bool m_available;

        public byte Owner { get => m_owner; set => m_owner = value; }
        public char LetterParcel { get => m_letterParcel; set => m_letterParcel = value; }
        public bool Available { get => m_available; set => m_available = value; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_owner">Owner of unit</param>
        /// <param name="p_letterParcel"></param>
        /// <param name="p_available">Allow to plant</param>
        public Unit(byte p_owner,  char p_letterParcel, bool p_available)
        {
            this.Owner = p_owner;
            this.LetterParcel = p_letterParcel;
            this.Available = p_available;
        }
    }
}
