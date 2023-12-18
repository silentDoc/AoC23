using AoC23.Common;

namespace AoC23.Day18
{
    record Instruction
    {
        public Coord2D Direction;
        public int Steps;
        public string Color;

        public void TransformPart2()
        {
            Steps = int.Parse(Color.Substring(0,5), System.Globalization.NumberStyles.HexNumber);
            Direction = Color.Last() switch
            {
                '0' => (1, 0),
                '1' => (0, 1),
                '2' => (-1, 0),
                '3' => (0, -1),
                _ => throw new Exception("Invalid direction char: " + Color.Last().ToString() )
            };
        }
    }

    internal class LavaDigger
    {
        Coord2D UP = (0, -1);
        Coord2D DOWN = (0, 1);
        Coord2D RIGHT = (1, 0);
        Coord2D LEFT = (-1, 0);

        List<Instruction> Instructions = new();
        List<Coord2D> Map = new();

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

        public long SolvePart1()
        {
            BuildMap();
            Map = Map.Distinct().ToList();  // The last position is the start position
            FillMap();
            return Map.Count();
        }

        public long SolvePart2()
        {
            foreach (var ins in Instructions)
                ins.TransformPart2();
            
            Coord2D current = (0, 0);
            List<Coord2D> vertices = new() { current };

            foreach (var ins in Instructions)
            {
                current += ins.Direction * ins.Steps;
                vertices.Add(current);
            }

            // Calculate the shoelace formula https://en.wikipedia.org/wiki/Shoelace_formula
            long shoelaceAreaSum = 0;
            long shoelaceAreaSub = 0;
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                shoelaceAreaSum += ((long) vertices[i].x) * ((long) vertices[i + 1].y);
                shoelaceAreaSub += ((long) vertices[i + 1].x) * ((long) vertices[i].y);
            }

            shoelaceAreaSum += ((long)vertices[vertices.Count - 1].x) * ((long)vertices[0].y);
            shoelaceAreaSub += ((long)vertices[0].x) * ((long)vertices[vertices.Count - 1].y);

            long shoeLaceArea = Math.Abs(shoelaceAreaSum - shoelaceAreaSub) / 2;

            // Now add perimeter. Pick's theorem : https://en.wikipedia.org/wiki/Pick%27s_theorem
            long perimeter = Instructions.Sum(x => x.Steps); // Start = end
            long Area = shoeLaceArea + (perimeter / 2) - 1;

            return Area;
        }

        public void ParseInput(List<string> lines)
            => lines.ForEach(x => Instructions.Add(ParseLine(x)) );

        public long Solve(int part)
            => part == 1 ? SolvePart1() : SolvePart2();
    }
}
