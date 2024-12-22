using System.Numerics;
using AdventLib;
namespace AdventOfCode
{
    internal class Day7 : AdventBase
    {
        readonly List<long[]> Equations;
        public Day7() : base(7)
            => Equations = [..Data.Split(Environment.NewLine).Select(line => line.Replace(":", "").Split(' ').Select(long.Parse).ToArray())];
        public override BigInteger PartOne() => Equations.Sum(x => CheckEquation(x));
        public override BigInteger PartTwo() => Equations.Sum(x => CheckEquation(x, 3));

        #region Operators
        readonly List<Func<long, long, long>> Operators =
        [
            (op1,op2) => op1 + op2,
            (op1,op2) => op1 * op2,
            (op1,op2) => long.Parse(op1.ToString() + op2.ToString())
        ];
        #endregion

        long CheckEquation(long[] equation, int @base = 2)
        {
            for (var o = 0; o < Math.Pow(@base, equation.Length - 2); o++)
            {
                var total = equation[1];
                for (var i = 0; i < equation.Length - 2; i++)
                    total = Operators[Math.DivRem(o, (int)Math.Pow(@base, i)).Quotient % @base](total, equation[i + 2]);
                if (total == equation[0])  return equation[0];
            }
            return 0;
        }
    }
}
