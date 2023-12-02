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

        public void ParseInput(List<string> input)
            => input.ForEach(x => allGames.Add(ParseGame(x)));

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
                        hand.Green = int.Parse(v.Replace(" green", ""));

                }
                allhands.Add(hand);
            }

            return (gameid, allhands);
        }

        public bool CheckPossibleGame(List<HandOfGame> game, HandOfGame limit)
            => !(game.Any(x => x.Red > limit.Red) ||
                 game.Any(x => x.Blue > limit.Blue) ||
                 game.Any(x => x.Green > limit.Green));


        public int FindGamePower(List<HandOfGame> game)
            => 0;

        int SolvePart1()
        {
            HandOfGame limit = new();
            limit.Red = 12;
            limit.Green = 13;
            limit.Blue = 14;

            return allGames.Where(x => CheckPossibleGame(x.game, limit)).Sum(g => g.id);
        }

        public string Solve(int part)
            => part == 1 ? SolvePart1().ToString() : "";
    }
}
