using Code.Runtime.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Runtime.Logic.Match3
{
    public class Match3GridState : ILoadUnit
    {
        public Match3ShapeType this[int x, int y]
        {
            get => _gridState[x, y];
            set => _gridState[x, y] = value;
        }

        public Vector2Int size => _gridState.size;
        
        private Match3Grid<Match3ShapeType> _gridState;

        public UniTask Load()
        {
            int gridSize = RuntimeConstants.Test.GRID_LENGTH;
            _gridState = new Match3Grid<Match3ShapeType>(new Match3ShapeType[gridSize, gridSize]);
            return UniTask.CompletedTask;
        }

        public void Swap(Vector2Int a, Vector2Int b) => _gridState.Swap(a, b);
        
        public void ConfigureStartGridState()
        { 
            for (int x = 0; x < _gridState.size.x; x++)
            { 
                for (int y = 0; y < _gridState.size.y; y++)
                {
                    int possibleMatchCount = 0;
                    Match3ShapeType hMatchValue = Match3ShapeType.NONE;
                    Match3ShapeType vMatchValue = Match3ShapeType.NONE;
                    
                    if (x > 1)
                    {
                        if (_gridState[x - 1, y] == _gridState[x - 2, y]) 
                        {
                             hMatchValue = _gridState[x - 1, y];
                             possibleMatchCount++;
                        }
                    }
                    
                    if (y > 1)
                    {
                        if (_gridState[x, y - 1] == _gridState[x, y - 2])
                        {
                            vMatchValue = _gridState[x, y - 1];
                            possibleMatchCount++;    
                        }
                    }
                    
                    int randMax = typeof(Match3ShapeType).GetEnumValues().Length;
                    Match3ShapeType newShape = (Match3ShapeType) Random.Range(1, randMax - possibleMatchCount);
                    
                    if (newShape == hMatchValue)
                        newShape++;
                    
                    if (newShape == vMatchValue)
                        newShape++;
                    
                    _gridState[x, y] = newShape;
                }   
            }
        }
    }
}