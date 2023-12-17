using AoC23.Common;

namespace AoC23.Day16
{
    public record Beam
    {
        public Coord2D Pos;
        public Coord2D Dir;
    }

    internal class MirrorSystem
    {
        Dictionary<Coord2D, char> Map = new();
        HashSet<Coord2D> Energized = new();
        HashSet<(Coord2D, Coord2D)> AlreadyProcessed = new();

        Coord2D UP     = (0, -1);
        Coord2D DOWN   = (0,  1);
        Coord2D RIGHT  = (1,  0);
        Coord2D LEFT   = (-1, 0);

        List<Beam> BeamList = new List<Beam>();
        List<Beam> NextBeamList = new();

        void ParseLine(string line, int row)
        {
            for (int j = 0; j < line.Length; j++)
                Map[(j, row)] = line[j];
        }

        public void ParseInput(List<string> lines)
        { 
            for(int i=0; i<lines.Count; i++)
                ParseLine(lines[i],i);
        }

        void MoveBeam(Beam beam)
        {
            if (!Map.Keys.Contains(beam.Pos))   // End of the beam, out of map
                return;

            if (!AlreadyProcessed.Add((beam.Pos, beam.Dir)))   // We already processed a beam with this pos + dir
                return;

            Energized.Add(beam.Pos);

            // Splits first
            if (Map[beam.Pos] == '-' && (beam.Dir == DOWN || beam.Dir == UP))
            {
                Beam split1 = new Beam() { Pos = beam.Pos + LEFT, Dir = LEFT };
                Beam split2 = new Beam() { Pos = beam.Pos + RIGHT, Dir = RIGHT };
                NextBeamList.Add(split1);
                NextBeamList.Add(split2);
                return;
            }

            if (Map[beam.Pos] == '|' && (beam.Dir == LEFT || beam.Dir == RIGHT))
            {
                Beam split1 = new Beam() { Pos = beam.Pos + UP, Dir = UP };
                Beam split2 = new Beam() { Pos = beam.Pos + DOWN, Dir = DOWN };
                NextBeamList.Add(split1);
                NextBeamList.Add(split2);
                return;
            }

            beam.Dir = (beam.Dir.x, beam.Dir.y, Map[beam.Pos]) switch
            {
                (1, 0, '/') => UP,
                (-1, 0, '/') => DOWN,
                (0, 1, '/') => LEFT,
                (0, -1, '/') => RIGHT,
                (1, 0, '\\') => DOWN,
                (-1, 0, '\\') => UP,
                (0, 1, '\\') => RIGHT,
                (0, -1, '\\') => LEFT,
                (_ ,_ , _ ) => beam.Dir
            };
            beam.Pos += beam.Dir;
            NextBeamList.Add(beam);
        }

        int SolvePart2()
        {
            HashSet<int> Entries = new();

            var maxX = Map.Keys.Max(k => k.x);
            var maxY = Map.Keys.Max(k => k.y);

            // Corners
            Entries.Add(Energize((0, 0), DOWN));
            Entries.Add(Energize((0, 0), RIGHT));
            Entries.Add(Energize((maxX, 0), DOWN));
            Entries.Add(Energize((maxX, 0), LEFT));

            Entries.Add(Energize((0, maxY), UP));
            Entries.Add(Energize((0, maxY), RIGHT));
            Entries.Add(Energize((maxX, maxY), UP));
            Entries.Add(Energize((maxX, maxY), LEFT));

            // Edges
            for (int i = 1; i < maxX; i++)
            {
                Entries.Add(Energize((i, 0), DOWN));
                Entries.Add(Energize((i, maxY), UP));
            }

            for (int i = 1; i < maxY; i++)
            {
                Entries.Add(Energize((0, i), RIGHT));
                Entries.Add(Energize((maxX, i), LEFT));
            }

            return Entries.Max();
        }

        int Energize(Coord2D startPos, Coord2D startDir)
        {
            Beam start = new Beam() { Pos = startPos, Dir = startDir };

            BeamList = new() { start};
            //List<int> energized = new();
            Energized.Clear();
            AlreadyProcessed.Clear();

            while (BeamList.Count>0)
            {
                foreach (var beam in BeamList)
                    MoveBeam(beam);
                BeamList = NextBeamList;
                NextBeamList = new();
              //  energized.Add(Energized.Count());
            }
            return Energized.Count();
        }

        public int Solve(int part)
            => part == 1 ? Energize((0,0), RIGHT) : SolvePart2();
    }
}
