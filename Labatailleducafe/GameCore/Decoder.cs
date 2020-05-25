using System;

namespace GameCore
{
    /// <summary>
    /// The main Decoding class.
    /// Contains all methods for decoding weft 
    /// </summary>
    public static class Decoder
    {
        // Constant sea value
        private const int _seaValue = 64;
        // Constant forest value
        private const int _forestValue = 32;

        /// <summary>
        /// This method split data with delimiter "|" and ",".
        /// </summary>
        /// <param name="weft">Take string</param>
        /// <param name="integerArray">Take array of int with two dimensions 10/10</param>
        public static void WeftSplit(string weft,int[,] integerArray)
        {
            string[] dataSplit = weft.Split(new char[] {'|', ':'},StringSplitOptions.RemoveEmptyEntries);
            int col = 0, row = 0;
            foreach (string item in dataSplit)
            {
                if(row != 10)
                {
                    // Insert parse data in integerArray
                    integerArray[row,col] = int.Parse(item);
                    if(col == 9){
                        col=0;
                        row++;
                    }else{
                        col++;
                    }
                }
            }
        }

        /// <summary>
        /// This method call a function to get parcel.
        /// </summary>
        /// <param name="integerArray">Take array of int with two dimensions, here 10/10</param>
        /// <param name="mapArray">Take array of char with two dimensions, here 10/10</param>
        public static void Decode(int[,] integerArray, ref Char[,] mapArray)
        {
            int actualLetter = 96;
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    GetSeaOrForestOrParcel(ref actualLetter, integerArray, mapArray, row, col);
                }
            }
        }

        /// <summary>
        /// Show map
        /// </summary>
        /// <param name="p_mapArray"></param>
        public static void Display(ref Char[,] p_mapArray)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n{DateTime.Now} : Original Map");
            Console.ResetColor();
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    Console.Write(p_mapArray[row, col] + " ");
                }
                Console.WriteLine("");
            }
        }

        /// <summary>
        /// This method decode the type of a parcel like Sea Forest or the letter of a parcel.
        /// </summary>
        /// <paramref name="actualLetter">Take ref int</paramref>
        /// <param name="integerArray">Take array of int with two dimensions, here 10/10</param>
        /// <param name="mapArray">Take array of char with two dimensions, here 10/10</param>
        /// <param name="row">Take int</param>
        /// <param name="col">Take int</param>
        public static void GetSeaOrForestOrParcel(ref int actualLetter,int[,] integerArray, Char[,] mapArray, int row, int col){
            int value = integerArray[row,col];
            mapArray[row,col] = value >= _seaValue ? 
            'M' : value >= _forestValue ?
            'F' : GetLetter(ref actualLetter, integerArray, mapArray, row, col);
        }

        /// <summary>
        /// This method get letter according to the surronding parcel.
        /// </summary>
        /// <param name="actualLetter">Take ref int</param>
        /// <param name="integerArray">Take array of int with two dimensions, here 10/10</param>
        /// <param name="mapArray">Take array of char with two dimensions, here 10/10</param>
        /// <param name="row">Take int</param>
        /// <param name="col">Take int</param>
        /// <returns></returns>
        public static char GetLetter(ref int actualLetter, int[,] integerArray, Char[,] mapArray, int row, int col)
        {
            Boolean[] border = DecodeBorder(integerArray[row, col]);
            if (border[0] && border[1] && !border[3])
            {
                Boolean[] BorderValueRight = DecodeBorder(integerArray[row, col + 1]);
                mapArray[row, col] = !BorderValueRight[0] ?
                mapArray[row - 1, col + 1] : Convert.ToChar(actualLetter += 1);
            }
            // Border North, West, East
            mapArray[row, col] = border[0] && border[1] && border[3] ? Convert.ToChar(actualLetter += 1) : mapArray[row, col];
            // Border North
            mapArray[row, col] = !border[0] ? mapArray[row - 1, col] : mapArray[row, col];
            // Border West
            mapArray[row, col] = !border[1] ? mapArray[row, col - 1] : mapArray[row, col];
            return mapArray[row, col];
        }

        /// <summary>
        /// This method calls some functions to decode border.
        /// </summary>
        /// <param name="value">Take int</param>
        /// <returns>Boolean ("North","West","South","East")</returns>
        public static Boolean[] DecodeBorder(int value)
        {
            Boolean[] booleanBorder = { false, false, false, false }; // North, West, South, East
            value = GetSubstractionValue(value);
            GetBorder(value, booleanBorder);
            return booleanBorder;
        }

        /// <summary>
        /// This method substract value from general parcel like sea or forest.
        /// </summary>
        /// <param name="value">Take int</param>
        /// <returns>int value</returns>
        public static int GetSubstractionValue(int value){
            return value - _seaValue >= 0 ? 
            value-=_seaValue : value - _forestValue >= 0 ?
            value-=_forestValue : value;
        }

        /// <summary>
        /// This method get border from parcel
        /// </summary>
        /// <param name="value">Take int</param>
        /// <param name="booleanBorder">Take array of boolean</param>
        public static void GetBorder(int value, Boolean[] booleanBorder){
            int power = 8, counter = 3;
            // The power represent the mathematical power
            while (power != 0)
            {
                SetBorder(ref value,ref power,ref counter,booleanBorder);
                power/=2;
                counter--;
            }
        }

        /// <summary>
        /// This method set border from parcel
        /// </summary>
        /// <param name="value">Take ref int</param>
        /// <param name="power">Take ref int</param>
        /// <param name="counter">Take ref int</param>
        /// <param name="booleanBorder">Take array of boolean</param>
        public static void SetBorder(ref int value, ref int power, ref int counter, Boolean[] booleanBorder){
            if (value-power >= 0){
                value-=power;
                booleanBorder[counter] = true;
            }
        }

    }
}