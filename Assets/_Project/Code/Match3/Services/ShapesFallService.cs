using System.Collections.Generic;

namespace Code.Match3.Services
{
    public class ShapesFallService
    {
        public void ProcessFalling(ref Match3Grid<ShapeInfo> gridState, ICollection<ShapeFallInfo> shapeFallsBuffer)
        {
            for (int x = 0; x < gridState.sizeX; x++)
            {
                int holeCount = 0;
                for (int y = 0; y < gridState.sizeY; y++)
                {
                    if (gridState[x, y].isDestroyed)
                    {
                        holeCount++;
                    }
                    else if (holeCount > 0)
                    {
                        ShapeInfo temp = gridState[x, y];
                        gridState[x, y] = new ShapeInfo();
                        gridState[x, y - holeCount] = temp;
                        shapeFallsBuffer.Add(new ShapeFallInfo(x, y, y - holeCount));   
                    }
                }
            }
        }
    }
}