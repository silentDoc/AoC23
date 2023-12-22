using AoC23.Common;

namespace AoC23.Day22
{
    record SandBrick
    {
        public int Id = 0;
        public Coord3D Shape = (0, 0, 0);
        public Coord3D Position = (0, 0, 0);

        public List<Coord3D> CubeSpan()
        {
            List<Coord3D> res = new();

            if (Shape.x != 0)
                for (int i = 0; i <= Shape.x; i++)
                    res.Add(Position + (i, 0, 0));
            if (Shape.y != 0)
                for (int i = 0; i <= Shape.y; i++)
                    res.Add(Position + (0, i, 0));
            if (Shape.z != 0)
                for (int i = 0; i <= Shape.z; i++)
                    res.Add(Position + (0, 0, i));

            return res;
        }

        public int MaxZLevel
            => Position.z + Shape.z;

        public int MinZLevel
           => Position.z;

        public void Drop(int nz = -1)
            => Position.z = nz == -1 ? Position.z-1 : nz;

        public void Up()
            => Position.z++;

        public List<SandBrick> SupportedBy(List<SandBrick> allBricks)
        {
            var possibleSupporters = allBricks.Where(x => x.Id != Id && x.MaxZLevel == MinZLevel-1).ToList();
            var res = new List<SandBrick>();

            foreach (var brick in possibleSupporters)
            {
                var checkSpan = brick.CubeSpan().Where(c => c.z == MinZLevel - 1).ToList();
                foreach (var cube in checkSpan) // We put the lower row in the min level of our brick
                    cube.z++;

                if (CubeSpan().Intersect(checkSpan).Any())
                    res.Add(brick);
            }

            return res;
        }

        public List<SandBrick> Supports(List<SandBrick> allBricks)
        {
            var possibleSupported = allBricks.Where(x => x.Id != Id && MaxZLevel == x.MinZLevel - 1).ToList();
            var res = new List<SandBrick>();

            foreach (var brick in possibleSupported)
            {
                var checkSpan = brick.CubeSpan().Where(c => c.z == MaxZLevel + 1).ToList();
                foreach (var cube in checkSpan) // We put the lower row in the min level of our brick
                    cube.z--;

                if (CubeSpan().Intersect(checkSpan).Any())
                    res.Add(brick);
            }

            return res;
        }
    }

    internal class SandBlockout
    {
        List<SandBrick> Bricks = new();
        List<SandBrick> Dropped = new();

        int idCount = 0;

        private void ParseLine(string line)
        {
            var parts = line.Split('~', StringSplitOptions.TrimEntries);
            SandBrick brick = new();
            var pos = parts[0].Split(',').Select(int.Parse).ToList();
            var shape = parts[1].Split(',').Select(int.Parse).ToList();
            Coord3D posC = (pos[0], pos[1], pos[2]);
            Coord3D shapeC = (shape[0], shape[1], shape[2]) - posC;
            brick.Position = posC;
            brick.Shape = shapeC;
            brick.Id = idCount++;
            Bricks.Add(brick);
        }

        public void ParseInput(List<string> lines)
            => lines.ForEach(ParseLine);

        void DropBricks()
        { 
            // We will move first the ones closer to the groud
            var sortedBricks = Bricks.OrderBy(x => x.Position.z).ThenBy(p => p.Position.x).ThenBy(p => p.Position.y).ToList();

            foreach (var brick in sortedBricks) 
            {
                var otherBricks = Dropped.Where(x => x.Id != brick.Id);  // Leave it to iEnumerable for faster access.
                var canDrop = true;
                while (canDrop)
                {
                    var checks = otherBricks.Where(b => b.MinZLevel >= brick.MinZLevel - 1 && b.MinZLevel <= brick.MaxZLevel - 1).ToList();
                    var checkSpan = checks.SelectMany(x => x.CubeSpan()).ToList();

                    brick.Drop();
                    if( brick.CubeSpan().Intersect(checkSpan).Any() || brick.Position.z <1)
                    {
                        brick.Up();
                        canDrop = false;
                    }
                }
                Dropped.Add(brick);
            }
        }

        int CountRemoveBricks()
        {
            int count = 0;
            foreach (var brick in Dropped)
            {
                var supportedByMe = brick.Supports(Dropped);

                // If a brick does not support any other brick, we get rid of it
                if (supportedByMe.Count() == 0)
                {
                    count++;
                    continue;
                }

                // The brick supports some other brick(s)
                var amICritical = supportedByMe.Select(x => x.SupportedBy(Dropped).Count()).Any(x => x ==1);
                if (!amICritical)
                    count++;
            }
            return count;
        }

        int SolvePart1()
        {
            DropBricks();
            return CountRemoveBricks();
        }

        public int Solve(int part = 1)
            => SolvePart1();
    }
}
