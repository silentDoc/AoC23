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

        // Part 1
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

        public void TestRange(PartRange partRange, out PartRange? passes, out PartRange? fails)
        {
            passes = null;
            fails = null;

            if (compareType == CompareType.Direct)
            {
                passes = new PartRange(partRange);
                return;
            }

            var op = operand switch
            {
                'x' => partRange.x,
                'm' => partRange.m,
                'a' => partRange.a,
                's' => partRange.s,
                _ => throw new Exception("Invalid operand")
            };

            if (value > op.min && value < op.max)
            {
                passes = new PartRange(partRange);
                fails = new PartRange(partRange);

                (int min, int max) passR;
                (int min, int max) failR;

                // We will split into two ranges
                passR.min = (compareType == CompareType.Less) ? op.min : value + 1;
                passR.max = (compareType == CompareType.Less) ? value-1 : op.max;
                failR.min = (compareType == CompareType.Less) ? value : op.min;
                failR.max = (compareType == CompareType.Less) ? op.max : value;

                switch (operand)
                {
                    case 'x':
                        passes.x = passR;
                        fails.x = failR;
                        break;
                    case 'm':
                        passes.m = passR;
                        fails.m = failR;
                        break;
                    case 'a':
                        passes.a = passR;
                        fails.a = failR;
                        break;
                    case 's':
                        passes.s = passR;
                        fails.s = failR;
                        break;
                    default:
                        break;
                }
            }
            else if (value > op.max)
            {
                passes = (compareType == CompareType.Less) ? new PartRange(partRange) : null;
                fails = (compareType == CompareType.Less) ? null : new PartRange(partRange);
            }
            else if (value < op.min)
            {
                passes = (compareType == CompareType.Less) ? null :  new PartRange(partRange);
                fails = (compareType == CompareType.Less) ? new PartRange(partRange) : null;
            }
            return;
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

    class PartRange
    {
        public (int min, int max) x;
        public (int min, int max) m;
        public (int min, int max) a;
        public (int min, int max) s;

        public PartRange()
        {
            x = (1, 4000);
            m = (1, 4000);
            a = (1, 4000);
            s = (1, 4000);
        }

        public PartRange(PartRange other)
        {
            x = other.x;
            m = other.m;
            a = other.a;
            s = other.s;
        }

        public long Combs()
            => ((long)(x.max - x.min + 1)) * ((long)(m.max - m.min + 1)) * ((long)(a.max - a.min + 1)) * ((long)(s.max - s.min + 1));
    }

    internal class PartOptimizer
    {
        Dictionary<string, Workflow> Workflows = new();
        List<Part> Parts = new();

        public void ParseInput(List<string> lines)
        {
            var sep = lines.IndexOf("");
            for (int i = 0; i < sep; i++)
            {
                var wf = new Workflow(lines[i]);
                Workflows[wf.Name] = wf;
            }

            for (int i = sep + 1; i < lines.Count; i++)
                Parts.Add(new Part(lines[i]));
        }

        int RunWorkflows()
        {
            List<Part> Accepted = new();

            foreach (var part in Parts)
            {
                var result = Workflows["in"].RunWorkflow(part);
                while (result.outcome == Outcome.Jump)
                {
                    result = Workflows[result.dest].RunWorkflow(part);
                }

                if (result.outcome == Outcome.Accept)
                    Accepted.Add(part);
            }

            return Accepted.Sum( p => p.x) + Accepted.Sum(p => p.a) + Accepted.Sum(p => p.m) + Accepted.Sum(p => p.s);
        }

        long CheckCombinations()
        {
            // We build the tree
            PartRange start = new();
            Queue<(PartRange, Workflow)> queue = new();
            queue.Enqueue((start, Workflows["in"]));
            long total = 0;

            while(queue.Count>0)
            {
                var (range, flow) = queue.Dequeue();

                foreach(var rule in flow.Rules)
                {
                    if (range == null)
                        break;
                    PartRange? pass = null;
                    PartRange? fail = null;
                    
                    rule.TestRange(range, out pass, out fail);

                    if (rule.compareType == CompareType.Direct && rule.outcome == Outcome.Accept)
                    {
                        if(pass!=null)
                            total += pass.Combs();
                        break;
                    }
                    if (rule.compareType == CompareType.Direct && rule.outcome == Outcome.Jump)
                    {
                        if (pass != null)
                            queue.Enqueue((pass, Workflows[rule.destination]));
                        range = fail;
                        continue;
                    }

                    if (rule.compareType != CompareType.Direct)
                    {
                        if (rule.outcome == Outcome.Accept)
                        {
                            if (pass != null)
                                total += pass.Combs();
                        }
                        if (rule.outcome == Outcome.Jump)
                        {
                            if (pass != null)
                                queue.Enqueue((pass, Workflows[rule.destination]));
                        }
                        range = fail;   // to next rule
                    }
                }
            }

            return total;
        }

        public long Solve(int part = 1)
            => part == 1 ? RunWorkflows() : CheckCombinations();

    }
}
