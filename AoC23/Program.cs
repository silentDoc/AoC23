using AoC23.Day01;
using AoC23.Day02;
using System.Diagnostics;

namespace AoC23
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int day = 2;
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
                _ => throw new ArgumentException("Wrong day number - unimplemented")
            };
            
            st.Stop();
            Console.WriteLine("Result : {0}", result);
            Console.WriteLine("Ellapsed : {0}", st.Elapsed.TotalSeconds);
        }

        static string day1(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            CalibrationReader reader = new();
            reader.ParseInput(lines);

            return reader.Solve(part);
        }

        static string day2(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            CubeBagGame engine = new();
            engine.ParseInput(lines);

            return engine.Solve(part);
        }
    }
}