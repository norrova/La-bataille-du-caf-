using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public static class Math
    {
        /// <summary>
        /// Calculate a percentage
        /// </summary>
        /// <param name="p_top"></param>
        /// <param name="p_bottom"></param>
        /// <returns></returns>
        public static float Percent(float p_top, float p_bottom)
        {
            try
            {
                return (p_top / p_bottom) * 100;
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }
    }
}
