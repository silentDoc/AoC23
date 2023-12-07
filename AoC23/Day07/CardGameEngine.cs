using System.Linq;

namespace AoC23.Day07
{
    public static class HandRank
    {
        public static int HighCard = 0;
        public static int Pair = 1;
        public static int TwoPair = 2;
        public static int ThreeFold = 3;
        public static int FullHouse = 4;
        public static int Poker = 5;
        public static int Repoker = 6;
    }

    public class Hand
    {
        char[] CardPower = new char[13] { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };

        public List<char> Cards = new();
        public int Bid = 0;
        public int Rank = 0;

        public Hand(string inputLine)
        {
            var parts = inputLine.Split(' ', StringSplitOptions.TrimEntries);
            Cards = parts[0].ToCharArray().ToList();
            Bid = int.Parse(parts[1]);
            GetRank();
        }

        public int GetPowerByPos(int pos)
            => Array.IndexOf(CardPower, Cards[pos]);

        void GetRank()
        {
            var occurences = Cards.Distinct().Select(x => Cards.Count(c => c == x)).ToList();

            if (occurences.Any(x => x == 5))
                Rank = HandRank.Repoker;
            else if (occurences.Any(x => x == 4))
                Rank = HandRank.Poker;
            else if (occurences.Any(x => x == 3) && occurences.Any(x => x == 2))
                Rank = HandRank.FullHouse;
            else if (occurences.Any(x => x == 3))
                Rank = HandRank.ThreeFold;
            else if (occurences.Count(x => x == 2) == 2)
                Rank = HandRank.TwoPair;
            else if (occurences.Any(x => x == 2))       // Else above prevents from counting two pair or fullhouse
                Rank = HandRank.Pair;
            else
                Rank = HandRank.HighCard;
        }
    }

    public class CardGameEngine
    {
        List<Hand> hands = new();
        public void ParseInput(List<string> lines)
            => lines.ForEach(x => hands.Add(new Hand(x)));

        Hand FindBestHandByPower(List<Hand> hands)
        {
            int pos = 0;
            var power = hands.Max(x => x.GetPowerByPos(pos));
            var topSet = hands.Where(x => x.GetPowerByPos(pos) == power).ToList();

            while (topSet.Count > 1)
            {
                pos++;
                power = topSet.Max(x => x.GetPowerByPos(pos));
                topSet = topSet.Where(x => x.GetPowerByPos(pos) == power).ToList();
            }

            return topSet[0];
        }

        public int SolvePart1()
        {
            List<Hand> orderedGame = new();

            var allPossibleRanks = hands.Select(x => x.Rank).Distinct().OrderByDescending(x => x);
            foreach (var r in allPossibleRanks)
            {
                var orderedRank = hands.Where(h => h.Rank == r).ToList();
                // All these hands have the same figure, they are tied
                while (orderedRank.Count > 0)
                {
                    var best = FindBestHandByPower(orderedRank);
                    orderedGame.Add(best);
                    orderedRank.Remove(best);
                }
            }

            int result = 0;
            orderedGame.Reverse();  // I added the best hands at the beginning
            for (int i = 0; i < orderedGame.Count; i++)
                result += (i + 1) * orderedGame[i].Bid;

            return result;
        }


        public int Solve(int part)
            => part == 1 ? SolvePart1() : 0;
    }
}
