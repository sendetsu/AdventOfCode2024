using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventLib;
using AdventOfCode;

int Day1A(IEnumerable<int> list1, IEnumerable<int> list2) => list1.Order().Zip(list2.Order(), (first, second) => Math.Abs(first - second)).Sum();
int Day1B(IEnumerable<int> list1, IEnumerable<int> list2)
    => list2.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count()) switch
    { var dic => list1.Select(x => dic.TryGetValue(x, out int value) ? x * value : 0).Sum()};

int Day2A(IEnumerable<IEnumerable<int>> input) => input.CompareZip().Where(x => x.IsMonotone(3)).Count();
int Day2B(IEnumerable<IEnumerable<int>> input) => input.Select(x => x.ExpandAndLeaveN().CompareZip().Any(x => x.IsMonotone(3))).Count(x => x);

Regex D3 = new(@"mul\((\d{1,3}),(\d{1,3})\)");
Regex D3B = new(@"(?<do>do\(\))|(?<dont>don't\(\))|mul\((?<x>\d{1,3}),(?<y>\d{1,3})\)");
int Day3A(string input) => D3.Matches(input).Sum(m => m.Groups.Values.Skip(1).Select(g => int.Parse(g.Value)).Aggregate(1, (x, y) => x * y));
int Day3B(string input)
 => D3B.Matches(input)
        .OrderBy(m => m.Index)
        .Aggregate((Sum: 0, Enabled: true), (state, m) => state switch
            {
                _ when m.Groups["do"].Success => (state.Sum, true),
                _ when m.Groups["dont"].Success => (state.Sum, false),
                { Enabled: true } => (state.Sum + int.Parse(m.Groups["x"].Value) * int.Parse(m.Groups["y"].Value), true),
                _ => state,
            }).Sum;

int Day4A(string input, string text)
{
    var mods = Enumerable.Range(-1, 3);
    Dictionary<(int x, int y), char> data = input.Split(Environment.NewLine).SelectMany((line, y) => line.Select((c, x) => (x, y, c))).ToDictionary(t => (t.x, t.y), t => t.c);
    return data.Where(t => t.Value == text[0])
                .Sum(t =>
                    mods.Sum(mx =>
                        mods.Select(my =>
                            text.Select((x,i) => (x,i)).Aggregate(new int[3,3], (s, c) => {
                                s[mx+1,my+1] += data.TryGetValue((t.Key.x + mx * c.i, t.Key.y + my * c.i), out char value) && value == c.x ? 1 : 0;
                                return s;
                            })
                        ).SelectMany(a => a.ToEnumerable()).Count(a => a == text.Length))
                );
}

int Day4B(string input)
{
    Dictionary<(int x, int y), char> data = input.Split(Environment.NewLine).SelectMany((line, x) => line.Select((c, y) => (x, y, c))).ToDictionary(t => (t.x, t.y), t => t.c);
    return data.Count(t => t.Value == 'A'
                        && data.TryGetValue((t.Key.x + 1, t.Key.y + 1), out char value)
                        && data.TryGetValue((t.Key.x - 1, t.Key.y - 1), out char value2)
                        && data.TryGetValue((t.Key.x + 1, t.Key.y - 1), out char value3)
                        && data.TryGetValue((t.Key.x - 1, t.Key.y + 1), out char value4)
                        && (value, value2) is ('M', 'S') or ('S', 'M')
                        && (value3, value4) is ('M', 'S') or ('S', 'M'));
}

int Day5A(string input) => (new Day5(input)).PartOne;
int Day5B(string input) => (new Day5(input)).PartTwo;

Console.Write("What Day (and part)? ");
var (Day, Part) = (0, 0);
Regex lineReg = new(@"^(?<day>\d+)(?<part>[AB]?)");
do
{
    var line = Console.ReadLine();
    if (line is { Length: > 0 })
    {
        var matches = lineReg.Match(line);
        Day = int.Parse(matches.Groups["day"].Value);
        Part = matches.Groups["part"].Value switch { "A" => 1, "B" => 2, _ => 0 };
    }
} while ((Day, Part) is (0,0));
    var dayObj = Activator.CreateInstance(Type.GetType($"AdventOfCode.Day{Day}"));

    if (Part is 0 or 1)
        Console.WriteLine($"Day {Day} Part 1 ({Part}) : {(dayObj as AdventBase)?.PartOne()}");
    if (Part is 0 or 2)
        Console.WriteLine($"Day {Day} Part 2 ({Part}) : {(dayObj as AdventBase)?.PartTwo()}");

[Flags]
enum MyFlagEnum
{
    None = 0,
    First = 0b001,
    Second = 0b010,
    Third = 0b100,
    IdkSomething = First | Second,
    All = IdkSomething | Third,
}


static class Ext
{
    internal static IEnumerable<IEnumerable<int>> CompareZip(this IEnumerable<IEnumerable<int>> col)
        => col.Select(x => x.Zip(x.Skip(1), (a, b) => b - a));

    internal static IEnumerable<IEnumerable<T>> ExpandAndLeaveN<T>(this IEnumerable<T> col, int amount = 1)
        => col.Select((_, i) => col.Take(i).Concat(col.Skip(i + amount).Take(col.Count() - i - amount)));

    /// <summary>
    /// Determines whether all elements of an integer sequence are ordered and each within <paramref name="maxStep"/> of its neighbours
    /// </summary>
    /// <param name="col"></param>
    /// <param name="maxStep"></param>
    /// <returns></returns>
    internal static bool IsMonotone(this IEnumerable<int> col, int maxStep = 1)
        => col.All(d => d > 0 && d < (maxStep + 1)) || col.All(d => d > - (maxStep + 1) && d < 0);
    internal static IEnumerable<T> ToEnumerable<T>(this T[,] target)
    {
        foreach (var item in target)
            yield return (T)item;
    }
}