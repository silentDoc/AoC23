using AoC23.Common;

namespace AoC23.Day21
{
    internal class WalkingElf
    {
        Dictionary<Coord2D, int> Map = new();
        int inputSize = 0;

        void ParseLine(string line, int row)
        {
            for (int c = 0; c < line.Length; c++)
                Map[(c, row)] = line[c] switch
                {
                    '.' => -1,
                    'S' => 0,
                    '#' => -10,
                    _ => throw new Exception("Invalid map symbol")
                };
        }

        public void ParseInput(List<string> lines)
        { 
            for(int r= 0; r < lines.Count;r++)
                ParseLine(lines[r], r);

            inputSize = lines.Count;    // According to the input, we have a 131x131 grid
        }

        int TraverseMaps(int maxSteps, Coord2D start, int part =1)
        {
            HashSet<Coord2D> currentSet = new() { start};
            for (int steps = 0; steps < maxSteps; steps++)
            {
                var nextSet = new HashSet<Coord2D>();
                foreach (var current in currentSet)
                {
                    var neighs = (part == 1) ? current.GetNeighbors().Where(n => Map.Keys.Contains(n) && Map[n] != -10).ToList() 
                                             : current.GetNeighbors().ToList();
                    if (part == 2)  // To support infinite grid
                    {
                        foreach (var n in neighs)
                            if (Map[(Modulo(n.x, 131), Modulo(n.y, 131))] != -10)   // We check for the module to locate it in the original map
                                nextSet.Add(n);                                     // but we have to store the actual position in the hashset because it is not a duplicate
                    }
                    else
                        neighs.ForEach(x => nextSet.Add(x));
                }
                currentSet = nextSet;
            }

            return currentSet.Count();
        }

        int Modulo(int x, int mod)
            => ((x % mod) + mod) % mod;

        long CalcPart2()
        {
            // Facts and ideas to help solve this one:
            // 
            // We have an input that has 131x131 (inputSize) dimensions.
            // To know the number of maximum grids we will have ==> Steps / inputSize ==> 26501365 / 131
            // There will be some more positions to walk , we get them with remainder ==> 26501365 % 131
            // The problem follows a chess board pattern (black/white or even/odd) ==> In one step, we are either on even/odd squares (even/odd would be sum of row+col)
            // The nature of Manhattan distance will make that imaginary grid of grids expands like a circle (romboid):
            //
            //                #
            //               ###
            //              ##### 
            //               ###
            //                #
            //
            // So this points as sort of solution where we can find the number of positions to reach within all the original grids, multiply it by the number of grids 
            // and then add the remainder (the extra steps we can take out of the romboid)
            // The trick is that we can go back, for instance we can go left from grid 0 to grid -2 and then walk backwards ... so the solution has to be quadratic.
            // If we have only 1 grid, the number of positions is finite, either even or odd
            // If we have 5 grids (original + romboid up, down, left, right), the number of positions is 5 * 1 grid solution
            // We can take this lineally, our total is 
            // originalGridSteps + 2* originalGridSteps + 3 ... ?? - not quite I think
            // When we take remainders into account we have (remainder + num * originalGrid) => (rem), (rem + originalGridSteps), (rem + 2*originalGridSteps),
            // The solution points to a quadratic form : a*x^2 + b*x + c ?? In order to find them, the three sequences above should be enough

            // Also, bear in mind that the coords may be negative, so remainder is not helful --> we will have to use Modulo
            //
            // If the solution is in cuadratic form ( a*x^2 + b*x + c ) where x is the number of grids, we can solve the system for 0, 1 and 2 grids and find out a,b and c
            // because we know the number of max grids (26501365 / 131)
            //
            // a*0 + b*0 + c = spots found with remainder steps
            // a*1^2 + b*1 + c = spots found with remainder steps + max length 1 grid
            // a*2^2 + b*2 + c = spots found with remainder steps + max length 2 grids
            // 
            // This leaves a system of equations:
            // c = spots found with remainder steps
            // a+b+c = spots found with remainder steps + max length 1 grid
            // 4a +2b+c = spots found with remainder steps + max length 2 grids
            // 
            // c --> direct
            // a+b = spots1 - c
            // 4a + 2b = spots2 -c

            // We look for 3 coefficients :
            long part2Steps = 26501365;
            long gridLength = 131;
            long numGrids = part2Steps / inputSize;
            long remainder = part2Steps % ((long)inputSize);
            var start = Map.Keys.First(x => Map[x] == 0);
            List<long> spots = new();

            for (int currentNumGrids = 0; currentNumGrids < 3; currentNumGrids++)
            {
                int maxSteps = (int)  (remainder + currentNumGrids * gridLength);
                Console.WriteLine(currentNumGrids + " - " + maxSteps);
                var count = TraverseMaps(maxSteps, start, 2); // For part 2, we cannot reuse part 1 because it is too slow
                spots.Add(count);
                Console.WriteLine(count);
            }

            // ax^2 + bx + c ==> Solving formula
            // c = spots[0]
            // a*1 + b*1 + c = spots[1]  ( X = 1)
            // a*2^2 + b*2 + c = spots[2]  ( X = 2)

            long c = spots[0];
            long aPlusB = spots[1] - c;
            long fourAPlusTwoB = spots[2] - c;
            long twoA = fourAPlusTwoB - (2 * aPlusB);
            long a = twoA / 2;
            long b = aPlusB - a;

            long x2 = numGrids * numGrids;
            long ax2 = a * x2;
            long bx = b * numGrids;

            return ax2 + bx + c;
        }


        int FindSpots(int maxSteps)
        {
            var start = Map.Keys.First(x => Map[x] == 0);
            return TraverseMaps(maxSteps, start);
        }

        public long Solve(int part = 1)
            => part == 1 ? (long)FindSpots(64) : CalcPart2();
    }
}
