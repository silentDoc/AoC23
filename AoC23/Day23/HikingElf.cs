using AoC23.Common;

namespace AoC23.Day23
{
    internal class HikingElf
    {
        Dictionary<Coord2D, char> Map = new();
        Coord2D UP = (0, -1);
        Coord2D DOWN = (0, 1);
        Coord2D RIGHT = (1, 0);
        Coord2D LEFT = (-1, 0);

        void ParseLine(string line, int row)
        {
            for (int col = 0; col < line.Length; col++)
                Map[(col, row)] = line[col];
        }

        public void ParseInput(List<string> lines)
        { 
            for(int r=0; r<lines.Count; r++)
                ParseLine(lines[r], r);
        }

        List<Coord2D> GetNextSteps(Coord2D current, HashSet<Coord2D> visited)
        { 
            var candidates = Map[current] switch
                            {
                                'v' => new List<Coord2D>() { current + DOWN },
                                '^' => new List<Coord2D>() { current + UP },
                                '<' => new List<Coord2D>() { current + LEFT },
                                '>' => new List<Coord2D>() { current + RIGHT },
                                '.' => current.GetNeighbors().Where(x => Map.ContainsKey(x) &&  Map[x] != '#').ToList(),
                                _ => throw new Exception("Invalid position")
                            };
            return candidates.Where(x => !visited.Contains(x)).ToList();
        }

        int LongestPath()
        {
            Coord2D start = (1, 0);
            Coord2D end = (Map.Keys.Max(k => k.x) - 1, Map.Keys.Max(k => k.y));

            HashSet<Coord2D> initVisited = new() { start };
            Queue<(Coord2D currPos, HashSet<Coord2D> visited)> activePaths = new();
            List<int> distances = new();

            activePaths.Enqueue((start, initVisited));

            // We want to find all the paths
            while (activePaths.Count > 0)
            { 
                var (currentPos, visited) = activePaths.Dequeue();
                var nextSteps = GetNextSteps(currentPos, visited);
                bool reachedEnd = false;
                
                // We advance until we find several choices
                while (nextSteps.Count == 1)
                {
                    currentPos = nextSteps.First();
                    visited.Add(currentPos);
                    
                    if (currentPos == end)
                    {
                        reachedEnd = true;
                        break;
                    }

                    nextSteps = GetNextSteps(currentPos, visited);
                }

                if (reachedEnd)
                {
                    // We reached the end with this path, but we must continue exploring the rest that
                    // remain active
                    distances.Add(visited.Count());
                    continue;
                }

                // Reached a cul-de-sac, so the path leads nowhere
                if (nextSteps.Count == 0)
                    continue;

                foreach (var step in nextSteps)
                {
                    var newVisited = new HashSet<Coord2D>(visited);
                    newVisited.Add(step);
                    activePaths.Enqueue((step, newVisited));
                }
            }

            return distances.Max()-1;   // We have the starting cell as a step, which does not count
        }

        public int Solve(int part = 1)
            => LongestPath();
    }
}
