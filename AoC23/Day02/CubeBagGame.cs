using System.Xml.Schema;

namespace AoC23.Day02
{
    public record HandOfGame
    {
        public int Red = 0;
        public int Green = 0;
        public int Blue = 0;
    }

    public class CubeBagGame
    {
        List<(int id, List<HandOfGame> game)> allGames = new();


        public (int id, List<HandOfGame> game) ParseGame(string line) 
        {
            // To improve with regex

            var parts = line.Split(':');
            var hands = parts[1].Split(';');
            int gameid = int.Parse(parts[0].Replace("Game ", ""));

            List<HandOfGame> allhands = new();
            foreach(var h in hands)
            {
                HandOfGame hand = new HandOfGame();
                var values = h.Split(',');
                foreach (var v in values)
                {
                    if (v.Contains("red"))
                        hand.Red = int.Parse(v.Replace(" red", ""));
                    else if (v.Contains("blue"))
                        hand.Blue = int.Parse(v.Replace(" blue", ""));
                    else if (v.Contains("green"))
                        hand.Red = int.Parse(v.Replace(" green", ""));

                }
                allhands.Add(hand);
            }

            return (gameid, allhands);
        }

        public bool CheckPossibleGame(List<HandOfGame> game, HandOfGame limit)
        {
            return false;
        }
    }
}
