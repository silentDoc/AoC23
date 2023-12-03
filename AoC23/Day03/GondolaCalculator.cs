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

        public bool AdjacentToSymbol(List<Coord2D> SymbolPositions)
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
        List<Coord2D> Symbols = new List<Coord2D>();
        List<Coord2D> Gears = new List<Coord2D>();

        List<FoundNumber> Numbers = new();

        public void ParseInput(List<string> lines)
        {
            for (int j = 0; j < lines.Count; j++)
            {
                var line = lines[j];
                line.ToCharArray();
                for (int i = 0; i < line.Length; i++)
                {
                    if (char.IsDigit(line[i]))
                    {
                        StringBuilder sb = new StringBuilder();
                        int initalColumn = i;
                        while (char.IsDigit(line[i]))
                        {
                            sb.Append(line[i]);
                            i++;

                            if (i == line.Length)
                                break;
                        }
                        int num = int.Parse(sb.ToString());
                        List<Coord2D> numPositions = new List<Coord2D>();
                        for (int range = initalColumn; range < i; range++)
                            numPositions.Add(new Coord2D(range, j));

                        FoundNumber foundNum = new FoundNumber(num, numPositions);
                        Numbers.Add(foundNum);
                        i--;
                        continue;
                    }

                    if (line[i] == '.')
                        continue;

                    if (line[i] == '*')
                        Gears.Add(new Coord2D(i, j));

                    // not a digit or a dot -- a symbol
                    Symbols.Add(new Coord2D(i,j));
                }
            }
            Console.WriteLine();
        }

        int SolvePart1()
            => Numbers.Where(x => x.AdjacentToSymbol(Symbols)).Sum(x => x.Number);

        int SolvePart2()
        {
            int sum = 0;
            foreach (var gearPosition in Gears)
            { 
                var listAdjacent = Numbers.Where(x => x.AdjacentToGear(gearPosition)).Select(x => x.Number).ToList();
                if(listAdjacent.Count <=1)
                    continue;

                sum += listAdjacent.Aggregate(1, (acc, val) => acc * val);
            }
            return sum;
        }
        

        public string Solve(int part)
            => part == 1 ? SolvePart1().ToString() : SolvePart2().ToString();
    }
}
