using System.Collections.Concurrent;
using System.Diagnostics;
using System.Numerics;
using AdventLib;

namespace AdventOfCode
{
    internal class Day6 : AdventBase
    {

        static readonly Dictionary<char, Move> Moves = new()
        {
            ['^'] = new(0, -1, '^'),
            ['>'] = new(1, 0, '>'),
            ['v'] = new(0, 1, 'v'),
            ['<'] = new(-1, 0, '<'),
        };

        static readonly HashSet<(int x, int y)> Positions = [];

        private List<char>[,] OldMoves;
        private Move CurrentMove { get; set; }

        int CurrentX {  get; set; }
        int CurrentY { get; set; }

        int NextX => CurrentX + CurrentMove.Dx;
        int NextY => CurrentY + CurrentMove.Dy;
        char CurrentDirection => CurrentMove.Direction;

        public List<(int x, int y)> Obstacles { get; } = [];

        public Day6() : base(6)
        {
            var startingPoint = FindData(Moves.ContainsKey, out int x, out int y);
            if (startingPoint is not null)
            {
                CurrentX = x;
                CurrentY = y;
                CurrentMove = Moves[startingPoint.Value];
            }
        }

        public Day6(int x, int y, char direction, char[,] currentState) : base(6)
        {
            dataMatrix = currentState;
            OldMoves = new List<char>[dataMatrix.GetLength(0), dataMatrix.GetLength(1)];
            for (int j = 0; j < OldMoves.GetLength(1); j++)
                for (int i = 0; i < OldMoves.GetLength(0); i++)
                    OldMoves.SetValue(new List<char>(), i, j);
            CurrentX = x;
            CurrentY = y;
            CurrentMove = Moves[direction];
        }

        char[,] CopyMatrix(in char[,] currentState)
        {
            var matrix = new char[currentState.GetLength(0), currentState.GetLength(1)];
            for (int i = 0; i < currentState.GetLength(0); i++)
            {
                for (int j = 0; j < currentState.GetLength(1); j++)
                {
                    matrix[i, j] = currentState[i, j];
                }
            }
            return matrix;
        }

        public void DoAll()
        {
            while (Next()) ;
        }

        public override BigInteger PartOne()
        {
            DoAll();
            return DataMatrix.ToEnumerable().Count(Moves.ContainsKey);
        }
        public override BigInteger PartTwo()
        {
            Next();
            List<(int X, int Y, char, char[,])> tests = [];
            while (Next()) {
                if (NextIsOutOfBound)
                    break;
                if (NextDirection is not '#' or 'O')
                    tests.Add((CurrentX, CurrentY, CurrentDirection, CopyMatrix(DataMatrix)));
                    tests.Last().Item4[NextX, NextY] = 'O';
            }
            var sw = Stopwatch.StartNew();

            ConcurrentBag<(int x, int y)> PotentialBlocades = [];
            foreach (var t in tests.AsParallel())
            {
                var (X, Y, dir, data) = t;
                var subData = new Day6(X, Y, dir, data);
                try
                {
                    subData.DoAll();
                }
                catch (IsLoopingException)
                {
                    var Obstacle = (X + Moves[dir].Dx, Y + Moves[dir].Dy);
                    PotentialBlocades.Add(Obstacle);
                }
            };
            Console.WriteLine($"Ended in ({sw.Elapsed}s)");
            foreach (var (x, y) in PotentialBlocades)
                DataMatrix[x, y] = 'O';
            LogMatrixState();
            return PotentialBlocades.Distinct().Count();
        }

        bool NextIsOutOfBound => NextX < 0 || NextX >= DataMatrix.GetLength(0) || NextY < 0 || NextY >= DataMatrix.GetLength(1);
        char? NextDirection => NextIsOutOfBound ? null : DataMatrix[NextX, NextY];

        bool Next()
        {
            while(!NextIsOutOfBound)
            {
                if (NextDirection is '#' or 'O')
                        TurnRight();
                else
                {
                    if (OldMoves is not null)
                        if (OldMoves[CurrentX, CurrentY].Contains(CurrentDirection))
                            throw new IsLoopingException();
                        else
                            OldMoves[CurrentX, CurrentY].Add(CurrentDirection);
                    DataMatrix[CurrentX, CurrentY] = CurrentDirection;
                    CurrentX = NextX;
                    CurrentY = NextY;
                    return true;
                }
            }
            return false;
        }
        void TurnRight()
            => CurrentMove = Moves[CurrentMove.NextDirection];

        class Move(int Dx, int Dy, char Direction)
        {
            static readonly Dictionary<char, char> TurnDirections = new()
            {
                ['^'] = '>',
                ['>'] = 'v',
                ['v'] = '<',
                ['<'] = '^',
            };

            public int Dx { get; } = Dx;
            public int Dy { get; } = Dy;
            public char Direction { get; } = Direction;
            public char NextDirection { get; } = TurnDirections[Direction];
        }

        class IsLoopingException() : Exception { }
    }
}

