using System.Diagnostics;

namespace AoC23
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int day = 21;
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
                8 => day8(input, part),
                9 => day9(input, part),
                10 => day10(input, part),
                11 => day11(input, part),
                12 => day12(input, part),
                13 => day13(input, part),
                14 => day14(input, part),
                15 => day15(input, part),
                16 => day16(input, part),
                17 => day17(input, part),
                18 => day18(input, part),
                19 => day19(input, part),
                20 => day20(input, part),
                21 => day21(input, part),
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
            racer.ParseInput(lines);

            return racer.Solve(part).ToString();
        }

        static string day7(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day07.CardGameEngine engine = new();
            engine.ParseInput(lines, part); 

            return engine.Solve().ToString();
        }

        static string day8(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day08.DesertNavigator navi = new();
            navi.ParseInput(lines);

            return navi.Solve(part).ToString();
        }

        static string day9(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day09.SequenceAnalyzer analyzer = new();
            analyzer.ParseInput(lines);
            return analyzer.Solve(part).ToString();
        }

        static string day10(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day10.PipeRunnerOpt runner = new();
            runner.ParseInput(lines);
            
            return runner.Solve(part).ToString();
        }

        static string day11(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day11.GalaxyTraveler traveler = new();
            traveler.ParseInput(lines, part);

            return traveler.Solve().ToString();
        }

        static string day12(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day12.HotSpringChecker checker = new();
            checker.ParseInput(lines);

            return checker.Solve(part).ToString();
        }

        static string day13(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day13.LavaMirrors lava = new();
            lava.ParseInput(lines);

            return lava.Solve(part).ToString();
        }

        static string day14(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day14.RockPlatform platform = new();
            platform.ParseInput(lines);
            return platform.Solve(part).ToString();
        }

        static string day15(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day15.LavaFactoryHash hasher = new();
            hasher.ParseInput(lines);
            return hasher.Solve(part).ToString();
        }

        static string day16(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day16.MirrorSystem mirrors = new();
            mirrors.ParseInput(lines);
            return mirrors.Solve(part).ToString();
            //return hasher.Solve(part).ToString();
        }

        static string day17(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day17.HeatLossMinimizer minimizer = new();
            minimizer.ParseInput(lines);
            return minimizer.Solve(part).ToString();
        }

        static string day18(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day18.LavaDigger digger = new();
            digger.ParseInput(lines);
            
            return digger.Solve(part).ToString();
        }

        static string day19(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day19.PartOptimizer optimizer = new();
            optimizer.ParseInput(lines);
            return optimizer.Solve(part).ToString();
        }

        static string day20(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day20.PulseBroadcaster pulser = new();
            pulser.ParseInput(lines);
            return pulser.Solve(part).ToString();
        }

        static string day21(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day21.WalkingElf elf = new();
            elf.ParseInput(lines);

            //Day21.WalkingElf2 elf2 = new();
            
            //elf2.Part2(lines);

            return elf.Solve(part).ToString();
        }

    }
}