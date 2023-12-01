using System.Text;

namespace AoC23.Day01
{
    public class CalibrationReader
    {
        List<string> calibrations = new();

        public void ParseInput(List<string> input)
            => input.ForEach(calibrations.Add);

        public int ParseCalibration(string calibration)
        {
            var digits = calibration.ToCharArray().Where(x => char.IsDigit(x)).ToList();
            return int.Parse(digits.First().ToString() + digits.Last().ToString());
        }

        private string Normalize(string input)
        {
            StringBuilder result = new();
            var digitsLetter = new List<string> { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "zero" };
            var digits = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            for (int i = 0; i < input.Length; i++)
            {
                var current = input.Substring(i);
                if (char.IsDigit(current[0]))
                    result.Append(current[0]);
                else
                {
                    var found = digitsLetter.Where(d => current.StartsWith(d)).ToList();
                    if(found.Count > 0)
                        result.Append(digits[digitsLetter.IndexOf(found[0])]);
                }
            }
            return result.ToString();
        }

        public int ParseCalibrationWithLetters(string calibration)
        {
            var normalized = Normalize(calibration);
            return ParseCalibration(normalized);
        }


        private string SolvePart1()
            => calibrations.Select( c => ParseCalibration(c)).Sum().ToString();

        private string SolvePart2()
            => calibrations.Select(c => ParseCalibrationWithLetters(c)).Sum().ToString();

        public string Solve(int part)
            => (part == 1) ? SolvePart1() : SolvePart2();
                
    }
}
