using AoC23.Common;

namespace AoC23.Day18
{
    record Instruction
    {
        public Coord2D Direction;
        public int Steps;
        public string Color;
    }

    internal class LavaDigger
    {
        Coord2D UP = (0, -1);
        Coord2D DOWN = (0, 1);
        Coord2D RIGHT = (1, 0);
        Coord2D LEFT = (-1, 0);

        List<Instruction> Instructions = new();
        
        HashSet<Coord2D> Map = new();

        Instruction ParseLine(string line)
        {
            var groups = line.Split(' ');
            Instruction ins = new();
            ins.Direction = groups[0] switch
            {
                "U" => UP,
                "D" => DOWN,
                "L" => LEFT,
                "R" => RIGHT,
                "_" => throw new Exception("Invalid direction")
            };

            ins.Steps = int.Parse(groups[1]);
            ins.Color = groups[2].Replace("(#", "").Replace(")", "");
            return ins;
        }

        void BuildMap()
        {
            Coord2D current = (0, 0);
            foreach (var ins in Instructions)
            {
                for (int i = 0; i < ins.Steps; i++)
                {
                    Map.Add(current);
                    current += ins.Direction;
                }
            }
            Map.Add(current);
        }

        void FillMap()
        {
            HashSet<Coord2D> filled = new();
            Queue<Coord2D> active = new();

            var miny = Map.Min(p => p.y);
            var maxy = Map.Max(p => p.y);
            var minx = Map.Min(p => p.x);
            var maxx = Map.Max(p => p.x);

            active.Enqueue(((maxx + minx) / 2, (maxy + miny) / 2));

            while (active.Count > 0)
            {
                var current = active.Dequeue();

                var neighs = current.GetNeighbors().Where(n => !Map.Contains(n)).ToList();
                neighs = neighs.Where(n => n.x >= minx && n.x <= maxx && n.y >= miny && n.y <= maxy).ToList();

                filled.Add(current);
                foreach (var n in neighs)
                    if (!active.Contains(n) && !filled.Contains(n))
                    {
                        filled.Add(n);
                        active.Enqueue(n);
                    }
            }

            foreach (var f in filled)
                Map.Add(f);
        }

        public int SolvePart1()
        {
            BuildMap();
            FillMap();
            return Map.Count();
        }

        public void ParseInput(List<string> lines)
            => lines.ForEach(x => Instructions.Add(ParseLine(x)) );

        public int Solve(int part)
            => SolvePart1();
    }
}
