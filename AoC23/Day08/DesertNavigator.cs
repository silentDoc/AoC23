namespace AoC23.Day08
{
    class DesertNode
    {
        public string Name;
        public string sLeft;
        public string sRight;
        public DesertNode? Left = null;
        public DesertNode? Right = null;
    }

    internal class DesertNavigator
    {
        List<DesertNode> Nodes = new();
        string Instructions =  "";

        void ParseLine(string line)
        {
            var nodesOnly = line.Replace("= (", "").Replace(",", "").Replace(")", "");
            var nodes = nodesOnly.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            DesertNode newNode = new();
            newNode.Name = nodes[0];
            newNode.sLeft = nodes[1];
            newNode.sRight = nodes[2];

            Nodes.Add(newNode);
        }

        void LinkNodes()
        {
            foreach (var node in Nodes)
            {
                node.Left = Nodes.First(n => n.Name == node.sLeft);
                node.Right = Nodes.First(n => n.Name == node.sRight);
            }
        }

        public void ParseInput(List<string> lines)
        {
            Instructions = lines[0];
            for (int i = 2; i < lines.Count; i++)
                ParseLine(lines[i]);

            LinkNodes();
        }

        int NavigateStorm()
        {
            var insPtr = 0;
            DesertNode current = Nodes.First(n => n.Name == "AAA");
            int steps = 0;
            while(current.Name != "ZZZ")
            {
                current = (Instructions[insPtr] == 'L') ? current.Left : current.Right;
                steps++;
                insPtr++;
                if (insPtr == Instructions.Length)
                    insPtr = 0;

            }

            return steps;
        }

        public int Solve(int part = 1)
            => part == 1 ? NavigateStorm() : 0;
    }
}
