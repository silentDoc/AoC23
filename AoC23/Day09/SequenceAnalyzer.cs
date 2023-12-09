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
            return part == 1 ? seq.Last() + (difs.Any(x => x != 0) ? FindElement(difs, part) : difs[^1])
                             : seq.First() - (difs.Any(x => x != 0) ? FindElement(difs, part) : difs[0]);
        }

        public int Solve(int part)
            => sequences.Sum(x => FindElement(x, part));
    }
}
