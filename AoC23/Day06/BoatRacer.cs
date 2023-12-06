using System.Diagnostics;

namespace AoC23.Day06
{
    public class Race
    {
        public int Time = 0;
        public int Distance = 0;

        public Race(int time, int distance)
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
                                     .Select(x => int.Parse(x)).ToList();
            var distInputs = lines[1].Replace("Distance:", "").Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                     .Select(x => int.Parse(x)).ToList();

            for (int i = 0; i < timeInputs.Count; i++)
                Races.Add(new Race(timeInputs[i] , distInputs[i]));
        }

        int FindWaysToWin(Race race)
        {
            var number = 0;
            for (int i = 0; i < race.Time; i++)
            {
                var velocity = i;
                var distance = (race.Time - i) * velocity;
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

        public int Solve(int part)
            => part == 1 ? SolvePart1() : 0;

    }
}
