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
        Dictionary<string, long> LookUpTable = new Dictionary<string, long>();

        // Part 2
        long Calculate(string springs, List<int> groups)
        {
            // Cache key: spring pattern + group lengths
            string  key = springs + string.Join(',', groups); 

            if (LookUpTable.TryGetValue(key, out var value))
                return value;
            
            value = CalculatePart2(springs, groups);
            LookUpTable[key] = value;

            return value;
        }

        long CalculatePart2(string arrangement, List<int> groupsIn)
        {
            List<int> groups = groupsIn.ToList();

            while (true)
            {
                if (groups.Count == 0)
                    return arrangement.Contains('#') ? 0 : 1; // No more groups to match: if there are no springs left, we have a match
        
                if (string.IsNullOrEmpty(arrangement))
                    return 0; 
                

                if (arrangement.StartsWith('.'))
                {
                    arrangement = arrangement.Trim('.'); // Remove all dots from the beginnining (they do not help)
                    continue;
                }

                // Recursion
                if (arrangement.StartsWith('?'))
                     return Calculate("." + arrangement[1..], groups) + Calculate("#" + arrangement[1..], groups); 
              

                if (arrangement.StartsWith('#')) // Start of a group
                {
                    if (groups.Count == 0)
                        return 0; 
                  
                    if (arrangement.Length < groups[0])
                        return 0; // Not enough characters to match the group
                  
                    if (arrangement[..groups[0]].Contains('.'))
                        return 0; // Group cannot contain dots for the given length
                    
                    if (groups.Count > 1)
                    {
                        if (arrangement.Length < groups[0] + 1 || arrangement[groups[0]] == '#')
                            return 0; // A group cannot have a spring next
                       
                        arrangement = arrangement[(groups[0] + 1)..]; // Skip the character after the group - it's either a dot or a question mark
                        groups = groups.Skip(1).ToList();
                        continue;
                    }

                    arrangement = arrangement[groups[0]..]; // Last group, no need to check the character after the group
                    groups = groups.Skip(1).ToList();
                    continue;
                }

                throw new Exception("Invalid input");
            }
        }


        void ParseLine(string line)
        { 
            var parts = line.Split(' ');
            Setups.Add(parts[1].Split(",").Select(int.Parse).ToList());
            Arrangements.Add(parts[0].Trim());
        }

        // Part 1
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

        // Part 1
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

        void UnfoldInput()
        {
            for (int i = 0; i < Setups.Count; i++)
            {
                StringBuilder newArrangement = new StringBuilder(Arrangements[i]);
                List<int> newSetup = new();
                Setups[i].ForEach(x => newSetup.Add(x));
                for (int j = 0; j < 4; j++)
                {
                    newArrangement.Append('?');
                    newArrangement.Append(Arrangements[i]);
                    Setups[i].ForEach(x => newSetup.Add(x));
                }

                Setups[i] = newSetup;
                Arrangements[i] = newArrangement.ToString();
            }
        }

        [Obsolete]
        private bool PartialCheck(string currentArrangement, List<int> setup, int questionPos)
        {
            // This was the first attempt to part 2 - optimize the recursion by cutting non valid threads earlier
            var partialStr = currentArrangement.Substring(0, questionPos);
            var list = partialStr.Split('.', StringSplitOptions.RemoveEmptyEntries);

            var numPartElements = list.Count();

            if (numPartElements > setup.Count())
                return false;

            if (numPartElements == 1)
                return true;

            for (int i = 0; i < numPartElements - 1; i++)
                if (setup[i] != list[i].Length)
                    return false;

            return true;
        }

        [Obsolete]
        public void BuildArrangement2(string currentArrangement, List<int> setup)
        {
            // This was the first attempt to part 2 - optimize the recursion by cutting non valid threads earlier
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

                var na1 = newArrangement1.ToString();
                var na2 = newArrangement2.ToString();

                if (PartialCheck(na1, setup, firstQuestionMark))
                    BuildArrangement2(na1, setup);

                if (PartialCheck(na2, setup, firstQuestionMark))
                    BuildArrangement2(na2, setup);
            }
        }

        public long SolvePart2()
        {
            UnfoldInput();
            long sum = 0;
            for (int i = 0; i < Setups.Count; i++)
            {
                long res = Calculate(Arrangements[i], Setups[i]);
                sum += res;
                totalArrangements = 0;
            }
            return sum;
        }
                

        public void ParseInput(List<string> lines)
            => lines.ForEach(ParseLine);

        public long Solve(int part = 1)
            => part == 1 ? (long) SolvePart1() : SolvePart2();
    }
}
