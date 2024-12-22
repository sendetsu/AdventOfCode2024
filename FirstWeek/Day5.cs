using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Day5 : Comparer<int>
    {
        IEnumerable<Rule> Rules { get; }
        IEnumerable<int[]> UpdateBatches { get; }
        IEnumerable<int[]> GoodBatches => UpdateBatches.Where(x => Enumerable.SequenceEqual(x.Order(this), x));
        IEnumerable<int[]> BadBatches => UpdateBatches.Where(x => !Enumerable.SequenceEqual(x.Order(this), x));

        public int PartOne => GoodBatches
            .Select(x => x.ElementAt(Math.DivRem(x.Length, 2, out _)))
            .Sum();
        public int PartTwo => BadBatches
            .Select(x => x.Order(this))
            .Select(x => x.ElementAt(Math.DivRem(x.Count(), 2, out _)))
            .Sum();

        public Day5(string data)
        {
            var parts = data.Split(Environment.NewLine + Environment.NewLine).Select(x => x.Split(Environment.NewLine));
            Rules = parts.ElementAt(0).Select(x => x.Split('|').Select(x => int.Parse(x))).Select(x => new Rule(x.ElementAt(0), x.ElementAt(1)));
            UpdateBatches = parts.ElementAt(1).Select(x => x.Split(',')).Select(x => x.Select(y => int.Parse(y)).ToArray());
        }

        public override int Compare(int x, int y)
        {
            if (Rules.Any(r => r.FirstUpdateId == x && r.SecondUpdateId == y))
                return -1;
            else
                return 1;
        }

        record Rule(int FirstUpdateId, int SecondUpdateId);

    }
}
