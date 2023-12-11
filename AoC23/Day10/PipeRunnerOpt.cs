using AoC23.Common;

namespace AoC23.Day10
{
    public class PipeRunnerOpt
    {
        readonly Coord2D NORTH = new Coord2D(0, -1);
        readonly Coord2D SOUTH = new Coord2D(0, 1);
        readonly Coord2D EAST  = new Coord2D(1 , 0);
        readonly Coord2D WEST  = new Coord2D(-1, 0);

        const int DIST_MAX = 999999;

        Dictionary<Coord2D, PipePosition> Map = new();
        Coord2D StartPos = new(0,0);

        List<Coord2D> ConnectedPositions(char pipeSymbol)
            => pipeSymbol switch
            {
                '|' => new List<Coord2D> { NORTH, SOUTH },
                '-' => new List<Coord2D> { EAST , WEST },
                'L' => new List<Coord2D> { NORTH, EAST },
                'J' => new List<Coord2D> { NORTH, WEST },
                'F' => new List<Coord2D> { SOUTH, EAST },
                '7' => new List<Coord2D> { SOUTH, WEST },
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

        // Resolves which pipe shape is the "S" on the map
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
            var adjacents = Map.Keys.Where(x => StartPos.GetNeighbors().Contains(x));
            var connected = adjacents.Where(a => Map[a].adjacentPipes.Contains(StartPos)).ToList();

            var bWest = connected.Any(x => x + EAST == StartPos);
            var bEast = connected.Any(x => x + WEST == StartPos);
            var bNorth = connected.Any(x => x + SOUTH == StartPos);
            var bSouth = connected.Any(x => x + NORTH == StartPos);

            var startSym = FindStartSymbol((bEast, bWest, bNorth, bSouth));
            var adjacentDirs = ConnectedPositions(startSym);
            var adjacentPositions = adjacentDirs.Select(x => x + StartPos).ToList();
            Map[StartPos] = new PipePosition(StartPos, startSym, adjacentPositions);
            Map[StartPos].distance = 0;
        }

        void TraverseMazeIter(List<Coord2D> pipes, int startCost)
        {
            int cost = startCost;
            var pipesIter = ProcessAdjacentsInLoop(pipes, cost);

            while (pipesIter.Count > 0)
            {
                cost++;
                pipesIter = ProcessAdjacentsInLoop(pipesIter, cost);
            }
        }

        List<Coord2D> ProcessAdjacentsInLoop(List<Coord2D> pipes, int newCost)
        {
            var adjacentSet = pipes.SelectMany(x => Map[x].adjacentPipes).Where(p => Map[p].distance == DIST_MAX).ToList();
            foreach (var pipe in adjacentSet)
                Map[pipe].distance = newCost;   // this if should not be necessary

            return adjacentSet;
        }

        // Crossing counters for Part 2
        int CountHCrossings(Coord2D pos, int start, int end)
        {
            int count = 0;
            bool inHLine = false;
            char enterChar = ' ';
            string turns = "7JLF";

            for (int i = start; i <= end; i++)
            {
                Coord2D current = (i, pos.y);

                if (Map[current].distance == DIST_MAX)
                    continue;
                
                var sym = Map[current].Symbol;
                if (!inHLine && sym == '|')
                    count++;

                if (inHLine && sym == '-')
                    continue;

                if (!inHLine && turns.Contains(sym))
                {
                    enterChar = sym;
                    inHLine = true;
                    continue;
                }

                if (inHLine && turns.Contains(sym))
                {
                    if( (enterChar == 'L' && sym == '7') || (enterChar == 'F' && sym == 'J') ||
                        (enterChar == '7' && sym == 'L') || (enterChar == 'J' && sym == 'F')   )
                        count++;
                    inHLine = false;
                }
            }
            return count;
        }

        int CountVCrossings(Coord2D pos, int start, int end)
        {
            int count = 0;
            bool inVLine = false;
            char enterChar = ' ';
            string turns = "7JLF";

            for (int i = start; i <= end; i++)
            {
                Coord2D current = (pos.x, i);

                if (Map[current].distance == DIST_MAX)
                    continue;

                var sym = Map[current].Symbol;
                if (!inVLine && sym == '-')
                    count++;

                if (inVLine && sym == '|')
                    continue;

                if (!inVLine && turns.Contains(sym))
                {
                    enterChar = sym;
                    inVLine = true;
                    continue;
                }

                if (inVLine && turns.Contains(sym))
                {
                    if ( (enterChar == '7' && sym == 'L') || (enterChar == 'J' && sym == 'F') ||
                         (enterChar == 'L' && sym == '7') || (enterChar == 'F' && sym == 'J')    )
                        count++;

                    inVLine = false;
                }
            }
            return count;
        }

        bool IsClosedPosition(Coord2D pos, int MaxX, int MaxY)
        {
            if ( pos.x == 0 || pos.y == 0 || pos.x == MaxX || pos.y == MaxY)
                return false;

            List<int> crossings = new() { CountHCrossings(pos, 0, pos.x - 1) , CountHCrossings(pos, pos.x + 1, MaxX) ,
                                          CountVCrossings(pos, 0, pos.y - 1) , CountVCrossings(pos, pos.y + 1, MaxY)};
            return !crossings.Any(p => p % 2 == 0);
        }
        
        int SolvePart1()
        {
            TraverseMazeIter(new List<Coord2D> { StartPos }, 1);
            return Map.Values.Where(x => x.distance < DIST_MAX).Max(x => x.distance);
        }

        int SolvePart2()
        {
            TraverseMazeIter(new List<Coord2D> { StartPos }, 1);    // Find the originalMap solution
            int MaxX = Map.Keys.Max(k => k.x);
            int MaxY = Map.Keys.Max(k => k.y);

            var notInLoopPositions = Map.Keys.Where(x => Map[x].distance == DIST_MAX).ToList();

            return notInLoopPositions.Where(p => IsClosedPosition(p, MaxX, MaxY)).Count();
        }

        public int Solve(int part = 1)
            => part ==1 ? SolvePart1() : SolvePart2();
    }
}
