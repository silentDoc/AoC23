using System.ComponentModel.DataAnnotations;

namespace AoC23.Day20
{
    enum NodeType
    { 
        Broadcaster, 
        FlipFlop, 
        Conjunction
    }

    enum PulseType
    {
        High, 
        Low
    }

    class PulseNode
    {
        public string Name = "";
        public NodeType Type = NodeType.Broadcaster;
        public bool isOn = false;
        public PulseType recentPulse = PulseType.Low;
        public List<string> ConnectionsOut = new();
        public Dictionary<string, PulseType> ConnectionsIn = new();
    }

    record PulseToCheck
    {
        public string name;
        public string from;
        public PulseType type;
    }

    internal class PulseBroadcaster
    {
        public Dictionary<string, PulseNode> Network = new();
        
        void ParseLine(string line)
        { 
            var parts = line.Split("->", StringSplitOptions.TrimEntries);
            var name = parts[0] != "broadcaster" ? parts[0].Substring(1) : parts[0];

            var node = Network.ContainsKey(name) ? Network[name] : new PulseNode();
            node.Name = name;
            node.Type = name=="broadcaster" ? NodeType.Broadcaster 
                                            : parts[0].First() == '%' ? NodeType.FlipFlop                                                                                      
                                                                      : NodeType.Conjunction;
            node.ConnectionsOut = new(parts[1].Split(",", StringSplitOptions.TrimEntries).ToList());

            Network[name] = node;
        }

        public void ParseInput(List<string> lines)
            => lines.ForEach(ParseLine);

        void InitIncomingConns()
        {
            foreach (var conn in Network.Keys)
                foreach (var from in Network.Keys)
                    if (Network[from].ConnectionsOut.Contains(conn))
                        Network[conn].ConnectionsIn[from] = PulseType.Low;
        }

        long SendPulse()
        {
            InitIncomingConns();

            Queue<PulseToCheck> toCheck = new();
            PulseToCheck start = new PulseToCheck() { name = "broadcaster", from="button", type = PulseType.Low };
            toCheck.Enqueue(start);

            long count = 1; //Button

            while (toCheck.Count > 0)
            {
                var current = toCheck.Dequeue();
                var pulse = Network[current.name];
                var incomingType = current.type;

                if (pulse.Type == NodeType.FlipFlop)
                {
                    if (incomingType == PulseType.High)
                        continue;
                    pulse.isOn = !pulse.isOn;
                    var outgoingType = pulse.isOn ? PulseType.High : PulseType.Low;
                    foreach (var conn in pulse.ConnectionsOut)
                    {
                        toCheck.Enqueue(new PulseToCheck() { name = conn, from = current.name, type = outgoingType });
                        count++;
                    }
                }

                if (pulse.Type == NodeType.Conjunction)
                {
                    pulse.ConnectionsIn[current.from] = current.type;
                    PulseType toSend = PulseType.Low;
                    if (pulse.ConnectionsIn.Values.Any(x => x == PulseType.Low))
                        toSend = PulseType.High;
                    foreach (var conn in pulse.ConnectionsOut)
                    {
                        toCheck.Enqueue(new PulseToCheck() { name = conn, from = current.name, type = toSend });
                        count++;
                    }
                }

                if (pulse.Type == NodeType.Broadcaster)
                {
                    foreach (var conn in pulse.ConnectionsOut)
                    {
                        toCheck.Enqueue(new PulseToCheck() { name = conn, from = current.name, type = current.type });
                        count++;
                    }
                }

            }
            return count;
        }

        public long Solve(int part)
            => SendPulse(); 
    }
}
