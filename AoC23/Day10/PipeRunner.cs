using AoC23.Common;
using System.Text;

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

        public void ParseInput(List<string> lines, bool ProcessStart = true)
        {
            for (var row = 0; row < lines.Count; row++)
                ParseLine(lines[row], row);

            if (ProcessStart)   // NOT Needed for Part 2, when we expand the grid
            {
                // Resolve the symbol of the start Pos
                var adjacents = Map.Keys.Where(x => StartPos.GetNeighbors().Contains(x));
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
            var adjacentSet = pipes.SelectMany(x => Map[x].adjacentPipes).Where(p => Map[p].distance == 999999).ToList();
            foreach (var pipe in adjacentSet)
                Map[pipe].distance = newCost;   // this if should not be necessary

            return adjacentSet;
        }
                

        void TraverseOpenPositionsIter(List<Coord2D> pipes)
        {
            var pipesIter = ProcessAdjacentsOpen(pipes);
            while (pipesIter.Count > 0)
                pipesIter = ProcessAdjacentsOpen(pipesIter);
        }

        List<Coord2D> ProcessAdjacentsOpen(List<Coord2D> openPositions)
        {
            List<Coord2D> retVal = new();
            var maxX = Map.Keys.Max(p => p.x);
            var maxY = Map.Keys.Max(p => p.y);

            // Using this foreach is much faster than SelectMany
            foreach (var pos in openPositions)
            {
                // Using MaxX MaxY is much faster than Map.Keys.Contains
                var adjacentSet = pos.GetNeighbors().Where(n => n.x >= 0 && n.x <= maxX && n.y >= 0 && n.y <= maxY)
                                                    .Where(p => Map[p].distance == 999999).ToList();
                foreach (var pipe in adjacentSet)
                {
                    Map[pipe].distance = -1;
                    retVal.Add(pipe);
                }
            }

            return retVal;
        }
        
        int SolvePart1()
        {
            TraverseMazeIter(new List<Coord2D> { StartPos }, 1);
            return Map.Values.Where(x => x.distance < 999999).Max(x => x.distance);
        }

        // Where the magic happens - this ZOOMS IN the original input to be able to see open position connection between pipes
        void ExpandGrid()
        {
            List<string> newInput = new();

            int rows = Map.Keys.Max(p => p.y);
            int cols = Map.Keys.Max(p => p.x);

            for (int r = 0; r <= rows; r++)
            {
                StringBuilder l1 = new("");
                StringBuilder l2 = new("");
                StringBuilder l3 = new("");

                for (int c = 0; c <= cols; c++)
                {
                    var pipe = Map[(c, r)];

                    if (pipe.Symbol == '.')
                    {
                        l1.Append("...");
                        l2.Append("...");
                        l3.Append("...");
                    }
                    if (pipe.Symbol == '-')
                    {
                        l1.Append("...");
                        l2.Append("---");
                        l3.Append("...");
                    }
                    if (pipe.Symbol == '|')
                    {
                        l1.Append(".|.");
                        l2.Append(".|.");
                        l3.Append(".|.");
                    }
                    if (pipe.Symbol == 'F')
                    {
                        l1.Append("...");
                        l2.Append(".F-");
                        l3.Append(".|.");
                    }
                    if (pipe.Symbol == '7')
                    {
                        l1.Append("...");
                        l2.Append("-7.");
                        l3.Append(".|.");
                    }
                    if (pipe.Symbol == 'L')
                    {
                        l1.Append(".|.");
                        l2.Append(".L-");
                        l3.Append("...");
                    }
                    if (pipe.Symbol == 'J')
                    {
                        l1.Append(".|.");
                        l2.Append("-J.");
                        l3.Append("...");
                    }
                }
                newInput.Add(l1.ToString());
                newInput.Add(l2.ToString());
                newInput.Add(l3.ToString());
            }

            StartPos = (StartPos.x * 3 +1, StartPos.y * 3+1);
            Map.Clear();
            // Reinterpret the problem with the zoomed in map
            ParseInput(newInput, false);
            Map[StartPos].distance = 0;
        }

        int SolvePart2()
        {
            TraverseMazeIter(new List<Coord2D> { StartPos }, 1);    // Find the originalMap solution

            // Keep a copy of the solved original map
            var originalMap = new Dictionary<Coord2D, PipePosition>();
            foreach (var k in Map.Keys)
                originalMap[k] = Map[k];
            
            // Zoom in
            ExpandGrid(); 

            // Find the solution of zoomed map
            TraverseMazeIter(new List<Coord2D> { StartPos }, 1);

            // To find all the positions that are closed, we find the ones that are open
            // to do so, we start from the positions that are in the edge and do not belong to the loop (distance = Max)
            List<Coord2D> openPosSeed = new();
            int rows = Map.Keys.Max(p => p.y);
            int cols = Map.Keys.Max(p => p.x);

            for (int r = 0; r <= rows; r++)
            {
                if (Map[(0, r)].distance == 999999)
                {
                    Map[(0, r)].distance = -1;
                    openPosSeed.Add((0, r));
                }
                if (Map[(cols, r)].distance == 999999)
                {
                    Map[(cols, r)].distance = -1;
                    openPosSeed.Add((cols, r));
                }
            }

            for (int c = 0; c <= cols; c++)
            {
                if (Map[(c, rows)].distance == 999999)
                {
                    Map[(c, rows)].distance = -1;
                    openPosSeed.Add((c, rows));
                }
                if (Map[(c, 0)].distance == 999999)
                {
                    Map[(c, 0)].distance = -1;
                    openPosSeed.Add((c, rows));
                }
            }

            // Now we find all the connected positions that are not in the loop
            // starting from that seed
            TraverseOpenPositionsIter(openPosSeed);

            // We get back to the original map , we will check if each non loop position maps to an open position in the zoomed map
            var originalNotBelong = originalMap.Keys.Where(x => originalMap[x].distance == 999999).ToList();
            int countClosed = 0;
            foreach (var k in originalNotBelong)
            {
                Coord2D expGridKey = (k.x * 3 + 1, k.y * 3 + 1);
                if (Map[expGridKey].distance != -1)
                    countClosed++;
            }

            return countClosed;
        }

        public int Solve(int part = 1)
            => part ==1 ? SolvePart1() : SolvePart2();
    }
}
