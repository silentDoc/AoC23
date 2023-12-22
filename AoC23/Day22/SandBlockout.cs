using AoC23.Common;

namespace AoC23.Day22
{
    record SandBrick(int x1, int y1, int z1, int x2, int y2, int z2)
    {
        public List<SandBrick> Above { get; set; } = new();
        public List<SandBrick> Below { get; set; } = new();

        public int[] Xs => Enumerable.Range(x1, x2 - x1 + 1).ToArray();
        public int[] Ys => Enumerable.Range(y1, y2 - y1 + 1).ToArray();
        public int[] Zs => Enumerable.Range(z1, z2 - z1 + 1).ToArray();
    }

    internal class SandBlockout
    {
        List<SandBrick> Bricks = new();
        public void ParseInput(List<string> lines)
        {
            foreach (var line in lines)
            {
                var vs = line.Split('~', ',').Select(int.Parse).ToArray();
                var brick = new SandBrick(vs[0], vs[1], vs[2], vs[3], vs[4], vs[5]);
                Bricks.Add(brick);
            }

            Bricks = Bricks.OrderBy(b => b.z1).ToList();
        }

        int Drop()
        {
            for (var i = 0; i < Bricks.Count; i++)
            {
                var brick = Bricks[i];
                var z = brick.z1;

                while (z > 0)
                {
                    var supportingBricks = Bricks.Where(b =>
                            b.z2 == z - 1 &&
                            brick.Xs.Intersect(b.Xs).Any() &&
                            brick.Ys.Intersect(b.Ys).Any())
                        .ToList();

                    if (supportingBricks.Count > 0 || z == 1)
                    {
                        foreach (var b in supportingBricks)
                        {
                            b.Above.Add(brick);
                            brick.Below.Add(b);
                        }
                        Bricks[i] = brick;
                        break;
                    }

                    z--;
                    brick = brick with { z1 = z, z2 = z + brick.Zs.Length - 1 };
                }
            }
            return Bricks.Count(b => b.Above.All(a => a.Below.Count > 1));
        }

        public int Solve(int part = 1)
            => Drop();
    }
}

