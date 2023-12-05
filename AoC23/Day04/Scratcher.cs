namespace AoC23.Day04
{
    public class ScratchCard
    {
        List<int> CardNumbers = new();
        List<int> WinningNumbers = new();
        public long Copies = 1;

        public ScratchCard(string inputLine)
        {
            var info = inputLine.Split(":");
            var parts = info[1].Split("|");
            CardNumbers    = parts[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
            WinningNumbers = parts[0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
        }

        public int Points
            => (int) Math.Pow(2, MatchingNumbers - 1);

        public int MatchingNumbers
            => CardNumbers.Count(x => WinningNumbers.Contains(x));
    }

    internal class Scratcher
    {
        List<ScratchCard> cards = new();
        public void ParseInput(List<string> lines)
            => lines.ForEach(x => cards.Add(new ScratchCard(x)));

        long SolvePart2()
        {
            // Process deck
            for (int i = 0; i < cards.Count; i++)
                for (int j = i + 1; j <= i + cards[i].MatchingNumbers; j++)
                    if (j < cards.Count)
                        cards[j].Copies += cards[i].Copies; 

            return cards.Sum(x => x.Copies);
        }

        public long Solve(int part)
            => part == 1 ? (long) cards.Sum(x => x.Points)
                         : SolvePart2();
    }
}
