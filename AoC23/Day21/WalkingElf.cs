using AoC23.Common;
using AoC23.Day19;

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
            Queue<Coord2D> active = new();
            active.Enqueue(start);
            int steps = 1;
            int numQueueElements = 1;   // The number of steps increments only when 1 level of the queue has been processed

            while (active.Count > 0 && steps <=maxSteps)
            {
                var current = active.Dequeue();
                var neighs = (part==1) ? current.GetNeighbors().Where(n => Map.Keys.Contains(n) && Map[n]!=-10).ToList() :
                                         current.GetNeighbors().ToList();

                if (part == 2)  // To support infinite grid
                {
                    foreach (var n in neighs)
                    {
                        n.x = Modulo(n.x, 131);
                        n.y = Modulo(n.y, 131);
                    }
                    neighs = neighs.Where(n => Map.Keys.Contains(n) && Map[n] != -10).ToList();
                }

                foreach (var n in neighs)
                    if(!active.Contains(n))
                        active.Enqueue(n);

                numQueueElements--;
                if (numQueueElements == 0)
                {
                    steps++;
                    numQueueElements = active.Count();
                }
            }
            return active.Count();
        }

        int Modulo(int x, int mod)
            => x >0 ? x % mod : (x-x) % mod;


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
            // originalGridSteps + 2* originalGrid + 3 ...
            // When we take remainders into account we have (remainder + num * originalGrid) => (rem), (rem + originalGridSteps), (rem + 2*originalGridSteps),
            // The solution points to a quadratic form : a*x^2 + b*x + c = 0 ?? In order to find them, the three sequences above should be enough

            // Also, bear in mind that the coords may be negative, so remainder is not helful --> we will have to use Modulo

            // We look for 3 coefficients :
            long part2Steps = 26501365;
            long numGrids = part2Steps / ((long)inputSize);
            long remainder = part2Steps % ((long)inputSize);
            var start = Map.Keys.First(x => Map[x] == 0);
            List<long> quadratic = new();

            for (int coeff = 0; coeff < 3; coeff++)
            {
                int maxSteps = (int)  (remainder + coeff * numGrids);
                var count = TraverseMaps(maxSteps, start, 2);
                quadratic.Add(count);
                Console.WriteLine(count);
            }

            // ax^2 + bx + c ==> Solving formula => x = (x = 

            var c = quadratic[0];
            var aPlusB = quadratic[1] - c;
            var fourAPlusTwoB = quadratic[2] - c;
            var twoA = fourAPlusTwoB - (2 * aPlusB);
            var a = twoA / 2;
            var b = aPlusB - a;

            return a * numGrids * numGrids + b * numGrids + c;
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
