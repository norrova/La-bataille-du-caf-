using Xunit;
using GameCore;
using ServerCore;

namespace LabatailleducafeTest
{
    public class GameTest
    {
        [Fact]
        public void Checker()
        {
            Game v_game = new Game();
            Assert.True(new Game().Checker("FINI", Response.FINI));
            Assert.True(new Game().Checker("VALI", Response.VALI));
            Assert.True(new Game().Checker("ENCO", Response.ENCO));
            Assert.True(new Game().Checker("INVA", Response.INVA));

            Assert.False(new Game().Checker("FINI", Response.ENCO));
            Assert.False(new Game().Checker("ENCO", Response.FINI));
        }
    }
}
