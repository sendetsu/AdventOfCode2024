using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AdventLib;

namespace AdventOfCode
{
    internal class Day12 : AdventBase
    {

        List<Region> Regions { get; set; } = [];

        public Day12() : base(12)
        {
            UseExample();
            List<Tile> Tiles = [..Enumerable.Range(0, XLength).SelectMany(x =>
                                Enumerable.Range(0, YLength).Select(y =>
                                    new Tile(x, y, DataMatrix[x, y])))];
            foreach (var tile in Tiles.AsParallel())
            {
                if (Tiles.FirstOrDefault(t => (t.X, t.Y) == (tile.X, tile.Y - 1)) is Tile northTile)
                    tile.Neighbours.Add(northTile);
                if (Tiles.FirstOrDefault(t => (t.X, t.Y) == (tile.X + 1, tile.Y)) is Tile westTile)
                    tile.Neighbours.Add(westTile);
                if (Tiles.FirstOrDefault(t => (t.X, t.Y) == (tile.X, tile.Y + 1)) is Tile southTile)
                    tile.Neighbours.Add(southTile);
                if (Tiles.FirstOrDefault(t => (t.X, t.Y) == (tile.X - 1, tile.Y)) is Tile eastTile)
                    tile.Neighbours.Add(eastTile);
            }

            do
            {
                var tile = Tiles.First();
                var region = new Region();
                Regions.Add(region);
                IEnumerable<Tile> newPlants = [tile];
                while (newPlants.Any())
                {
                    region.IncludedTiles.AddRange(newPlants);
                    foreach (var plant in newPlants)
                        Tiles.Remove(plant);
                    newPlants = region.IncludedTiles.SelectMany(t => t.PlantNeighbours.Where(x => !region.IncludedTiles.Contains(x))).Distinct().ToList();
                }
            } while (Tiles.Count > 0);
        }

        public override BigInteger PartOne() => Regions.Sum(r => r.Price);

        public override BigInteger PartTwo() => Regions.Sum(r => r.BulkPrice);


        class Region
        {
            internal List<Tile> IncludedTiles { get; } = [];
            int Area => IncludedTiles.Count;
            int Perimeter => 4 * Area - IncludedTiles.Sum(x => x.PlantNeighbours.Count());
            int NbSides => 0; // TODO part 2
            internal int Price => Area * Perimeter;
            internal int BulkPrice => Area * NbSides;
        }
        class Tile(int x, int y, char plant)
        {
            internal int X { get; } = x;
            internal int Y { get; } = y;
            internal char Plant { get; } = plant;

            internal List<Tile> Neighbours { get; set; } = [];
            internal IEnumerable<Tile> PlantNeighbours => Neighbours.Where(n => n.Plant == Plant);

            internal int UnsharedBorders => OtherNeighbours.Where(x => !x.Neighbours.Any(o => PlantNeighbours.SelectMany(p => p.OtherNeighbours).Contains(o))).Count();
            internal IEnumerable<Tile> OtherNeighbours => Neighbours.Where(n => n.Plant != Plant);
        }


        void UseExample()
        {
            Data = """
                   RRRRIICCFF
                   RRRRIICCCF
                   VVRRRCCFFF
                   VVRCCCJFFF
                   VVVVCJJCFE
                   VVIVCCJJEE
                   VVIIICJJEE
                   MIIIIIJJEE
                   MIIISIJEEE
                   MMMISSJEEE
                   """;
        }
    }
}
