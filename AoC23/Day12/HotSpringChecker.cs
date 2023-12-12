using System.Text;

namespace AoC23.Day12
{
    class HotSpring
    {
        public List<int> Setup = new();
        public string Arrangement = "";
    }

    internal class HotSpringChecker
    {
        List<List<int>> Setups = new();
        List<string> Arrangements = new();
        int totalArrangements = 0;

        void ParseLine(string line)
        { 
            var parts = line.Split(' ');
            Setups.Add(parts[1].Split(",").Select(int.Parse).ToList());
            Arrangements.Add(parts[0].Trim());
        }

        bool CheckArrangement(string arrangement, List<int> setup)
        {
            var list = arrangement.Split('.', StringSplitOptions.RemoveEmptyEntries);

            if (list.Count() != setup.Count())
                return false;

            for (int i = 0; i < setup.Count; i++)
                if (setup[i] != list[i].Length)
                    return false;

            return true;
        }

        public void BuildArrangement(string currentArrangement, List<int> setup)
        {
            var firstQuestionMark = currentArrangement.IndexOf('?');
            if (firstQuestionMark == -1)
            {
                if (CheckArrangement(currentArrangement, setup))
                    totalArrangements++;
            }
            else
            {
                StringBuilder newArrangement1 = new(currentArrangement);
                StringBuilder newArrangement2 = new(currentArrangement);

                newArrangement1[firstQuestionMark] = '.';
                newArrangement2[firstQuestionMark] = '#';

                BuildArrangement(newArrangement1.ToString(), setup);
                BuildArrangement(newArrangement2.ToString(), setup);
            }
        }

        public int SolvePart1()
        {
            int sum = 0;
            for (int i = 0; i < Setups.Count; i++)
            {
                BuildArrangement(Arrangements[i], Setups[i]);
                sum += totalArrangements;
                totalArrangements = 0;
            }
            return sum;
        }
                

        public void ParseInput(List<string> lines)
            => lines.ForEach(ParseLine);

        public int Solve(int part = 1)
            => SolvePart1();
    }
}
