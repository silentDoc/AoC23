using AoC23.Common;

namespace AoC23.Day17
{
    class HeatLossMinimizer
    {
        Dictionary<Coord2D, int> Map = new();
        public void ParseInput(List<string> lines)
        {
            for (int r = 0; r < lines.Count; r++)
                for (int c = 0; c < lines[0].Length; c++)
                    Map[(c, r)] = int.Parse(lines[r][c].ToString());
        }


        int ShortestPath(Coord2D start, Coord2D end)
        {
            Queue<(Coord2D pos, int cost)> priorityQueue = new();
            HashSet<(Coord2D pos, int cost)> visited = new();
            priorityQueue.Enqueue((start, 0));
            int CurrentMinCost = 99999999;

            while (priorityQueue.Count > 0)
            {
                var item = priorityQueue.Dequeue();
                var mapCell = item.pos;
                var currentCost = item.cost;

                if (CurrentMinCost < currentCost)
                    continue;

                var newCost = currentCost + Map[mapCell];

                if (mapCell == end)
                {
                    if (CurrentMinCost > currentCost)
                        CurrentMinCost = currentCost;
                    continue;
                }

                var candidates = mapCell.GetNeighbors().Where(p => Map.Keys.Contains(p)).ToList();

                foreach (var candidate in candidates)
                    if (visited.Add((candidate, newCost)))
                        priorityQueue.Enqueue((candidate, newCost));
            }

            return CurrentMinCost;
        }

        int MinimizeLoss()
        {

            return 0;
        }

        public int Solve(int part)
            => MinimizeLoss();
    }
}
