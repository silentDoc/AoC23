using System.Text;

namespace AoC23.Day15
{
    class BoxContent
    {
        public string Lens = "";
        public int Focal = 0;
    }

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

        public long SolvePart2()
        {
            Dictionary<int, List<BoxContent>> boxes = new();
            for (int i = 0; i < 256; i++)
                boxes[i] = new();

            foreach (var seq in sequences)
            {
                bool removeOperation = seq.IndexOf('-') != -1;
                var lens = seq.Substring(0, seq.Length - 1);
                

                if (removeOperation)
                {
                    var boxnum = Hash(lens);
                    var existingElement = boxes[boxnum].FirstOrDefault(x => x.Lens == lens);
                    if (existingElement !=null)
                        boxes[boxnum].Remove(existingElement);
                }
                else
                {
                    var groups = seq.Split('=');
                    lens = groups[0];
                    var boxnum = Hash(lens);
                    var existingElement = boxes[boxnum].FirstOrDefault(x => x.Lens == lens);
                    existingElement = boxes[boxnum].FirstOrDefault(x => x.Lens == lens);
                    int focal = int.Parse(groups[1]);

                    if (existingElement != null)
                    {
                        int indexOf = boxes[boxnum].IndexOf(existingElement);
                        boxes[boxnum][indexOf].Focal = focal;
                    }
                    else
                        boxes[boxnum].Add(new BoxContent() { Lens = lens, Focal = focal });
                }
            }

            Dictionary<string, long> valuesPerLens = new();

            for (int box = 0; box < 256; box++)
            {
                for (int j = 0; j < boxes[box].Count; j++)
                {
                    var element = boxes[box][j];
                    var power = (box + 1) * (j+1) * element.Focal;

                    if(!valuesPerLens.ContainsKey(element.Lens))
                        valuesPerLens[element.Lens] = 0;
                    valuesPerLens[element.Lens] += power;
                }
            }

            return valuesPerLens.Values.Sum();
        }

        public long Solve(int part = 1)
            => part == 1 ? (long)sequences.Sum(x => Hash(x)) : SolvePart2();

    }
}
