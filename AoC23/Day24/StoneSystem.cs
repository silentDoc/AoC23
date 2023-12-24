using AoC23.Common;

namespace AoC23.Day24
{
    public record HailStone
    {
        public Coord3DL position;
        public Coord3DL velocity;

        public HailStone(string input)
        { 
            var vs = input.Split('@', ',').Select(long.Parse).ToList();
            position = (vs[0], vs[1], vs[2]);
            velocity = (vs[3], vs[4], vs[5]);
        }
    }

    internal class StoneSystem
    {
        List<HailStone> Stones = new();

        long TEST_MIN = 200000000000000;
        long TEST_MAX = 400000000000000;


        public void ParseInput(List<string> lines)
            => lines.ForEach(x => Stones.Add(new HailStone(x)));

        bool Crossing(HailStone stone1, HailStone stone2)
        {
            // Refresh maths :) https://www.cuemath.com/geometry/intersection-of-two-lines/
            // Crossing of thwo lines : 
            // a1x + b1y + c1 = 0
            // a2x + b2y + c1 = 0
            // Get slopes - and cross product rule

            var (p0x, p0y) = (stone1.position.x, stone1.position.y);
            var (p1x, p1y) = (stone2.position.x, stone2.position.y);
            var (v0x, v0y) = (stone1.velocity.x, stone1.velocity.y);
            var (v1x, v1y) = (stone2.velocity.x, stone2.velocity.y);

            float slope0 = ((float) v0y) / ((float) v0x);
            float slope1 = ((float) v1y) / ((float) v1x);

            if (slope0 == slope1)
                return false;   // Parallel or divide by 0

            var crossX = ((slope1 * p1x) - (slope0 * p0x) + p0y - p1y) / (slope1 - slope0);
            var crossY = (slope0 * (crossX - p0x)) + p0y;

            var passTest = crossX >= TEST_MIN && crossX <= TEST_MAX && crossY >= TEST_MIN && crossY <= TEST_MAX;
            if(!passTest) 
                return false;

            // Last - is it future ? 
            var isFutureX0 = (v0x < 0) ? crossX < p0x : crossX >= p0x;
            var isFutureY0 = (v0y < 0) ? crossY < p0y : crossY >= p0y;
            var isFutureX1 = (v1x < 0) ? crossX < p1x : crossX >= p1x;
            var isFutureY1 = (v1y < 0) ? crossY < p1y : crossY >= p1y;

            return isFutureX0 && isFutureY0 && isFutureX1 && isFutureY1;
        }


        int FindCrossings()
        {
            int count = 0;
            for (var i = 0; i < Stones.Count - 1; i++)
                for (var j = i + 1; j < Stones.Count; j++)
                    if (Crossing(Stones[i], Stones[j]))
                        count++;
                
            return count;
        }

        public int Solve(int part = 1)
            => FindCrossings();
    }
}
