using System;
using Xunit;
using GameCore;

namespace LabatailleducafeTest.GameBoardTest
{
    public class BorderTest
    {
        [Fact]
        public void DecodeBorderTest()
        {
            int value = 3;
            // border North and West
            Boolean[] border = { true, true, false, false };
            Assert.Equal(Decoder.DecodeBorder(value), border);

            value = 67;
            Boolean[] border2 = { true, true, false, false };
            Assert.Equal(Decoder.DecodeBorder(value),border2);
        }

        [Fact]
        public void GetSubstractionValueTest()
        {
            int value = 64;
            Assert.Equal(Decoder.GetSubstractionValue(value), 0);

            value = 63;
            Assert.Equal(Decoder.GetSubstractionValue(value), 31);

            value = 32;
            Assert.Equal(Decoder.GetSubstractionValue(value), 0);

            value = 31;
            Assert.Equal(Decoder.GetSubstractionValue(value), 31);
        }
    }
}
