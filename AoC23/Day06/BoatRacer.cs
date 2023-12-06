using System.Diagnostics;

namespace AoC23.Day06
{
    public class Race
    {
        public long Time = 0;
        public long Distance = 0;

        public Race(long time, long distance)
        {
            Time = time;
            Distance = distance;
        }
    }

    public class BoatRacer
    {
        List<Race> Races = new();

        public void ParseInput(List<string> lines)
        { 
            var timeInputs = lines[0].Replace("Time:", "").Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                     .Select(x => long.Parse(x)).ToList();
            var distInputs = lines[1].Replace("Distance:", "").Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                     .Select(x => long.Parse(x)).ToList();

            for (int i = 0; i < timeInputs.Count; i++)
                Races.Add(new Race(timeInputs[i] , distInputs[i]));
        }

        int FindWaysToWin(Race race)
        {
            int number = 0;
            for (long i = 0; i < race.Time; i++)
            {
                long velocity = i;
                long distance = (race.Time - i) * velocity;
                if(distance > race.Distance)
                    number++;
            }
            return number;
        }


        int SolvePart1()
        {
            List<int> results = new();

            foreach (var race in Races)
                results.Add(FindWaysToWin(race));

            return results.Aggregate(1, (acc,x) => acc*x);
        }

        int SolvePart2()
        {
            // tweak the input
            var timestr = "";
            var diststr = "";
            for (int i = 0; i < Races.Count; i++)
            {
                timestr = timestr + Races[i].Time.ToString();
                diststr = diststr + Races[i].Distance.ToString();
            }

            var onlyRace = new Race(long.Parse(timestr), long.Parse(diststr));
            return FindWaysToWin(onlyRace);
        }

        public int Solve(int part)
            => part == 1 ? SolvePart1() : SolvePart2();

    }
}
