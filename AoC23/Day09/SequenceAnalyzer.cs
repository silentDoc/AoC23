namespace AoC23.Day09
{
    class SequenceAnalyzer
    {
        List<List<int>> sequences = new();

        void ParseLine(string line)
            => sequences.Add(line.Split(" ").Select(x => int.Parse(x)).ToList());

        public void ParseInput(List<string> lines)
            => lines.ForEach(ParseLine);

        int FindElement(List<int> seq, int part = 1)
        {
            var difs = seq.Skip(1).Zip(seq.Take(seq.Count - 1), (f, s) => f - s).ToList();
            return difs.Any(x => x != 0) ? (part == 1) ? seq.Last() + FindElement(difs, part) : seq.First() - FindElement(difs, part)
                                         : (part == 1) ? seq.Last() + difs[difs.Count - 1] : seq.First() - difs[0];
        }

        public int Solve(int part)
            => sequences.Sum(x => FindElement(x, part));
    }
}
