namespace AoC23.Day20
{
    enum ModuleType
    { 
        Broadcaster, 
        FlipFlop, 
        Conjunction, 
        Untyped
    }

    enum PulseType
    {
        High, 
        Low
    }

    class Module
    {
        public string Name = "";
        public ModuleType Type = ModuleType.Untyped;
        public bool isOn = false;
        public List<string> Sources = new();
        public Dictionary<string, PulseType> Dests = new();
    }

    record Pulse
    {
        public string Dest = "";
        public string Source = "";
        public PulseType type;
    }

    internal class PulseBroadcaster
    {
        public Dictionary<string, Module> Network = new();
        
        void ParseLine(string line)
        { 
            var parts = line.Split("->", StringSplitOptions.TrimEntries);
            var name = parts[0] != "broadcaster" ? parts[0].Substring(1) : parts[0];

            var node = Network.ContainsKey(name) ? Network[name] : new Module();
            node.Name = name;
            node.Type = name=="broadcaster" ? ModuleType.Broadcaster 
                                            : parts[0].First() == '%' ? ModuleType.FlipFlop                                                                                      
                                                                      : ModuleType.Conjunction;
            node.Sources = new(parts[1].Split(",", StringSplitOptions.TrimEntries).ToList());
            Network[name] = node;
        }

        public void ParseInput(List<string> lines)
            => lines.ForEach(ParseLine);

        void InitIncomingConns()    // Adds the list of nodes that feed the node in question
        {
            foreach (var conn in Network.Keys)
                foreach (var from in Network.Keys)
                    if (Network[from].Sources.Contains(conn))
                        Network[conn].Dests[from] = PulseType.Low;
        }

        void CompleteNodes()
        { 
            // Checks that any destination node exists as a node in our network
            var allNodes = Network.Values.SelectMany(x => x.Sources).Distinct().ToList();
            foreach(var node in allNodes)
                if(!Network.Keys.Contains(node))
                {
                    Module newNode = new();
                    newNode.Name = node;
                    newNode.Type = ModuleType.Untyped;
                    Network[node] = newNode;
                }
        }

        long SendPulse(int part = 1)
        {
            CompleteNodes();
            InitIncomingConns();
            long countLo = 0; //Button
            long countHi = 0;
            long buttonPushes = 0;

            // For part 2 - they are the modules that feed rm - which is the inversor that feeds rx
            long dh = -1;
            long qd = -1;
            long bb = -1;
            long dp = -1;

            bool stop = false;
            
            while(!stop)
            {
                buttonPushes++;
                Queue<Pulse> toCheck = new();
                Pulse start = new Pulse() { Dest = "broadcaster", Source = "button", type = PulseType.Low };
                toCheck.Enqueue(start);

                while (toCheck.Count > 0)
                {
                    var currentPulse = toCheck.Dequeue();
                    var module = Network[currentPulse.Dest];
                    var incomingType = currentPulse.type;

                    // Part 2
                    if (module.Name == "rm")
                    {
                        // rm is the only one that activates rx
                        if (module.Dests["dp"] == PulseType.High && dp == -1)
                            dp = buttonPushes;
                        if (module.Dests["qd"] == PulseType.High && qd == -1)
                            qd = buttonPushes;
                        if (module.Dests["bb"] == PulseType.High && bb == -1)
                            bb = buttonPushes;
                        if (module.Dests["dh"] == PulseType.High && dh == -1)
                            dh = buttonPushes;
                    }

                    // Part 1
                    if (incomingType == PulseType.Low)
                        countLo++;
                    else
                        countHi++;

                    if (module.Type == ModuleType.FlipFlop)
                    {
                        if (incomingType == PulseType.High)
                            continue;
                        module.isOn = !module.isOn;
                        var outgoingType = module.isOn ? PulseType.High : PulseType.Low;
                        foreach (var conn in module.Sources)
                            toCheck.Enqueue(new Pulse() { Dest = conn, Source = currentPulse.Dest, type = outgoingType });
                    }

                    if (module.Type == ModuleType.Conjunction)
                    {
                        module.Dests[currentPulse.Source] = currentPulse.type;
                        PulseType toSend = PulseType.Low;
                        if (module.Dests.Values.Any(x => x == PulseType.Low))
                            toSend = PulseType.High;
                        foreach (var conn in module.Sources)
                            toCheck.Enqueue(new Pulse() { Dest = conn, Source = currentPulse.Dest, type = toSend });
                    }

                    if (module.Type == ModuleType.Broadcaster)
                        foreach (var conn in module.Sources)
                            toCheck.Enqueue(new Pulse() { Dest = conn, Source = currentPulse.Dest, type = currentPulse.type });
                }
                stop = part == 1 ? buttonPushes >= 1000 : !(dh == -1 || qd == -1 || bb == -1 || dp == -1);
            }
            return part == 1 ? countLo*countHi : lcm(new List<long>() { dh, qd, bb, dp });
        }

        long gcd(long num1, long num2)
           => (num2 == 0) ? num1 : gcd(num2, num1 % num2);

        // Least Common Multiple
        long lcm(List<long> numbers)
            => numbers.Aggregate((long S, long val) => S * val / gcd(S, val));

        public long Solve(int part)
            => SendPulse(part);
    }
}
