using AoC23.Common;

namespace AoC23.Day21
{
    internal class WalkingElf
    {
        Dictionary<Coord2D, int> Map = new();

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
        }

        int TraverseMaps(int maxSteps, Coord2D start)
        {
            Queue<Coord2D> active = new();
            active.Enqueue(start);
            int steps = 1;
            int numQueueElements = 1;   // The number of steps increments only when 1 level of the queue has been processed

            while (active.Count > 0 && steps <=maxSteps)
            {
                var current = active.Dequeue();
                var neighs = current.GetNeighbors().Where(n => Map.Keys.Contains(n) && Map[n]!=-10).ToList();

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

        int FindSpots(int maxSteps)
        {
            var start = Map.Keys.First(x => Map[x] == 0);
            return TraverseMaps(maxSteps, start);
        }

        public int Solve(int part = 1)
            => FindSpots(64);
    }
}
