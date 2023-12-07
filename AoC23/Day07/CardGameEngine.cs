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
        char[] CardPowerPart2 = new char[13] { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };

        public List<char> Cards = new();
        public List<char> Part2Cards = new();
        public int Bid = 0;
        public int Rank = 0;
        int Part = 1;

        public Hand(string inputLine, int part =1)
        {
            var parts = inputLine.Split(' ', StringSplitOptions.TrimEntries);
            Cards = parts[0].ToCharArray().ToList();
            Part2Cards = parts[0].ToCharArray().ToList();
            Bid = int.Parse(parts[1]);

            if (part == 2)
            {
                ProcessJokers();
                Rank = GetRank(Part2Cards);
                Part = 2;
            }
            else
                Rank = GetRank(Cards);
        }

        void ProcessJokers()
        {
            // Special case
            if (Cards.Count(x => x == 'J') == 5)
            {
                for (int i = 0; i < Part2Cards.Count; i++)
                    Part2Cards[i] = 'A';
                return;
            }


            var cardsNoJokers = Cards.Where(x => x != 'J').ToList();
            var occurences = cardsNoJokers.Distinct().Select(x => Cards.Count(c => c == x)).ToList();
            var maxOcc = occurences.Max();

            // Find the best card
            char bestCard = 'f';
            if (occurences.Count(x => x == maxOcc) > 1)
            {
                // Two tying cards, find the best one
                var tyingCards = cardsNoJokers.Where(x => cardsNoJokers.Count(y => y == x) == maxOcc).ToList();
                var maxPower = tyingCards.Max(x => Array.IndexOf(CardPower, x));

                bestCard = tyingCards.First(x => Array.IndexOf(CardPower, x) == maxPower);
            }
            else
                bestCard = cardsNoJokers.First( x => cardsNoJokers.Count(c => c ==x) == maxOcc);

            // Replace the Joker by the best hand
            for (int i = 0; i < Part2Cards.Count; i++)
                if (Part2Cards[i] == 'J')
                    Part2Cards[i] = bestCard;
        }

        public int GetPowerByPos(int pos)
            => Part == 1 ?  Array.IndexOf(CardPower, Cards[pos])
                         :  Array.IndexOf(CardPowerPart2, Cards[pos]);

        int GetRank(List<char> cards)
        {
            var occurences = cards.Distinct().Select(x => cards.Count(c => c == x)).ToList();
            var rank = 0;

            if (occurences.Any(x => x == 5))
                rank = HandRank.Repoker;
            else if (occurences.Any(x => x == 4))
                rank = HandRank.Poker;
            else if (occurences.Any(x => x == 3) && occurences.Any(x => x == 2))
                rank = HandRank.FullHouse;
            else if (occurences.Any(x => x == 3))
                rank = HandRank.ThreeFold;
            else if (occurences.Count(x => x == 2) == 2)
                rank = HandRank.TwoPair;
            else if (occurences.Any(x => x == 2))       // Else above prevents from counting two pair or fullhouse
                rank = HandRank.Pair;
            else
                rank = HandRank.HighCard;

            return rank;
        }
    }

    public class CardGameEngine
    {
        List<Hand> hands = new();
        public void ParseInput(List<string> lines, int part =1)
            => lines.ForEach(x => hands.Add(new Hand(x, part)));

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

        public int RunGame(int part = 1)
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
            => RunGame(part);
    }
}
