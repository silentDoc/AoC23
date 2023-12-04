using AoC23.Common;
using System.Text;

namespace AoC23.Day03
{
    class FoundNumber
    { 
        public int Number { get; set; }
        List<Coord2D> Positions { get; set; } = new List<Coord2D>();

        public FoundNumber(int number, List<Coord2D> positions) 
        {
            Number = number;
            Positions = positions;
        }

        public bool AdjacentToSymbol(HashSet<Coord2D> SymbolPositions)
        {
            var allNeighbours = Positions.SelectMany(x => x.GetNeighbors8());
            return allNeighbours.Any( p => SymbolPositions.Contains(p));
        }

        public bool AdjacentToGear(Coord2D gearPosition)
        {
            var allNeighbours = Positions.SelectMany(x => x.GetNeighbors8());
            return allNeighbours.Contains(gearPosition);
        }
    }

    public  class GondolaCalculator
    {
        HashSet<Coord2D> Symbols = new HashSet<Coord2D>();
        HashSet<Coord2D> Gears = new HashSet<Coord2D>();
        List<FoundNumber> Numbers = new();

        public void ParseInput(List<string> lines)
        {
            for (int j = 0; j < lines.Count; j++)
            {
                var line = lines[j];
                line.ToCharArray();
                for (int i = 0; i < line.Length; i++)
                {
                    if (char.IsDigit(line[i]))      // A number will occupy a list of positions (its digits')
                    {
                        StringBuilder sb = new StringBuilder();
                        int initalColumn = i;
                        while (i < line.Length && char.IsDigit(line[i]))
                        {
                            sb.Append(line[i]);
                            i++;
                        }

                        int num = int.Parse(sb.ToString());
                        List<Coord2D> numPositions = new List<Coord2D>();
                        for (int range = initalColumn; range < i; range++)
                            numPositions.Add(new Coord2D(range, j));

                        Numbers.Add( new FoundNumber(num, numPositions));
                        i--;    // The last increment could skip a symbol just after a digit
                    }
                    else if (line[i] != '.')    // Anything not a digit or a dot is a symbol
                    {
                        Symbols.Add(new Coord2D(i, j));
                        if (line[i] == '*')
                            Gears.Add(new Coord2D(i, j));
                    }
                }
            }
        }

        int SolvePart2()
        {
            int sum = 0;
            foreach (var gearPosition in Gears)
            { 
                var listAdjacent = Numbers.Where(x => x.AdjacentToGear(gearPosition)).Select(x => x.Number).ToList();
                if(listAdjacent.Count >1)
                    sum += listAdjacent.Aggregate(1, (acc, val) => acc * val);
            }
            return sum;
        }
        public string Solve(int part)
            => part == 1 ? Numbers.Where(x => x.AdjacentToSymbol(Symbols)).Sum(x => x.Number).ToString()
                         : SolvePart2().ToString();
    }
}
