using System.Linq;
using System.Reflection.Metadata.Ecma335;
namespace AoC23.Day25
{
    internal class SnowWires
    {
        // https://en.wikipedia.org/wiki/Algebraic_connectivity ??
        // https://en.wikipedia.org/wiki/Stoer%E2%80%93Wagner_algorithm ??
        // https://en.wikipedia.org/wiki/Karger%27s_algorithm
        // AoC lets you learn new stuff - I will try Karger algorithm first because it is the simplest :D

        Dictionary<string, HashSet<string>> connections = new();

        public void ParseLine(string line)
        {
            var vs = line.Replace(":", "").Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (!connections.ContainsKey(vs[0]))
                connections[vs[0]] = new();

            foreach (var v in vs.Skip(1))
            {
                if (!connections.ContainsKey(v))
                    connections[v] = new();
                connections[vs[0]].Add(v);
                connections[v].Add(vs[0]);
            }
        }

        protected int HowManyCuts(List<List<string>> subGraphs, List<(string v1, string v2)> edges)
        {
            int result = 0;
            for (int edge = 0; edge < edges.Count; edge++)
            {
                var subGraph1 = subGraphs.Where(s => s.Contains(edges[edge].v1)).First();
                var subGraph2 = subGraphs.Where(s => s.Contains(edges[edge].v2)).First();
                if (subGraph1 != subGraph2) 
                    result++;
            }

            return result;
        }

        public int Karger3Cuts()
        {
            // Build Edges
            List<(string v1, string v2)> edges = new();
            foreach (var vertex1 in connections.Keys)
                foreach (var vertex2 in connections[vertex1])
                    if (!edges.Contains((vertex1, vertex2)) && !edges.Contains((vertex2, vertex1)))
                        edges.Add((vertex1, vertex2));

            List<List<string>> subsets = new List<List<string>>();
            
            var numCuts = -1;

            while(numCuts !=3)      // This is key, Karger algorithm cannot ensure that the cut is minimal, but the problem tells us there are 3 cuts. 
            {
                subsets = new List<List<string>>();

                foreach (var vertex in connections.Keys)
                    subsets.Add(new List<string>() { vertex });

                int i;
                List<string> subset1, subset2;

                while (subsets.Count > 2)
                {
                    i = new Random().Next() % edges.Count;      // Karger algorithm does not guarantee a min, but a cut. We randomly choose nodes

                    subset1 = subsets.Where(s => s.Contains(edges[i].v1)).First();
                    subset2 = subsets.Where(s => s.Contains(edges[i].v2)).First();

                    if (subset1 == subset2) continue;

                    subsets.Remove(subset2);
                    subset1.AddRange(subset2);
                }

                numCuts = HowManyCuts(subsets, edges);
            } 
            return subsets[0].Count * subsets[1].Count;
        }

        public void ParseInput(List<string> lines)
            => lines.ForEach(ParseLine);

        public int Solve(int part)
            => Karger3Cuts();
    }
}
