using AoC23.Common;

namespace AoC23.Day17
{
    class SearchState : IEquatable<SearchState>
    {
        public Coord2D Pos;
        public int Streak;
        public Coord2D Dir;

        public SearchState(Coord2D pos, Coord2D dir, int streak)
        {
            Pos = pos;
            Streak = streak;
            Dir = dir;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + Pos.GetHashCode();
            hash = hash * 23 + Dir.GetHashCode();
            hash = hash * 19 + Streak;

            return hash;
        }

        bool IEquatable<SearchState>.Equals(SearchState? other)
        {
            if (other == null)
                return false;
            return Pos.Equals(other.Pos) && Dir.Equals(other.Dir) && Streak == other.Streak;
        }
    }

    class HeatLossMinimizer
    {
        Dictionary<Coord2D, int> Map = new();

        Coord2D UP = (0, -1);
        Coord2D DOWN = (0, 1);
        Coord2D RIGHT = (1, 0);
        Coord2D LEFT = (-1, 0);

        public void ParseInput(List<string> lines)
        {
            for (int r = 0; r < lines.Count; r++)
                for (int c = 0; c < lines[0].Length; c++)
                    Map[(c, r)] = int.Parse(lines[r][c].ToString());
        }


        Coord2D TurnLeft(Coord2D currentDir)
            => currentDir switch
            {
                (0, 1) => RIGHT,
                (0, -1) => LEFT,
                (1, 0) => UP,
                (-1, 0) => DOWN,
                (_, _) => throw new Exception("Invalid direction")
            };

        Coord2D TurnRight(Coord2D currentDir)
            => currentDir switch
            {
                (0, 1) => LEFT,
                (0, -1) => RIGHT,
                (1, 0) => DOWN,
                (-1, 0) => UP,
                (_, _) => throw new Exception("Invalid direction")
            };

        // Day 17 Forces us to modify the usual state of a visited tile. The state of a tile will be defined by a separate class : 
        // - The tile position
        // - The current cost
        // - The direction
        // - The current streak of turns going in the current direction
        // The twist is that we will consider that a tile is visited if the current tile has
        //    less cost than current, with same direction as current, will less or equal streak.
        // This can be seen in the elements of the priority queue and visited dictionary (not hashmap because we need costs)
        int ShortestPath(Coord2D start, Coord2D startDir, Coord2D end)
        {
            var priorityQueue = new PriorityQueue<SearchState, int>();
            Dictionary<SearchState, int> visited = new();

            var startState = new SearchState(start, startDir, 0);
            visited[startState] = 0;
            priorityQueue.Enqueue(startState, 0);

            int minCostBFS = 999999;

            while (priorityQueue.Count > 0)
            {
                priorityQueue.TryDequeue(out var state, out int currentCost);
                var currentPos = state.Pos;
                var currentDir = state.Dir;
                var currentStreak = state.Streak;

                if (minCostBFS < currentCost)
                    continue;

                if (currentPos == end)
                    return currentCost;

                // Find possible new directions
                List<Coord2D> nextDirs = new(){currentDir, TurnLeft(currentDir), TurnRight(currentDir)};
                if (currentStreak >= 3)
                    nextDirs.Remove(currentDir);

                int nextStreak = 0;
                foreach (var nextDir in nextDirs)
                {
                    if (nextDir == currentDir)
                        nextStreak = currentStreak + 1;
                    else
                        nextStreak = 1;

                    var nextPos = currentPos + nextDir;
                    
                    if (!Map.ContainsKey(nextPos))
                        continue;

                    var nextCost = visited[state] + Map[nextPos];
                    var nextState = new SearchState(nextPos, nextDir, nextStreak);

                    int visitedCost = -1;
                    if (visited.ContainsKey(nextState))
                        visitedCost = visited[nextState];

                    if (visitedCost == -1 || nextCost < visitedCost)
                    {
                        visited[nextState] = nextCost;
                        priorityQueue.Enqueue(nextState, nextCost);
                    }
                }
            }

            return minCostBFS;
        }

        int MinimizeLoss()
        {
            Coord2D startPos = (0, 0);
            Coord2D endPos = (Map.Keys.Max(p => p.x), Map.Keys.Max(p => p.y));
            Coord2D dirRight = (1, 0);
            Coord2D dirDown = (1, 0);

            var min1 = ShortestPath(startPos, dirRight, endPos);
            var min2 = ShortestPath(startPos, dirDown, endPos);

            return Math.Min(min1, min2);
        }

        public int Solve(int part)
            => MinimizeLoss();
    }
}
