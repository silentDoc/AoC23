using AoC23.Common;
using System.Text;

namespace AoC23.Day11
{
    internal class GalaxyTraveler
    {
        Dictionary<Coord2D, int> Space = new();

        void ParseLine(string line, int row)
        {
            for (int i = 0; i < line.Length; i++)
                if (line[i] == '#')
                    Space[(i, row)] = Space.Values.Count == 0 ? 0 : Space.Values.Max() + 1;
        }

        public void ParseInput(List<string> lines)
        {
            char[][] input = new char[lines.Count][];
            for (int i = 0; i < lines.Count; i++)
                input[i] = lines[i].ToCharArray();

            // Find additional rows and cols
            List<int> numAdditionalRows = new();
            List<int> numAdditionalCols = new();

            for (int i = 0; i < lines.Count; i++)
                if (!lines[i].Any(x => x == '#'))
                    numAdditionalRows.Add(i);

            for (int j = 0; j < lines[0].Length; j++)
            {
                var additionalCol = true;
                for (int i = 0; i < lines.Count; i++)
                    if (input[i][j] == '#')
                        additionalCol = false;

                if (additionalCol)
                    numAdditionalCols.Add(j);
            }
            // Rebuild the input
            List<string> newInput = new List<string>();
            
            for (int i = 0; i < lines.Count; i++)
            {
                StringBuilder line = new("");
                for (int j = 0; j < lines[0].Length; j++)
                {
                    line.Append(lines[i][j]);
                    if(numAdditionalCols.Contains(j))
                        line.Append(lines[i][j]);
                }
                newInput.Add(line.ToString());
                if (numAdditionalRows.Contains(i))
                    newInput.Add(line.ToString());
            }

            for (int i = 0; i < newInput.Count; i++)
                ParseLine(newInput[i], i);
        }

        int ShortestPath(Coord2D start, Coord2D end, int initial_cost = 0)
        {
            Queue<(Coord2D pos, int cost)> priorityQueue = new();
            HashSet<Coord2D> visited = new();
            priorityQueue.Enqueue((start, initial_cost));

            int MaxX = Space.Keys.Max(k => k.x);
            int MaxY = Space.Keys.Max(k => k.y);

            while (priorityQueue.Count > 0)
            {
                var item = priorityQueue.Dequeue();
                var mapCell = item.pos;
                var currentCost = item.cost;

                if (mapCell == end)
                    return currentCost;

                var newCost = currentCost + 1;
                // Check 0 and max
                var candidates = mapCell.GetNeighbors().Where(p => p.x >= 0 && p.x <= MaxX && p.y >= 0 && p.y <= MaxY).ToList();

                foreach (var candidate in candidates)
                    if (visited.Add(candidate))
                        priorityQueue.Enqueue((candidate, newCost));
            }

            return -1;
        }

        int SolvePart1()
        {
            var maxGalaxy = Space.Values.Max();
            int sum = 0;

            for (int i = 0; i <= maxGalaxy; i++)
            {
                var start = Space.Keys.First(x => Space[x] == i);
                for (int j = i + 1; j <= maxGalaxy; j++)
                {
                    var end = Space.Keys.First(x => Space[x] == j);
                    //sum += ShortestPath(start, end);
                    sum += start.Manhattan(end);
                }
            }

            return sum;
        }


        public int Solve(int part =1)
            => part ==1 ? SolvePart1() : 0;
    }
}
