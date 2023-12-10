using AoC23.Common;

namespace AoC23.Day10
{
    class PipePosition
    {
        public Coord2D position;
        public List<Coord2D> adjacentPipes;
        public int distance = 999999;
        public char Symbol = '.';

        public PipePosition(Coord2D position, char symbol, List<Coord2D> adjacentPipes)
        {
            this.position = position;
            this.adjacentPipes = adjacentPipes;
            this.Symbol = symbol;
        }
    }

    public class PipeRunner
    {
        Coord2D posNorth = new Coord2D(0, -1);
        Coord2D posSouth = new Coord2D(0, 1);
        Coord2D posEast = new Coord2D(1 , 0);
        Coord2D posWest = new Coord2D(-1, 0);

        Dictionary<Coord2D, PipePosition> Map = new();
        Coord2D StartPos = new(0,0);


        List<Coord2D> ConnectedPositions(char pipeSymbol)
            => pipeSymbol switch
            {
                '|' => new List<Coord2D> { posNorth, posSouth },
                '-' => new List<Coord2D> { posEast, posWest },
                'L' => new List<Coord2D> { posNorth, posEast },
                'J' => new List<Coord2D> { posNorth, posWest },
                'F' => new List<Coord2D> { posSouth, posEast },
                '7' => new List<Coord2D> { posSouth, posWest },
                '.' => new List<Coord2D> { },
                'S' => new List<Coord2D> { },
                '_' => throw new Exception("Invalid symbol")
            };

        void ParseLine(string line, int row)
        {
            for(int col = 0; col<line.Length; col++) 
            {
                Coord2D currentPos = (col, row);
                char symbol = line[col];
                var adjacentDirs = ConnectedPositions(symbol);
                var adjacentPositions = adjacentDirs.Select(x => x + currentPos).ToList();

                Map[currentPos] = new PipePosition(currentPos, symbol, adjacentPositions);
                if (symbol == 'S')
                    StartPos = currentPos;
            }
        }


        Char FindStartSymbol((bool east, bool west, bool north, bool south) connections)
            => connections switch
            {
                (true, true, false, false) => '-',
                (false, false, true, true) => '|',
                (true, false, true, false) => 'L',
                (false, true, true, false) => 'J',
                (true, false, false, true) => 'F',
                (false, true, false, true) => '7',
                (_,_,_,_) => throw new Exception("wtf"), 
            };

        public void ParseInput(List<string> lines)
        {
            for (var row = 0; row < lines.Count; row++)
                ParseLine(lines[row], row);

            // Resolve the symbol of the start Pos
            var adjacents = Map.Keys.Where( x => StartPos.GetNeighbors().Contains(x));
            var connected = adjacents.Where(a => Map[a].adjacentPipes.Contains(StartPos)).ToList();

            var bWest = connected.Any(x => x + posEast == StartPos);
            var bEast = connected.Any(x => x + posWest == StartPos);
            var bNorth = connected.Any(x => x + posSouth == StartPos);
            var bSouth = connected.Any(x => x + posNorth == StartPos);

            var startSym = FindStartSymbol((bEast, bWest, bNorth, bSouth));
            var adjacentDirs = ConnectedPositions(startSym);
            var adjacentPositions = adjacentDirs.Select(x => x + StartPos).ToList();
            Map[StartPos] = new PipePosition(StartPos, startSym, adjacentPositions);
            Map[StartPos].distance = 0;
        }


        void TraverseMaze(List<Coord2D> pipes, int newCost)
        {
            if (pipes.Count == 0)
                return;

            var adjacentSet = pipes.SelectMany(x => Map[x].adjacentPipes).Where(p => Map[p].distance == 999999).ToList();

            foreach (var pipe in adjacentSet)
                if(Map[pipe].distance > newCost)
                    Map[pipe].distance = newCost;   // this if should not be necessary

            TraverseMaze(adjacentSet, newCost + 1);
        }

        void PrintMap()
        {
            int rows = Map.Keys.Max(p => p.y);
            int cols = Map.Keys.Max(p => p.x);

            for (int r = 0; r <= rows; r++)
            {
                for (int c = 0; c <= cols; c++)
                {
                    Coord2D key = (c, r);
                    Console.Write(Map[key].ToString());
                }
                Console.WriteLine();
            }
        }

        void PrintDist()
        {
            int rows = Map.Keys.Max(p => p.y);
            int cols = Map.Keys.Max(p => p.x);

            for (int r = 0; r <= rows; r++)
            {
                for (int c = 0; c <= cols; c++)
                {
                    Coord2D key = (c, r);
                    if(Map[key].distance< 999999)
                        Console.Write(Map[key].distance.ToString());
                    else
                        Console.Write(".");
                }
                Console.WriteLine();
            }
        }

        int SolvePart1()
        {
            TraverseMaze(new List<Coord2D> { StartPos }, 1);

            PrintMap();
            PrintDist();

            return Map.Values.Where(x => x.distance < 999999).Max(x => x.distance);
        }

        public int Solve(int part = 1)
            => SolvePart1();
    }
}
