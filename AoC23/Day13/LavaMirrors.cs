using System.Windows.Markup;

namespace AoC23.Day13
{


    internal class LavaMirrors
    {
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
        }



        public int CheckFormation(List<string> lines)
        { 
            // Vert
            for(int i=0; i<lines.Count/2; i++) 
            {
                var subSet1 = lines.Take(i + 1);
                var subSet2 = lines.Skip(i + 1).Take(i + 1);

            }
        }
            

    }
}
