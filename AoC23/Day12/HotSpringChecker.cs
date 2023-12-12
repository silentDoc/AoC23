using System.Text;

namespace AoC23.Day12
{
    internal class HotSpringChecker
    {
        List<List<int>> Setups = new();
        List<string> Arrangements = new();
        Dictionary<string, long> LookUpTable = new Dictionary<string, long>();

        void ParseLine(string line)
        { 
            var parts = line.Split(' ');
            Setups.Add(parts[1].Split(",").Select(int.Parse).ToList());
            Arrangements.Add(parts[0].Trim());
        }

        void UnfoldInput()
        {
            for (int i = 0; i < Setups.Count; i++)
            {
                StringBuilder newArrangement = new StringBuilder(Arrangements[i]);
                List<int> newSetup = new();
                Setups[i].ForEach(x => newSetup.Add(x));
                newArrangement.Append(string.Concat(Enumerable.Repeat("?" + Arrangements[i], 4)));

                for (int j = 0; j < 4; j++)
                    Setups[i].ForEach(x => newSetup.Add(x));

                Setups[i] = newSetup;
                Arrangements[i] = newArrangement.ToString();
            }
        }

        // Part 2
        long Calculate(string springs, List<int> groups)
        {
            // Cache key: spring pattern + group lengths
            string key = springs + string.Join(',', groups);

            if (LookUpTable.TryGetValue(key, out var value))
                return value;

            value = SolveSubGroup(springs, groups);
            LookUpTable[key] = value;

            return value;
        }

        long SolveSubGroup(string arrangement, List<int> groupsIn)
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

        public long CalcArrangements(int part = 1)
        {
            if(part ==2)
                UnfoldInput();

            long sum = 0;
            for (int i = 0; i < Setups.Count; i++)
                sum += Calculate(Arrangements[i], Setups[i]);
            return sum;
        }

        public void ParseInput(List<string> lines)
            => lines.ForEach(ParseLine);

        public long Solve(int part = 1)
            => CalcArrangements(part);
    }
}
