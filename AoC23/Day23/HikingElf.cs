using AoC23.Common;

namespace AoC23.Day23
{
    class MapEdge
    {
        public Coord2D A = (0, 0);
        public Coord2D B = (0, 0);
        public int cost = 0;
    }

    internal class HikingElf
    {
        List<MapEdge> compressedMapEdges = new();
        HashSet<Coord2D> compressedMapNodes = new();

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
                                '.' => current.GetNeighbors().Where(x => Map.ContainsKey(x) &&  Map[x] != '#' && !visited.Contains(x)).ToList(),
                                _ => throw new Exception("Invalid position")
                            };
            return candidates.Where(x => !visited.Contains(x)).ToList();
        }

        List<Coord2D> GetNextStepsP2(Coord2D current, HashSet<Coord2D> visited)
            => current.GetNeighbors().Where(x => Map.ContainsKey(x) && Map[x] != '#' && !visited.Contains(x)).ToList();

        // Part 1
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
                
                // We advance until we find several choices - This speeds up dramatically because we do not use the queue
                // or replicate the hashset only for 1 more cell
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

        // Part 2
        void BuildGraph(Coord2D startWalk, Coord2D startNode)
        {
            // Traverse the whole map and find cells that have bifurcations
            Coord2D end = (Map.Keys.Max(k => k.x) - 1, Map.Keys.Max(k => k.y));
            HashSet<Coord2D> visited = new() { startNode , startWalk };
            var currentPos = startWalk;
            var nextSteps = GetNextStepsP2(currentPos, visited);
            bool reachedEnd = false;

            // Like in part 1 - compressing the graph means advancing while we have no bifurcation and 
            // creating a graph with cost (number of steps) for this sub path
            while (nextSteps.Count == 1)
            {
                currentPos = nextSteps.First();
                visited.Add(currentPos);

                if (currentPos == end)
                {
                    reachedEnd = true;
                    break;
                }
                nextSteps = GetNextStepsP2(currentPos, visited);
            }

            if (reachedEnd)
            {
                // Add the node
                var edge = new MapEdge();
                edge.A = startNode;
                edge.B = end;
                edge.cost = visited.Count - 1;
                compressedMapEdges.Add(edge);
                compressedMapNodes.Add(end);
                return;
            }

            // Reached a cul-de-sac, so the path leads nowhere
            if (nextSteps.Count == 0)
                return;

            // We are in a node with several alternatives
            // We may have the edge already

            var existingEdge = GetEdge(startNode, currentPos);

            if (existingEdge == null)
            {
                var edgeBifurc = new MapEdge();
                edgeBifurc.A = startNode;
                edgeBifurc.B = currentPos;
                edgeBifurc.cost = visited.Count - 1;
                compressedMapEdges.Add(edgeBifurc);

                if (compressedMapNodes.Add(currentPos))
                    foreach (var step in nextSteps)
                    {
                        if (visited.Contains(step)) // We skip goint backwards
                            continue;
                        BuildGraph(step, currentPos);
                    }
            }
        }

        List<Coord2D> GetNeighNodes(Coord2D node, HashSet<Coord2D> visited)
        {
            List<Coord2D> retVal = compressedMapEdges.Where(e => e.A == node).Select(x => x.B).ToList();
            retVal.AddRange(compressedMapEdges.Where(e => e.B == node).Select(x => x.A).ToList());
            retVal = retVal.Where(x => !visited.Contains(x)).ToList();
            return retVal;
        }
      
        MapEdge? GetEdge(Coord2D node1, Coord2D node2)
            => compressedMapEdges.FirstOrDefault(x => x.A == node1 && x.B == node2) ?? compressedMapEdges.FirstOrDefault(x => x.B == node1 && x.A == node2);
        

        int SolvePart2()
        {
            Coord2D start = (1, 0);
            Coord2D end = (Map.Keys.Max(k => k.x) - 1, Map.Keys.Max(k => k.y));
            compressedMapNodes.Add(start);
            
            BuildGraph(start, start);  // Transforms the map into a graph

            // I suppose I could use some variatio of Dijkstra to solve it more quickly, the current BFS solution takes almost 5 mins
            // I will try to improve it some day - but not today :)
            HashSet<Coord2D> visitedNodes = new();
            Queue <(Coord2D current,int cost, HashSet<Coord2D> visited)> activeNodes = new();
            activeNodes.Enqueue((start, 0, visitedNodes));
            List<int> distances = new();

            while (activeNodes.Count > 0)
            {
                var (currentNode, currentCost, visited) = activeNodes.Dequeue();
                var neighs = GetNeighNodes(currentNode, visited).Distinct();
                visited.Add(currentNode);

                foreach (var n in neighs)
                {
                    var edge = GetEdge(currentNode, n);
                    if (edge == null)
                        throw new Exception("Map is not properly compressed");
                    var nextCost = currentCost + edge.cost;
                    if (n == end)
                    {
                        distances.Add(nextCost);
                    }
                    else
                    {
                        HashSet<Coord2D> newVisitedNodes = new(visited);
                        activeNodes.Enqueue((n, nextCost, newVisitedNodes));
                    }
                }
            }
            return distances.Max();
        }

        public int Solve(int part = 1)
            => part == 1 ? LongestPath() : SolvePart2();
    }
}
