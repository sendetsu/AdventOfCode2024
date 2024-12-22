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
    internal class Day8 : AdventBase
    {
        ConcurrentBag<(int x, int y)> Antinodes { get; set; } = [];
        int MaxX, MaxY;

        public Day8() : base(8)
        {
            MaxX = DataAsCharCoords.Max(x => x.Key.X);
            MaxY = DataAsCharCoords.Max(x => x.Key.Y);
        }

        public override BigInteger PartOne()
        {
            foreach (var group in DataAsCharCoords.GroupBy(x => x.Value, x => x.Key).Where(x => x.Key is not '.'))
            {
                var type = group.Key; //Useless tho

                foreach(var i in group)
                    foreach (var j in group)
                    {
                        if (i == j)
                            continue;

                        var x1 = 2*(i.X - j.X) + j.X;
                        var y1 = 2 * (i.Y - j.Y) + j.Y;
                        if (x1 >= 0 && y1 >= 0 && x1 <= MaxX && y1 <= MaxY)
                            Antinodes.Add((x1, y1));
                        var x2 = 2*(j.X - i.X) + i.X;
                        var y2 = 2 * (j.Y - i.Y) + i.Y;
                        if (x2 >= 0 && y2 >= 0 && x2 <= MaxX && y2 <= MaxY)
                            Antinodes.Add((x2, y2));
                    }
            }
            foreach (var a in Antinodes.Distinct())
                DataAsCharCoords[a] = '-';
            LogCoordinateState();
            return Antinodes.Distinct().Count();
        }

        public override BigInteger PartTwo()
        {
            foreach (var group in DataAsCharCoords.GroupBy(x => x.Value, x => x.Key).Where(x => x.Key is not '.'))
            {
                var type = group.Key; //Useless tho

                foreach (var i in group)
                    foreach (var j in group)
                    {
                        if (i == j)
                            continue;
                        var dX = i.X - j.X;
                        var dY = i.Y - j.Y;
                        for (var pos = (x: j.X, y: j.Y); (dX > 0 ? pos.x <= MaxX : pos.x >= 0) && (dY > 0 ? pos.y <= MaxY : pos.y >= 0); pos = (pos.x + dX, pos.y + dY))
                            if (pos.x >= 0 && pos.x <= MaxX && pos.y >= 0 && pos.y <= MaxY)
                                Antinodes.Add(pos);
                        for (var pos = (x: j.X, y: j.Y); (dX < 0 ? pos.x <= MaxX : pos.x >= 0) && (dY < 0 ? pos.y <= MaxY : pos.y >= 0); pos = (pos.x - dX, pos.y - dY))
                            if (pos.x >= 0 && pos.x <= MaxX && pos.y >= 0 && pos.y <= MaxY)
                                Antinodes.Add(pos);
                        var dX2 = i.X - j.X;
                        var dY2 = i.Y - j.Y;
                        for (var pos = (x: i.X, y: i.Y); (dX2 > 0 ? pos.x <= MaxX : pos.x >= 0) && (dY2 > 0 ? pos.y <= MaxY : pos.y >= 0); pos = (pos.x + dX2, pos.y + dY2))
                            if (pos.x >= 0 && pos.x <= MaxX && pos.y >= 0 && pos.y <= MaxY)
                                Antinodes.Add(pos);
                        for (var pos = (x: i.X, y: i.Y); (dX2 < 0 ? pos.x <= MaxX : pos.x >= 0) && (dY2 < 0 ? pos.y <= MaxY : pos.y >= 0); pos = (pos.x - dX2, pos.y - dY2))
                            if (pos.x >= 0 && pos.x <= MaxX && pos.y >= 0 && pos.y <= MaxY)
                                Antinodes.Add(pos);
                    }
            }
            foreach (var a in Antinodes.Distinct())
                DataAsCharCoords[a] = '-';
            LogCoordinateState();
            return Antinodes.Distinct().Count();
        }
    }
}
