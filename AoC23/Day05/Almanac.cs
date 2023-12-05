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

    public class GardenValueSet
    {
        public long seed = -1;
        public long soil = -1;
        public long fert = -1;
        public long water = -1;
        public long light = -1;
        public long temp = -1;
        public long humid = -1;
        public long location = -1;
    }

    class Almanac
    {
        List<long> seeds = new();
        List<GardenValueSet> gardenRelations = new();

        List<AlmanacRule> seed2soil = new();
        List<AlmanacRule> soil2fert = new();
        List<AlmanacRule> fert2water = new();
        List<AlmanacRule> water2light = new();
        List<AlmanacRule> light2temp = new();
        List<AlmanacRule> temp2humid = new();
        List<AlmanacRule> humid2location = new();

        public void ParseInput(List<string> lines)
        {
            seeds = lines[0].Replace("seeds: ", "").Split(' ')
                            .Select(x => long.Parse(x)).ToList();

            var pointer = 3;
            while (!string.IsNullOrEmpty(lines[pointer]))
                seed2soil.Add(new AlmanacRule(lines[pointer++]));
            pointer += 2;
            while (!string.IsNullOrEmpty(lines[pointer]))
                soil2fert.Add(new AlmanacRule(lines[pointer++]));
            pointer += 2;
            while (!string.IsNullOrEmpty(lines[pointer]))
                fert2water.Add(new AlmanacRule(lines[pointer++]));
            pointer += 2;
            while (!string.IsNullOrEmpty(lines[pointer]))
                water2light.Add(new AlmanacRule(lines[pointer++]));
            pointer += 2;
            while (!string.IsNullOrEmpty(lines[pointer]))
                light2temp.Add(new AlmanacRule(lines[pointer++]));
            pointer += 2;
            while (!string.IsNullOrEmpty(lines[pointer]))
                temp2humid.Add(new AlmanacRule(lines[pointer++]));
            pointer += 2;
            while (pointer<lines.Count)
                humid2location.Add(new AlmanacRule(lines[pointer++]));
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

                // Fully process a range
                
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
            foreach (var s in seeds)
            {
                GardenValueSet relation = new();
                relation.seed = s;
                gardenRelations.Add(relation);
            }
           
            foreach (var relation in gardenRelations)
                relation.soil = MapValue(relation.seed, seed2soil);
            foreach (var relation in gardenRelations)
                relation.fert = MapValue(relation.soil, soil2fert);
            foreach (var relation in gardenRelations)
                relation.water = MapValue(relation.fert, fert2water);
            foreach (var relation in gardenRelations)
                relation.light = MapValue(relation.water, water2light);
            foreach (var relation in gardenRelations)
                relation.temp = MapValue(relation.light, light2temp);
            foreach (var relation in gardenRelations)
                relation.humid = MapValue(relation.temp, temp2humid);
            foreach (var relation in gardenRelations)
                relation.location = MapValue(relation.humid, humid2location);

            return gardenRelations.Min(x => x.location);
        }
        long SolvePart2()
        {
            List<long> preppedSeeds = new();

            for (int i = 0; i < seeds.Count; i++)
                if (i % 2 == 0)
                    preppedSeeds.Add(seeds[i]);
                else
                    preppedSeeds.Add(seeds[i-1] + seeds[i]-1);

            var soilRanges = MapRange(preppedSeeds, seed2soil);
            var fertRanges = MapRange(soilRanges, soil2fert);
            var waterRanges = MapRange(fertRanges, fert2water);
            var lightRanges = MapRange(waterRanges, water2light);
            var tempRanges = MapRange(lightRanges, light2temp);
            var humidRanges = MapRange(tempRanges, temp2humid);
            var locationRanges = MapRange(humidRanges, humid2location);

            return locationRanges.Min()-1;
    }
        public long Solve(int part = 1)
            => part == 1 ? SolvePart1() : SolvePart2();

    }
}
