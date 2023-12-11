using AoC23.Common;

namespace AoC23.Day11
{
    internal class GalaxyTraveler
    {
        List<Coord2D> Galaxies = new();

        public void ParseInput(List<string> lines, int part =1)
        {
            char[][] input = new char[lines.Count][];
            for (int i = 0; i < lines.Count; i++)
                input[i] = lines[i].ToCharArray();

            // Find additional rows and cols
            List<int> numAdditionalRows = new();
            List<int> numAdditionalCols = new();

            for (int i = 0; i < lines.Count; i++)
                if (!lines[i].Any(x => x == '#'))
                    numAdditionalRows.Add(i);

            for (int j = 0; j < lines[0].Length; j++)
            {
                var additionalCol = true;
                for (int i = 0; i < lines.Count; i++)
                    if (input[i][j] == '#')
                        additionalCol = false;

                if (additionalCol)
                    numAdditionalCols.Add(j);
            }

            // Rebuild the input
            List<string> newInput = new List<string>();
            
            for (int row = 0; row < lines.Count; row++)
                for (int col = 0; col < lines[0].Length; col++)
                    if (lines[row][col] == '#')
                    {
                        int newRow = row + numAdditionalRows.Count(x => x < row) * ((part == 1) ? 1 : 999999);
                        int newCol = col + numAdditionalCols.Count(x => x < col) * ((part == 1) ? 1 : 999999);

                        Galaxies.Add((newRow, newCol));
                    }
        }

        long FindDistances()
        {
            long sum = 0;
            for (int i = 0; i < Galaxies.Count; i++)
                for (int j = i + 1; j < Galaxies.Count; j++)
                    sum += Galaxies[i].Manhattan(Galaxies[j]);

            return sum;
        }

        public long Solve(int part =1)
            => FindDistances();
    }
}
