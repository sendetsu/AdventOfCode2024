using System.Numerics;
using System.Text.RegularExpressions;
using AdventLib;

namespace AdventOfCode
{
    internal class Day13() : AdventBase(13)
    {

        Regex Regex = new("""
                Button A: X\+(?<Ax>\d+), Y\+(?<Ay>\d+)
                Button B: X\+(?<Bx>\d+), Y\+(?<By>\d+)
                Prize: X=(?<Px>\d+), Y=(?<Py>\d+)
            """, RegexOptions.Compiled);

        public override BigInteger PartOne()
        {
            throw new NotImplementedException();
        }

        public override BigInteger PartTwo()
        {
            throw new NotImplementedException();
        }
    }
}
