namespace AoC23.Day06
{
    internal class BoatRacerShort
    {
        List<long[]> raceData = new List<long[]>();
        long part2Time = 0;
        long part2Dist = 0;

        public void ParseInput(List<string> lines)
        {
            var timeInputs = lines[0].Replace("Time:", "").Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                     .Select(x => long.Parse(x)).ToList();
            var distInputs = lines[1].Replace("Distance:", "").Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                     .Select(x => long.Parse(x)).ToList();
            part2Time = long.Parse(lines[0].Replace("Time:", "").Replace(" ", ""));
            part2Dist = long.Parse(lines[1].Replace("Distance:", "").Replace(" ", ""));

            for (int i = 0; i < timeInputs.Count; i++)
                raceData.Add(new long[2] { timeInputs[i], distInputs[i] });
        }

        long FindWaysToWin(long[] data)
            => (long) Enumerable.Range(0, (int) data[0]).Count(x => x * (data[0] - x) > data[1]);

        public long Solve(int part)
            => part == 1 ? raceData.Select(rd => FindWaysToWin(rd)).Aggregate(1, (long acc, long x) => x * acc)
                         : FindWaysToWin(new long[] { part2Time, part2Dist });
    }
}
