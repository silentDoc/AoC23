using System.Text;

namespace AoC23.Day15
{
    internal class LavaFactoryHash
    {
        List<string> sequences = new();

        public void ParseInput(List<string> lines)
            => lines[0].Split(',').ToList().ForEach(x => sequences.Add(x));

        int Hash(string seq, int currentValue = 0)
        {
            var cv = currentValue;
            byte[] ascii = Encoding.ASCII.GetBytes(seq);

            for(int i=0;i<seq.Length;i++) 
            {
                cv += (int)ascii[i];
                cv *= 17;
                cv %= 256;
            }

            return cv;
        }

        public int Solve(int part = 1)
            => sequences.Sum(x => Hash(x));

    }
}
