using Code.Runtime.Logic.Match3;
using UnityEngine;

namespace Code.Runtime.Infrastructure.Services
{
    public class ShapesFallService
    {
        public void ProcessFall(Match3ShapeType[,] shapes)
        {
            int holesCount = 0;
            for (int i = 0; i < shapes.GetLength(0); i++)
            {
                int bottom = -1;
                for (int j = 0; j < shapes.GetLength(1); j++)
                {
                    if (shapes[i, j] == Match3ShapeType.NONE && bottom == -1)
                    {
                        bottom = j;
                        continue;
                    }

                    if (shapes[i, j] != Match3ShapeType.NONE)
                    {
                        shapes[i, bottom] = shapes[i, j];
                        shapes[i, j] = Match3ShapeType.NONE;
                        bottom++;
                    }
                }
            }
        }
    }
}