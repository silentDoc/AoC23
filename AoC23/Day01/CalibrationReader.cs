using System.Text;

namespace AoC23.Day01
{
    public class CalibrationReader
    {
        List<string> calibrations = new();

        public void ParseInput(List<string> input)
            => input.ForEach(calibrations.Add);

        public int ParseCalibration(string calibration, bool withLetters = false)
        {
            var nomalized = withLetters ? Normalize(calibration) : calibration;

            var digits = nomalized.ToCharArray()
                                  .Where(x => char.IsDigit(x))
                                  .ToList();

            return int.Parse( digits.First().ToString() + digits.Last().ToString() );
        }

        private string Normalize(string input)
        {
            StringBuilder result = new();
            var digitsLetter = new List<string> {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            for (int i = 0; i < input.Length; i++)
            {
                var current = input.Substring(i);
                
                if (char.IsDigit(current[0]))
                    result.Append(current[0]);
                else
                {
                    var found = digitsLetter.Where(d => current.StartsWith(d)).ToList();
                    if(found.Count > 0)
                        result.Append(digitsLetter.IndexOf(found[0]));
                }
            }
            return result.ToString();
        }

        public string Solve(int part)
            => calibrations.Select(c => ParseCalibration(c, part == 2))
                           .Sum()
                           .ToString();
    }
}
