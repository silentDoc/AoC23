using AoC23.Common;
using Microsoft.Z3;

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

        // Part 1
        int FindCrossings()
        {
            int count = 0;
            for (var i = 0; i < Stones.Count - 1; i++)
                for (var j = i + 1; j < Stones.Count; j++)
                    if (Crossing(Stones[i], Stones[j]))
                        count++;
                
            return count;
        }

        // Part 2 - Advent of code 2018 Day 23 was super helpful for this one
        // Constraint Programming :)

        // Helpers to get the solver Equations
        BoolExpr CrossingEquation(ArithExpr p, ArithExpr v, ArithExpr t, long hp, long hv, Context ctx)
        {
            // x + vx * t == px + pvx * t
            // y + vy * t == py + pvy * t  -- etc ...

            ArithExpr MulLeft = ctx.MkMul(v, t);
            ArithExpr left = ctx.MkAdd(p, MulLeft);
            ArithExpr MulRight = ctx.MkMul(ctx.MkInt(hv), t);
            ArithExpr right = ctx.MkAdd(ctx.MkInt(hp), MulRight);

            return ctx.MkEq(left, right);
        }

        private long SolvePart2()
        {
            Context ctx = new Context();
            ArithExpr x = ctx.MkIntConst("x");
            ArithExpr y = ctx.MkIntConst("y");
            ArithExpr z = ctx.MkIntConst("z");

            ArithExpr vx = ctx.MkIntConst("vx");
            ArithExpr vy = ctx.MkIntConst("vy");
            ArithExpr vz = ctx.MkIntConst("vz");

            Solver solver = ctx.MkSolver();

            for (int i = 0; i < Stones.Count; i++)
            {
                var (hpx, hpy, hpz) = Stones[i].position;
                var (hvx, hvy, hvz) = Stones[i].velocity;

                ArithExpr t = ctx.MkIntConst("t_"+i);
                solver.Add(ctx.MkGe(t, ctx.MkInt(0)));      // t >=0
                solver.Add(CrossingEquation(x, vx, t, hpx, hvx, ctx));  // x + vx * t == px + pvx * t
                solver.Add(CrossingEquation(y, vy, t, hpy, hvy, ctx));  // y + vy * t == py + pvy * t
                solver.Add(CrossingEquation(z, vz, t, hpz, hvz, ctx));  // z + vz * t == pz + pvz * t
            }

            var status = solver.Check();
            var model = solver.Model;

            var resX = model.Evaluate(x);
            var resY = model.Evaluate(y);
            var resZ = model.Evaluate(z);

            long xx = long.Parse(resX.ToString());
            long yy = long.Parse(resY.ToString());
            long zz = long.Parse(resZ.ToString());

            return xx + yy + zz;
        }

        public long Solve(int part = 1)
            => part == 1 ? FindCrossings() : SolvePart2();
    }
}
