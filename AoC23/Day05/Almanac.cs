namespace AoC23.Day05
{
    public class AlmanacRule
    {
        public long sourceStart = 0;
        public long sourceEnd = 0;
        public long destinationStart = 0;
        public long range = 0;

        public AlmanacRule(string inputLine)
        {
            var nums = inputLine.Split(' ').Select(x => long.Parse(x)).ToList();
            destinationStart = nums[0];
            sourceStart = nums[1];
            range = nums[2];
            sourceEnd = sourceStart + nums[2];
        }

        public bool InRange(long queryValue)
            => queryValue >= sourceStart && queryValue <= sourceEnd;

        public long MapValue(long queryValue)
            => destinationStart + (queryValue - sourceStart);
    }

    class Almanac
    {
        List<long> seeds = new();
        List<AlmanacRule>[] ruleSets = new List<AlmanacRule>[7];

        public void ParseInput(List<string> lines)
        {
            seeds = lines[0].Replace("seeds: ", "").Split(' ')
                            .Select(x => long.Parse(x)).ToList();

            var pointer = 1;
            for (int i = 0; i < 6; i++)
            {
                ruleSets[i] = new();
                pointer += 2;
                while (!string.IsNullOrEmpty(lines[pointer]))
                    ruleSets[i].Add(new AlmanacRule(lines[pointer++]));
            }

            pointer += 2;
            ruleSets[6] = new();
            while (pointer<lines.Count)
                ruleSets[6].Add(new AlmanacRule(lines[pointer++]));
        }
       
        long MapValue(long queryValue, List<AlmanacRule> rules)
        {
            var rule = rules.FirstOrDefault(x => x.InRange(queryValue));
            return rule == null ? queryValue : rule.MapValue(queryValue);
        }

        bool IsMapped(long queryValue, List<AlmanacRule> rules)
            => rules.Any(x => x.InRange(queryValue));

        AlmanacRule WhichRuleMaps(long queryValue, List<AlmanacRule> rules)
            => rules.First(x => x.InRange(queryValue));

        List<long> MapRange(List<long> ranges, List<AlmanacRule> rules)
        {
            List<long> result = new();

            for(int i = 0; i < ranges.Count; i++) 
            {
                long rangeStart = ranges[i++];
                long rangeEnd = ranges[i];
                long current = rangeStart;

                while (current <= rangeEnd)
                {
                    result.Add(MapValue(current, rules));

                    if (IsMapped(current, rules))
                    {
                        var rule = WhichRuleMaps(current, rules);
                        // Find until which point the rule allows us to map the value
                        var lastValueInRange = Math.Min(rangeEnd, rule.sourceEnd);
                        result.Add(rule.MapValue(lastValueInRange));
                        current = lastValueInRange;
                        if (current == rule.sourceEnd)
                            current++;

                        if (current == rangeEnd)
                            break;
                    }
                    else
                    {
                        // Not mapped, find the first rule that maps it
                        var nextRule = rules.Where(x => x.sourceStart > current).OrderBy(x => x.sourceStart).FirstOrDefault();
                        if (nextRule == null)
                        {
                            result.Add(rangeEnd);
                            current = rangeEnd;
                            break;
                        }
                        else
                        {
                            var nextValueInRange = Math.Min(nextRule.sourceStart, rangeEnd);
                            if (nextRule.sourceStart == rangeEnd)
                            {
                                result.Add(rangeEnd - 1);
                                result.Add(nextRule.MapValue(rangeEnd));
                                result.Add(nextRule.MapValue(rangeEnd));
                                break;
                            }
                            else if (nextRule.sourceStart > rangeEnd)
                            {
                                result.Add(rangeEnd);
                                break;
                            }
                            else
                            {
                                result.Add(nextRule.sourceStart-1);
                                current = nextRule.sourceStart;
                            }
                        }
                    }
                }
            }
            return result;
        }
            

        long SolvePart1()
        {
            List<long> results = new();
            foreach (var currentSeed in seeds)
            {
                long value = currentSeed;
                for (int r = 0; r < 7; r++)
                    value = MapValue(value, ruleSets[r]);
                results.Add(value);
            }

            return results.Min();
        }
        long SolvePart2()
        {
            List<long> ranges = new();

            for (int i = 0; i < seeds.Count; i++)
                if (i % 2 == 0)
                    ranges.Add(seeds[i]);
                else
                    ranges.Add(seeds[i-1] + seeds[i]-1);

            for(int i = 0;i<7; i++)
                ranges = MapRange(ranges, ruleSets[i]);

            return ranges.Min()-1;
        }

        public long Solve(int part = 1)
            => part == 1 ? SolvePart1() : SolvePart2();
    }
}
