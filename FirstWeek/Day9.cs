using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AdventLib;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode
{
    internal class Day9 : AdventBase
    {
        readonly Dictionary<int, int?> Blocks = [];

        readonly List<(int? id, IEnumerable<int> keys)> IndexedData = [];
        public Day9() : base(9)
        {
            int idx = 0, key = 0;
            foreach (var (d, i) in Data.Select((d, i) => ((d - '0'), i)))
            {
                var id = i % 2 is 1 ? null : (int?)idx++;

                List<int> zz = [];
                foreach (var digit in Enumerable.Range(0, d).Select(_ => id))
                {
                    zz.Add(key);
                    Blocks[key++] = digit;
                }
                IndexedData.Add((id, zz));
            }
        }

        public override BigInteger PartOne()
        {
            List<int> consolidatedBlocks = [];

            while (Blocks.Any(x => x.Value is null))
            {
                var blocks = Blocks.OrderBy(x => x.Key);
                var firstEmptyBlock = blocks.First(x => x.Value is null);
                var lastFullBlock = blocks.Last(x => x.Value is not null);

                Blocks[firstEmptyBlock.Key] = lastFullBlock.Value;
                Blocks.Remove(lastFullBlock.Key);
            }
            return consolidatedBlocks.Select((n, i) => (long)(n * i)).Sum();
        }

        public override BigInteger PartTwo()
        {
            var filledBlocks = IndexedData.Where(x => x.id != null).Reverse();
            foreach (var (id, keys) in filledBlocks)
            {
                var blocks = Blocks;
                int? startingKey = blocks.First(x => x.Value is null).Key;
                while (startingKey != null && startingKey < keys.Min())
                {
                    var emptySpots = blocks.Skip(startingKey.Value).TakeWhile(x => x.Value is null);
                    if (emptySpots.Count() >= keys.Count())
                    {
                        var spots = emptySpots.ToArray();
                        for (int i = 0; i < keys.Count(); i++)
                        {
                            Blocks[spots[i].Key] = id;
                            Blocks[keys.ElementAt(i)] = null;
                        }
                        break;
                    }
                    startingKey = blocks.Skip(startingKey.Value).SkipWhile(x => x.Value is null).SkipWhile(x => x.Value != null).Select(x => (int?)x.Key).FirstOrDefault();
                };
            }
            return Blocks.Select((n, i) => (long)((n.Value ?? 0) * i)).Sum();
        }

        void Log()
        {
            foreach (var (_, value) in Blocks)
                Console.Write(value is null ? "." : $"{value}");
            Console.WriteLine();
        }
    }
}
