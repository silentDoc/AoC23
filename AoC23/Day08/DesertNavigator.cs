namespace AoC23.Day08
{
    internal class DesertNavigator
    {
        Dictionary<string, (string, string)> Map = new();
        string Instructions =  "";

        void ParseLine(string line)
        {
            var nodesOnly = line.Replace("= (", "").Replace(",", "").Replace(")", "");
            var nodes = nodesOnly.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Map[nodes[0]] = (nodes[1], nodes[2]);
        }
        public void ParseInput(List<string> lines)
        {
            Instructions = lines[0];
            for (int i = 2; i < lines.Count; i++)
                ParseLine(lines[i]);
        }

        int NavigateStorm()     // Part 1
        {
            var insPtr = 0;
            var current = "AAA";
            int steps = 0;
            while(current != "ZZZ")
            {
                current = (Instructions[insPtr] == 'L') ? Map[current].Item1 : Map[current].Item2;
                steps++;
                insPtr++;
                if (insPtr == Instructions.Length)
                    insPtr = 0;

            }
            return steps;
        }

        long NavigateStormGhost(string ghostNode)   // For Part 2, different condition
        {
            var insPtr = 0;
            var current = ghostNode;
            int steps = 0;
            while (current.Last() != 'Z')
            {
                current = (Instructions[insPtr] == 'L') ? Map[current].Item1 : Map[current].Item2;
                steps++;
                insPtr++;
                if (insPtr == Instructions.Length)
                    insPtr = 0;
            }
            return (long) steps;
        }

        // Greatest Common Divisor
        long gcd(long num1, long num2)
            => (num2 == 0) ? num1 : gcd(num2, num1 % num2);

        // Least Common Multiple
        long lcm(List<long> numbers)
            => numbers.Aggregate((long S, long val) => S * val / gcd(S, val));


        long GhostNavigator()
        {
            List<string> currentNodes = Map.Keys.Where(x => x.Last() == 'A').ToList();
            var distances = currentNodes.Select(x => NavigateStormGhost(x)).ToList();
            return lcm(distances);
        }

        public long Solve(int part = 1)
            => part == 1 ? NavigateStorm() : GhostNavigator();
    }
}
