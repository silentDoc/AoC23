using System.Diagnostics;

namespace AoC23
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int day = 7;
            int part = 2;
            bool test = false;

            string input = "./Input/day" + day.ToString("00");
            input += (test) ? "_test.txt" : ".txt";

            Console.WriteLine("AoC 2023 - Day {0} , Part {1} - Test Data {2}", day, part, test);
            Stopwatch st = new();
            st.Start();
            string result = day switch
            {
                1 => day1(input, part),
                2 => day2(input, part),
                3 => day3(input, part),
                4 => day4(input, part),
                5 => day5(input, part),
                6 => day6(input, part),
                7 => day7(input, part),
                _ => throw new ArgumentException("Wrong day number - unimplemented")
            };
            
            st.Stop();
            Console.WriteLine("Result : {0}", result);
            Console.WriteLine("Elapsed : {0}", st.Elapsed.TotalSeconds);
        }

        static string day1(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day01.CalibrationReader reader = new();
            reader.ParseInput(lines);

            return reader.Solve(part);
        }

        static string day2(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day02.CubeBagGame engine = new();
            engine.ParseInput(lines);

            return engine.Solve(part);
        }

        static string day3(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day03.GondolaCalculator gondola = new();
            gondola.ParseInput(lines);

            return gondola.Solve(part);
        }

        static string day4(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day04.Scratcher scratcher = new();
            scratcher.ParseInput(lines);

            return scratcher.Solve(part).ToString();
        }

        static string day5(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day05.Almanac almanac = new();
            almanac.ParseInput(lines);

            return almanac.Solve(part).ToString();
        }

        static string day6(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day06.BoatRacer racer = new();
            //Day06.BoatRacerShort racer = new();
            racer.ParseInput(lines);

            return racer.Solve(part).ToString();
        }

        static string day7(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day07.CardGameEngine engine = new();
            engine.ParseInput(lines, part);

            return engine.Solve(part).ToString();
        }
    }
}