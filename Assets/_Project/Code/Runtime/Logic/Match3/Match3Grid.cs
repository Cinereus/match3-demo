using UnityEngine;

namespace Code.Runtime.Logic.Match3
{
    public struct Match3Grid<T>
    {
        public T this[int x, int y]
        {
            get => _cells[x, y];
            set => _cells[x, y] = value;
        }

        public readonly Vector2Int size => new(_cells.GetLength(0), _cells.GetLength(1));

        private readonly T[,] _cells;
        
        public Match3Grid(T [,] cells)
        {
            _cells = cells;
        }

        public bool IsValid(Vector2Int coordinates) => 
            0 <= coordinates.x && coordinates.x < size.x && 0 <= coordinates.y && coordinates.y < size.y;
        
        public void Swap(Vector2Int a, Vector2Int b)
        {
            string err = $"[{nameof(Match3Grid<T>)}] Swap failed, index \"[index]\" less than zero or more than grid size!";
            if (IsValid(a) == false)
            {
                Debug.LogError(err.Replace("[index]", a.ToString()));
                return;
            }
            if (IsValid(b) == false)
            {
                Debug.LogError(err.Replace("[index]", b.ToString()));
                return;
            }

            (this[a.x, a.y], this[b.x , b.y]) = (this[b.x, b.y], this[a.x, a.y]);
        }
    }
}