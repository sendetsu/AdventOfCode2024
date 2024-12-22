using System.Linq;
using System.Numerics;
using AdventLib;

namespace AdventOfCode
{
    internal class Day10 : AdventBase
    {
        List<Tile> Tiles { get; set; } = [];

        public Day10() : base(10)
        {
            Tiles.AddRange(Enumerable.Range(0, XLength).SelectMany(x =>
                                Enumerable.Range(0, YLength).Select(y =>
                                    new Tile(x, y, int.Parse($"{DataMatrix[x, y]}")))));
            foreach (var tile in Tiles.AsParallel()) // Just bruteforcing it zzz
                tile.Upwards.AddRange(Tiles.Where(x => (Math.Abs(tile.X - x.X) + Math.Abs(tile.Y - x.Y)) is 1 && x.Elevation == tile.Elevation + 1));
        }

        public override BigInteger PartOne() => Tiles.Where(x => x.Elevation is 0).Sum(x => x.NbSummits);
        public override BigInteger PartTwo() => Tiles.Where(x => x.Elevation is 0).Sum(x => x.NbPaths);
        class Tile(int X, int Y, int Elevation)
        {
            internal int X { get; } = X;
            internal int Y { get; } = Y;
            internal int Elevation { get; } = Elevation;
            internal List<Tile> Upwards { get; set; } = [];

            IEnumerable<Tile>? summits;
            int? paths;

            internal IEnumerable<Tile> ReachableSummits => summits ??= Elevation is 9 ? [this] : Upwards.SelectMany(x => x.ReachableSummits).Distinct();
            internal int NbSummits => ReachableSummits.Count();
            internal int NbPaths => paths ??= Elevation is 9 ? 1 : Upwards.Sum(x => x.NbPaths);
        }
    }


}
