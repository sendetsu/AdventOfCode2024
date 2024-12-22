using System.Numerics;
using System.Reflection;

namespace AdventLib
{
    public abstract class AdventBase
    {
        static Dictionary<int, AdventBase> Days { get; set; } = [];
        int Day { get; }
        static readonly string BasePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "Inputs");
        protected string? Data;
        IEnumerable<string> DataLines => Data?.Split(Environment.NewLine).SelectMany(s => s.Split('\n')) ?? [];

        private Dictionary<(int X, int Y), char> charData = [];
        protected Dictionary<(int X, int Y), char> DataAsCharCoords
        {
            get
            {
                if (charData.Count == 0 && !string.IsNullOrEmpty(Data))
                {
                    charData = DataLines
                        .SelectMany((line, y) => line.Select((c, x) => (x, y, c)))
                        .ToDictionary(t => (t.x, t.y), t => t.c);
                }
                return charData;
            }
        }

        protected char[,]? dataMatrix;

        public AdventBase(int Day)
        {
            this.Day = Day;
            Data = LoadInput(Day);
            Days.Add(Day, this);
        }

        protected char[,] DataMatrix
        {
            get
            {
                if (dataMatrix is null && !string.IsNullOrEmpty(Data))
                {
                    var dataDic = DataLines
                        .SelectMany((line, y) => line.Select((c, x) => (x, y, c)))
                        .ToDictionary(t => (t.x, t.y), t => t.c);
                    dataMatrix = CreateMatrix(dataDic);
                }
             return dataMatrix;
            }
        }

        protected int XLength => DataMatrix.GetLength(0);
        protected int YLength => DataMatrix.GetLength(1);

        protected char? FindData(Func<char, bool> checkValue, out int x, out int y)
        {
            for (int i = 0; i < DataMatrix.GetLength(0); i++)
                for (int j = 0; j < DataMatrix.GetLength(1); j++)
                    if (checkValue(DataMatrix[i, j]))
                    {
                        x = i;
                        y = j;
                        return DataMatrix[i, j];
                    }
            (x, y) = (0, 0);
            return null;
        }

        protected static char[,] CreateMatrix (Dictionary<(int x, int y), char> dataDic)
        {
            var matrix = new char[dataDic.Max(d => d.Key.x) + 1, dataDic.Max(d => d.Key.y) + 1];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] =
                        dataDic.TryGetValue((i, j), out char value) ? value : ' ';
                }
            }
            return matrix;
        }

        public static string? LoadInput(int day, string? part = null)
        {
            var filename = $"Day{day}" + (part is not null ? $"_{part}" : string.Empty) + ".txt";
            string? data = null;
            var filepath = Path.Combine(BasePath, filename);
            if (File.Exists(filepath))
                data = File.ReadAllText(filepath);
            return data;
        }

        protected void LogCoordinateState(char[]? filter = null)
        {
            var matrix = DataAsCharCoords.OrderBy(x => x.Key.X).GroupBy(x => x.Key.Y).OrderBy(x => x.Key);

            foreach (var y in matrix)
            {
                foreach (var x in y)
                    if(filter is null || filter.Contains(x.Value))
                        Console.Write(x.Value);
                    else
                        Console.Write(' ');
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        protected void LogMatrixState(char[]? filter = null)
        {
            for (int j = 0; j < DataMatrix.GetLength(1); j++) { 
                for (int i = 0; i < DataMatrix.GetLength(0); i++)
                    if (filter is null || filter.Contains(DataMatrix[i, j]))
                        Console.Write(DataMatrix[i,j]);
                    else
                        Console.Write(' ');
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public abstract BigInteger PartOne();
        public abstract BigInteger PartTwo();

        public override string ToString() => $"Data is {Data?.Length} characters long";
    }
}
