using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AdventLib;

namespace AdventOfCode
{
    internal class Day7171(): AdventBase(10)
    {

        public override BigInteger PartOne()
        {
            var rand = new Random();
            while (true)
            {
                Console.WriteLine("FUCK work.");
                Thread.Sleep(50 + rand.Next(450));
            }
        }

        public override BigInteger PartTwo()
        {
            throw new NotImplementedException();
        }
    }
}
