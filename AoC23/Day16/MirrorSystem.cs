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

            Energized.Add(beam.Pos);

            if (Map[beam.Pos] == '.' || 
               (Map[beam.Pos] == '-' && (beam.Dir == RIGHT || beam.Dir == LEFT)) ||
               (Map[beam.Pos] == '|' && (beam.Dir == UP || beam.Dir == DOWN)))
            {
                beam.Pos += beam.Dir;
                NextBeamList.Add(beam);
                return;
            }

            if (Map[beam.Pos] == '/' || Map[beam.Pos] == '\\')
            {
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
                    (_, _, _) => throw new Exception("Invalid tuple resolving turn")
                };
                beam.Pos += beam.Dir;
                NextBeamList.Add(beam);
                return;
            }

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
        }

        int SolvePart1()
        {
            Beam start = new Beam() { Pos = (0, 0), Dir = RIGHT };
            BeamList.Add(start);

            List<int> energized = new();

            while (BeamList.Count>0)
            {
                foreach (var beam in BeamList)
                    MoveBeam(beam);
                BeamList = NextBeamList;
                NextBeamList = new();

                energized.Add(Energized.Count());
                
                // This below is probably the ugliest and nastiest end contition I ever coded
                var jumpOut = false;
                if (energized.Count > 100)
                {
                    jumpOut = true;
                    for (int i = energized.Count - 50; i < energized.Count; i++)
                        if (energized[i] != energized[i - 1])
                            jumpOut = false;
                }

                if (jumpOut)
                    break;
            }

            return Energized.Count();
        }

        public int Solve(int part)
            => part == 1 ? SolvePart1() : 0;
    }
}
