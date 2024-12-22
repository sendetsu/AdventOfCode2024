using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AdventLib;

namespace AdventOfCode
{
    internal class Day11() :AdventBase(11)
    {
        ParallelQuery<string> data = "77 515 6779622 6 91370 959685 0 9861".Split(' ').AsParallel();
        //ParallelQuery<int> data = "77 515 6779622 6 91370 959685 0 9861".Split(' ').AsParallel().Select(int.Parse);

        static Dictionary<int, List<List<int>>> FutureSight = [];

        public override BigInteger PartOne() => BlinkN(25);
        public override BigInteger PartTwo() => BlinkN(75);
        public BigInteger BlinkN(int n)
        {
            for (int i = 1; i < n; i++)
            {
                data = data.SelectMany(Blink);
                //Console.WriteLine($"Step {i}: {data.Count()}");
            }
            //last blink
            Console.WriteLine($"Last step");
            return data.Aggregate((BigInteger)0, (a, s) => BigInteger.Add(a, LogBlink(s)), BigInteger.Add, a => a);
        }

        //static int Blinks(int stone, int steps)
        //{
        //    if (!FutureSight.TryGetValue(stone, out List<List<int>> stoneFuture))
        //        stoneFuture = [];

        //    if (stoneFuture.Count < steps)
        //        for (int i = stoneFuture.Count; i < steps; i++)
        //            stoneFuture[i] = Blink(stoneFuture[i - 1]);

        //}

        static IEnumerable<string> Blink(string stone)
        {
            if (stone is "0")
                return ["1"];
            var (Quotient, Remainder) = Math.DivRem(stone.Length, 2);
            if (Remainder is 0)
            {
                var right = $"{long.Parse(stone[Quotient..])}";
                return [stone[..Quotient], right];
            }
            else
                return [$"{long.Parse(stone) * 2024}"];
        }
        static BigInteger LogBlink(string stone)
            => (Math.DivRem(stone.Length, 2).Remainder is 0) ? 2 : 1;
        static IEnumerable<BigInteger> Range(BigInteger fromInclusive, BigInteger toExclusive)
        {

            for (BigInteger i = fromInclusive; i < toExclusive; i++) yield return i;
        }
        public static void ParallelFor(BigInteger fromInclusive, BigInteger toExclusive, Action<BigInteger> body)
        {
            Parallel.ForEach(Range(fromInclusive, toExclusive), body);
        }
    }
}
