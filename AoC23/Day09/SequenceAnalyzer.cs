namespace AoC23.Day09
{

    class SequenceAnalyzer
    {
        List<List<int>> sequences = new();

        void ParseLine(string line)
            => sequences.Add(line.Split(" ").Select(x => int.Parse(x)).ToList());

        public void ParseInput(List<string> lines)
            => lines.ForEach(ParseLine);

        int FindNextElement(List<int> sequence)
        {
            var range = Enumerable.Range(1, sequence.Count - 1);
            var diff = range.Select(i => sequence[i] - sequence[i - 1]).ToList();
            
            int nextElement = diff.Any(x => x != 0) ? FindNextElement(diff)
                                                    : diff[diff.Count - 1];

            return sequence.Last() + nextElement;
        }

        public int Solve(int part)
            => part == 1 ? sequences.Sum(x => FindNextElement(x)) : 0;
    }
}
