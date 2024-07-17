using System.Collections.Generic;
using Random = System.Random;

namespace Code.Match3.Services
{
    public class ShapeStatesCreationService
    {
        private readonly int _maxShapeTypesCount;
        private readonly Random _rand = new Random();
        
        public ShapeStatesCreationService(int maxShapeTypesCount)
        {
            _maxShapeTypesCount = maxShapeTypesCount;
        }

        public void ConfigureNewGridState(ref Match3Grid<ShapeInfo> gridState)
        {
            for (int x = 0; x < gridState.sizeX; x++)
            {
                for (int y = 0; y < gridState.sizeY; y++)
                {
                    int possibleMatchCount = 0;
                    ShapeType hMatchValue = ShapeType.NONE;
                    ShapeType vMatchValue = ShapeType.NONE;

                    if (x > 1 && gridState[x - 1, y].type == gridState[x - 2, y].type)
                    { 
                        hMatchValue = gridState[x - 1, y].type;
                        possibleMatchCount++;
                    }

                    if (y > 1 && gridState[x, y - 1].type == gridState[x, y - 2].type)
                    {
                        vMatchValue = gridState[x, y - 1].type;
                        possibleMatchCount++;
                    }
                    
                    ShapeType newShapeType = (ShapeType) _rand.Next(1, _maxShapeTypesCount - possibleMatchCount);
                    if (newShapeType == hMatchValue)
                        newShapeType++;

                    if (newShapeType == vMatchValue)
                        newShapeType++;

                    gridState[x, y] = new ShapeInfo(newShapeType);
                }
            }
        }

        public void CreateNewShapeStates(ref Match3Grid<ShapeInfo> gridState, ICollection<ShapeCreateInfo> createInfos)
        {
            for (int x = 0; x < gridState.sizeX; x++)
            {
                for (int y = 0; y < gridState.sizeY; y++)
                {
                    if (gridState[x, y].isDestroyed == false)
                        continue;
                    
                    gridState[x, y] = new ShapeInfo((ShapeType) _rand.Next(1, _maxShapeTypesCount));
                    createInfos.Add(new ShapeCreateInfo(new ShapePos(x, y), gridState[x, y]));
                }
            }
        }
    }
}