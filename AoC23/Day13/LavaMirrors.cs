using System.Globalization;
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

        public int CheckFormationVert(List<string> lines, int ignoreScore = -1)
        {
            int a = 0;
            // Vert
            for(int i=0; i<lines.Count-1; i++) 
            {
                int ini = i + 1;
                int fin = lines.Count - ini;
                var numElements = ini > fin ? fin : ini;
                var iniSkip = 0;
                if (ini > numElements)
                    iniSkip = ini - numElements;

                var subSet1 = lines.Skip(iniSkip).Take(numElements).ToList();
                var subSet2 = lines.Skip(ini).Take(numElements).ToList();
                subSet2.Reverse();

                var peek = Enumerable.Range(0, subSet1.Count()).Select(i => subSet1[i] == subSet2[i]).ToList();

                if (Enumerable.Range(0, subSet1.Count()).Select(i => subSet1[i] == subSet2[i]).Any(x => x == false))
                    continue;
                if((i+1)*100 != ignoreScore)
                    return i+1;
                
            }
            return -1;
        }

        public bool CheckLineHoriz(string line, int pos)
        {
            int ini = pos + 1;
            int fin = line.Length - ini;
            var numElements = ini > fin ? fin : ini;
            var iniSkip = 0;
            if (ini > numElements)
                iniSkip = ini - numElements;

            var subSet1 = line.Skip(iniSkip).Take(numElements).ToList();
            var subSet2 = line.Skip(ini).Take(numElements).ToList();
            subSet2.Reverse();

            return !Enumerable.Range(0, subSet1.Count()).Select(i => subSet1[i] == subSet2[i]).Any(x => x == false);
        }

        public int CheckFormationHoriz(List<string> lines, int ignoreScore=-1)
        {
            // Vert
            for (int i = 0; i < lines[0].Length-1; i++)
            {
                var check = lines.Select(x => CheckLineHoriz(x, i)).ToList();
                if(check.Any(x => x == false)) 
                    continue;
                if((i+1) != ignoreScore)
                    return i + 1;

            }
            return -1;
        }

        private int FormationValue(List<string> formation)
        { 
            int v = CheckFormationVert(formation);
            int h = CheckFormationHoriz(formation);

            h = (h>0) ? h : 0;
            v = (v > 0) ? v*100 : 0;

            horizontalResults.Add(h);
            verticalResults.Add(v);

            return h + v;
        }

        private int FormationValueIgnoring(List<string> formation, int pos)
        {
            int v = CheckFormationVert(formation, verticalResults[pos]);
            int h = CheckFormationHoriz(formation, horizontalResults[pos]);

            h = (h > 0) ? h : 0;
            v = (v > 0) ? v * 100 : 0;

            return h + v;
        }

        int PatchFormation(List<string> lines, int pos) 
        {
            List<StringBuilder> sbuilders = new();
            int OriScore = FormationValue(lines);
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

                    var newScore = FormationValueIgnoring(patched, pos);
                    if (newScore > 0)
                    {
                        Console.WriteLine(i.ToString() + " - " + j.ToString() + " - " +newScore.ToString());
                        return newScore;
                    }
                    else
                        sbuilders[i][j] = oldChar;

                }
            return 0;
        }

        private int SolvePart1()
            => formations.Sum(x => FormationValue(x));

        private int SolvePart2()
        {
            int sum = 0;
            for(int i=0; i<formations.Count; i++) 
                sum+= PatchFormation(formations[i],i);
            return sum;
        }
        

        public int Solve(int part = 1)
            => part ==1 ? SolvePart1() : SolvePart2();
       

    }
}
