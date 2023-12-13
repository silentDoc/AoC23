using System.Text;

namespace AoC23.Day13
{
    internal class LavaMirrors
    {
        List<int> horizontalResults = new List<int>();
        List<int> verticalResults = new List<int>();
        List<List<string>> formations = new();

        public void ParseInput(List<string> lines)
        {
            List<string> activeFormation = new();
            foreach (var line in lines) 
                if (string.IsNullOrEmpty(line))
                {
                    formations.Add(activeFormation);
                    activeFormation = new();
                }
                else
                    activeFormation.Add(line);

            formations.Add(activeFormation);
        }

        public bool CheckMirror<T>(List<T> elements, int pos)
        {
            int ini = pos + 1;
            int fin = elements.Count - ini;
            var numElements = ini > fin ? fin : ini;
            var iniSkip = 0;
            if (ini > numElements)
                iniSkip = ini - numElements;

            var subSet1 = elements.Skip(iniSkip).Take(numElements).ToList();
            var subSet2 = elements.Skip(ini).Take(numElements).ToList();
            subSet2.Reverse();

            //return subSet1.Zip(subSet2).Aggregate(true, (result, x) => result &= x.First.Equals(x.Second));
            return !Enumerable.Range(0, subSet1.Count()).Select(i => subSet1[i].Equals(subSet2[i])).Any(x => x == false);
        }

        public int FindMirrorRow(List<string> lines, int ignoreScore = -1)
        {
            for(int i=0; i<lines.Count-1; i++) 
            {
                if (!CheckMirror<string>(lines, i))
                    continue;

                if((i+1)*100 != ignoreScore)
                    return i+1;
            }
            return -1;
        }

        public int FindMirrorColumn(List<string> lines, int ignoreScore=-1)
        {
            // Vert
            for (int i = 0; i < lines[0].Length-1; i++)
            {
                var check = lines.Select(x => CheckMirror<char>(x.ToList(), i)).ToList();
                if (check.Any(x => x == false)) 
                    continue;
                if((i+1) != ignoreScore)
                    return i + 1;
            }
            return -1;
        }

        private int GetValue(List<string> formation, int pos = -1)
        {
            int row    = pos == -1 ? FindMirrorRow(formation)    : FindMirrorRow(formation, verticalResults[pos]);
            int column = pos == -1 ? FindMirrorColumn(formation) : FindMirrorColumn(formation, horizontalResults[pos]);

            column = (column > 0) ? column : 0;
            row = (row > 0) ? row * 100 : 0;

            if (pos == -1)
            {
                horizontalResults.Add(column);   // We keep the original reflections in here
                verticalResults.Add(row);     // to Ignore them for part 2
            }

            return column + row;
        }

        int PatchFormation(List<string> lines, int pos) 
        {
            List<StringBuilder> sbuilders = new();
            GetValue(lines);  // This finds the mirror on the non patched setup, and stores the original results
            foreach (var line in lines)
                sbuilders.Add(new StringBuilder(line));
            
            for (int i = 0; i < lines.Count; i++)
                for (int j = 0; j < lines[0].Length; j++)
                {
                    var oldChar = sbuilders[i][j];

                    if (sbuilders[i][j] == '#')
                        sbuilders[i][j] = '.';
                    else
                        sbuilders[i][j] = '#';

                    List<string> patched = new List<string>();
                    sbuilders.ForEach(x => patched.Add(x.ToString()));

                    var newScore = GetValue(patched, pos);
                    if (newScore > 0)
                        return newScore;
                    else
                        sbuilders[i][j] = oldChar;  // not the valid snudge, restore the position for the next iteration
                }
            return 0;
        }

        private int FindMirrors(int part)
        {
            if (part == 1)
                return formations.Sum(x => GetValue(x));

            int sum = 0;
            for(int i=0; i<formations.Count; i++) 
                sum+= PatchFormation(formations[i],i);
            return sum;
        }

        public int Solve(int part = 1)
            => FindMirrors(part);
    }
}
