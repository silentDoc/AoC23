using AoC23.Day02;

namespace AoC23Tests
{
    [TestClass]
    public class Day02_Tests
    {
        [DataTestMethod]
        [DataRow("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green", true)]
        [DataRow("Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue", true)]
        [DataRow("Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red", false)]
        [DataRow("Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red", false)]
        [DataRow("Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", true)]
        public void Day02_Part1_Sample_Input_Test(string input, bool expected)
        {
            var gameEngine = new CubeBagGame();
            (int id, List<HandOfGame> handOfGame) = gameEngine.ParseGame(input);

            HandOfGame limit = new();
            limit.Red = 12;
            limit.Green = 13;
            limit.Blue = 14;

            var result = gameEngine.CheckPossibleGame(handOfGame, limit);

            Assert.AreEqual(expected, result);
        }

    }


}
