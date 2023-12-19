namespace AoC23.Day19
{
    enum CompareType
    { 
        Greater,
        Less, 
        Direct
    }

    enum Outcome
    {
        Accept,
        Reject,
        Jump
    }

    class Rule
    {
        public string destination ="";
        public char operand = ' ';
        public int value = -1;
        public CompareType compareType = CompareType.Direct;
        public Outcome outcome = Outcome.Accept;

        public Rule(string inputString)
        {
            if (inputString[0] == 'A' || inputString[0] == 'R')     // A or R
            {
                compareType = CompareType.Direct;
                outcome = inputString[0] == 'A' ? Outcome.Accept : Outcome.Reject;
                return;
            }

            if (inputString.IndexOf(":") == -1)     // rfg
            {
                compareType = CompareType.Direct;
                outcome = Outcome.Jump;
                destination = inputString;
                return;
            }

            // Comparison  a<2006:qkq or a>406:A
            operand = inputString[0];
            compareType = inputString[1]=='<' ? CompareType.Less : CompareType.Greater;
            var rest = inputString.Substring(2).Split(":", StringSplitOptions.TrimEntries);
            value = int.Parse(rest[0]);
            destination = rest[1];
            if (destination[0] == 'A')
                outcome = Outcome.Accept;
            else if (destination[0] == 'R')
                outcome = Outcome.Reject;
            else
                outcome = Outcome.Jump;
        }

        public bool Test(Part part)
        {
            if (compareType == CompareType.Direct)
                return true;

            var op = operand switch
            {
                'x' => part.x,
                'm' => part.m,
                'a' => part.a,
                's' => part.s,
                _ => throw new Exception("Invalid operand")
            };

            return compareType == CompareType.Less ? op < value : op > value;
        }
    }

    class Workflow
    {
        public string Name;
        public List<Rule> Rules;

        public Workflow(string inputString)
        {
            // px{a<2006:qkq,m>2090:A,rfg}
            var parts = inputString.Split("{", StringSplitOptions.TrimEntries);
            Name = parts[0];
            var rules = parts[1].Replace("}", "").Split(",");
            Rules = new();
            foreach (var rule in rules)
                Rules.Add(new Rule(rule));
        }

        public (Outcome outcome, string dest) RunWorkflow(Part part)
        {
            foreach (var rule in Rules)
            {
                if (rule.Test(part))
                    return (rule.outcome, rule.destination);
            }

            return (Outcome.Reject, "");
        }
    }

    class Part
    {
        public int x = 0;
        public int m = 0;
        public int a = 0;
        public int s = 0;

        public Part(string inputString)
        {
            var strNums = inputString.Replace("{x=", "").Replace("m=", "")
                                     .Replace("a=", "").Replace("s=", "").Replace("}", "");
            var nums = strNums.Split(',').Select(int.Parse).ToList();
            x = nums[0];
            m = nums[1];
            a = nums[2]; 
            s = nums[3];
        }
    }

    internal class PartOptimizer
    {
        List<Workflow> Workflows = new();
        List<Part> Parts = new();

        public void ParseInput(List<string> lines)
        {
            var sep = lines.IndexOf("");
            for(int i=0; i<sep; i++) 
                Workflows.Add(new Workflow(lines[i]));

            for (int i = sep + 1; i < lines.Count; i++)
                Parts.Add(new Part(lines[i]));
        }

        int RunWorkflows()
        {
            List<Part> Accepted = new();

            foreach (var part in Parts)
            {
                Workflow current = Workflows.First(x => x.Name == "in");
                var result = current.RunWorkflow(part);
                while (result.outcome == Outcome.Jump)
                {
                    current = Workflows.First(x => x.Name == result.dest);
                    result = current.RunWorkflow(part);
                }

                if (result.outcome == Outcome.Accept)
                    Accepted.Add(part);
            }

            return Accepted.Sum( p => p.x) + Accepted.Sum(p => p.a) + Accepted.Sum(p => p.m) + Accepted.Sum(p => p.s);
        }

        public int Solve(int part = 1)
            => RunWorkflows();

    }
}
