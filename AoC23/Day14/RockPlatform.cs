using System.Text;

namespace AoC23.Day14
{
    internal class RockPlatform
    {
        char[][] Rocks = null;
        int Rows = 0;
        int Columns = 0;

        void ParseLine(string line, int pos)
            => Rocks[pos] = line.ToCharArray();

        public void ParseInput(List<string> lines)
        { 
            Rocks = new char[lines.Count][];
            foreach (var item in Enumerable.Range(0, lines.Count))
                ParseLine(lines[item], item);
            Rows = lines.Count;
            Columns = lines[0].Length;
        }

        void TiltNorth()
        {
            for (int row = 1; row < Rows; row++)
                for (int column = 0; column < Columns; column++)
                {
                    if (Rocks[row][column] != 'O')
                        continue;
                    
                    int newRow = row - 1;
                    while (newRow >= 0 && Rocks[newRow][column] == '.')
                        newRow--;
                    newRow++;
                    Rocks[row][column] = '.';
                    Rocks[newRow][column] = 'O';
                }
        }

        void TiltSouth()
        {
            for (int row = Rows-2; row >=0; row--)
                for (int column = 0; column < Columns; column++)
                {
                    if (Rocks[row][column] != 'O')
                        continue;

                    int newRow = row + 1;
                    while (newRow < Rows && Rocks[newRow][column] == '.')
                        newRow++;
                    newRow--;
                    Rocks[row][column] = '.';
                    Rocks[newRow][column] = 'O';
                }
        }

        void TiltEast()
        {
            for (int column = Columns-2; column >= 0; column--)
                for (int row =0; row < Rows; row++)
                {
                    if (Rocks[row][column] != 'O')
                        continue;

                    int newColumn = column + 1;
                    while (newColumn < Columns && Rocks[row][newColumn] == '.')
                        newColumn++;
                    newColumn--;
                    Rocks[row][column] = '.';
                    Rocks[row][newColumn] = 'O';
                }
        }

        void TiltWest()
        {
            for (int column = 1; column < Columns; column++)
                for (int row = 0; row < Rows; row++)
                {
                    if (Rocks[row][column] != 'O')
                        continue;

                    int newColumn = column - 1;
                    while (newColumn >=0 && Rocks[row][newColumn] == '.')
                        newColumn--;
                    newColumn++;
                    Rocks[row][column] = '.';
                    Rocks[row][newColumn] = 'O';
                }
        }

        void Cycle()
        {
            TiltNorth();
            TiltWest();
            TiltSouth();
            TiltEast();
        }

        int GetTotalLoad()
        {
            int totalLoad = 0;
            for (int row = 0; row < Rows; row++)
                for (int column = 0; column < Rows; column++)
                    if (Rocks[row][column] == 'O')
                        totalLoad += (Rows - row);
            return totalLoad;
        }

        string GetDishSignature()
        {
            StringBuilder sb = new();
            for (int i = 0; i < Rows; i++)
                sb.Append(new string(Rocks[i]));
            return sb.ToString();
        }

        int SolvePart1()
        {
            TiltNorth();
            return GetTotalLoad();
        }

        int SolvePart2()
        {
            long numCycles = 1000000000;
            Dictionary<long, string> seen = new Dictionary<long, string>();
            long cycle = 0;
            string signature = "";
            for (cycle = 1; cycle <= numCycles; cycle++)
            {
                Cycle();
                signature = GetDishSignature();
                if (seen.Values.Contains(signature))
                    break;
                seen[cycle] = signature;

            }

            var firstSeen = seen.Keys.First(x => seen[x] == signature);
            var loopLength = cycle - firstSeen;
            var loopRepeats = (numCycles-firstSeen) / loopLength;
            var remaining = numCycles - (firstSeen + loopRepeats * loopLength);

            for (var i = 0; i < remaining; i++)
                Cycle();

            return GetTotalLoad();
        }

        public int Solve(int part = 1)
            => part == 1 ? SolvePart1() : SolvePart2();
    }
}
