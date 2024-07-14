using System.Collections.Generic;

namespace Code.Match3.Services
{
    public class FindMatchService
    {
        public void FindMatches(Match3Grid<ShapeInfo> gridState, ICollection<ShapeMatchInfo> matches)
        {
            FindHorizontalMatches(gridState, matches);
            FindVerticalMatches(gridState, matches);
        }
        
        private void FindHorizontalMatches(Match3Grid<ShapeInfo> gridState, ICollection<ShapeMatchInfo> matches)
        {
            int width = gridState.sizeX;
            int height = gridState.sizeY;
            for (int y = 0; y < height; y++)
            {
                int matchLength = 1;
                ShapeInfo current = gridState[0, y];

                for (int x = 1; x < width; x++)
                {
                    ShapeInfo next = gridState[x, y];

                    if (current.isDestroyed)
                    {
                        current = next;
                        continue;
                    }

                    if (current.type == next.type && !current.isBonus && !next.isBonus)
                    {
                        matchLength++;
                        continue;
                    }

                    if (matchLength > 2)
                        matches.Add(new ShapeMatchInfo(new ShapePos(x - matchLength, y), matchLength,
                            true));

                    current = next;
                    matchLength = 1;
                }

                if (matchLength > 2)
                    matches.Add(new ShapeMatchInfo(new ShapePos(width - matchLength, y), matchLength,
                        true));
            }
        }

        private void FindVerticalMatches(Match3Grid<ShapeInfo> gridState, ICollection<ShapeMatchInfo> matches)
        {
            int width = gridState.sizeX;
            int height = gridState.sizeY;
            for (int x = 0; x < width; x++)
            {
                int matchLength = 1;
                ShapeInfo current = gridState[x, 0];

                for (int y = 1; y < height; y++)
                {
                    ShapeInfo next = gridState[x, y];
                    
                    if (current.isDestroyed)
                    {
                        current = next;
                        continue;
                    }

                    if (current.type == next.type && !current.isBonus && !next.isBonus)
                    {
                        matchLength++;
                        continue;
                    }

                    if (matchLength > 2)
                        matches.Add(new ShapeMatchInfo(new ShapePos(x, y - matchLength), matchLength));

                    current = next;
                    matchLength = 1;
                }

                if (matchLength > 2)
                    matches.Add(new ShapeMatchInfo(new ShapePos(x, height - matchLength), matchLength));
            }
        }
    }
}