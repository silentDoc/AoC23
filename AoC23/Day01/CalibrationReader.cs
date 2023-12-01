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
        

        private string SolvePart1()
            => calibrations.Select( c => ParseCalibration(c)).Sum().ToString();

        public string Solve(int part)
            => (part == 1) ? SolvePart1() : "";
                
    }
}
