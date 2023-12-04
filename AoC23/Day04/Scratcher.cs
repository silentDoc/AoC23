namespace AoC23.Day04
{
    public class ScratchCard
    {
        List<int> CardNumbers = new();
        List<int> WinningNumbers = new();
        int CardNum = 0;
        public ScratchCard(string inputLine)
        {
            var info = inputLine.Split(":");
            CardNum = int.Parse(info[0].Replace("Card ", ""));

            var parts = info[1].Split("|");
            CardNumbers = parts[1].Split(" ", StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
            WinningNumbers = parts[0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
        }

        public int Points()
        {
            int appearences = CardNumbers.Where(x => WinningNumbers.Contains(x)).Count();
            return (int) Math.Pow(2, appearences - 1);
        }
        
    }

    internal class Scratcher
    {
        List<ScratchCard> cards = new();
        public void ParseInput(List<string> lines)
            => lines.ForEach(x => cards.Add(new ScratchCard(x)));

        int SolvePart2()
            => 0;


        public int Solve(int part)
            => part == 1 ? cards.Sum(x => x.Points())
                         : SolvePart2();
    }
}
