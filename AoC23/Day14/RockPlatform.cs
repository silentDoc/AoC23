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
                for (int column = 0; column < Rows; column++)
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

        int GetTotalLoad()
        {
            int totalLoad = 0;
            for (int row = 0; row < Rows; row++)
                for (int column = 0; column < Rows; column++)
                    if (Rocks[row][column] == 'O')
                        totalLoad += (Rows - row);
            return totalLoad;
        }

        int SolvePart1()
        {
            TiltNorth();
            return GetTotalLoad();
        }

        public int Solve(int part = 1)
            => SolvePart1();
    }
}
