using System;

namespace Code.Match3
{ 
    public struct Match3Grid<T>
    {
        public ref T this[int x, int y] => ref _shapes[x, y];

        public readonly int sizeX => _shapes.GetLength(0);
        public readonly int sizeY => _shapes.GetLength(1);
        
        private readonly T[,] _shapes;

        public Match3Grid(T [,] shapes)
        {
            _shapes = shapes;
        }

        public bool IsValid(ShapePos pos) => 
            0 <= pos.x && pos.x < sizeX && 0 <= pos.y && pos.y < sizeY;
        
        public void Swap(ShapePos a, ShapePos b)
        {
            string err = $"[{nameof(Match3Grid<T>)}] Swap failed, coordinates \"[coordinates]\" less than zero or more than grid size!";
            try
            {
                if (IsValid(a) == false) 
                    throw new IndexOutOfRangeException(err.Replace("[coordinates]", $"{a.x}, {a.y}"));
                
                if (IsValid(b) == false) 
                    throw new IndexOutOfRangeException(err.Replace("[coordinates]", $"{b.x}, {b.y}"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            (this[a.x, a.y], this[b.x , b.y]) = (this[b.x, b.y], this[a.x, a.y]);
        }

        public override string ToString()
        {
            var line = string.Empty;
            var output = string.Empty;
            for (int y = sizeY - 1; y >= 0 ; y--)
            {
                for (int x = 0 ; x < sizeX; x++)
                {
                    line += string.Concat(x > 0 ? ", " : string.Empty,
                        _shapes[x, y] == null ? $"({x},{y}) null" : $"({x},{y}) {_shapes[x, y].ToString()}");
                }
                output += string.Concat(y < sizeY - 1 ? "\n" : string.Empty, line);
                line = string.Empty;
            }
            return output;
        }
    }
}