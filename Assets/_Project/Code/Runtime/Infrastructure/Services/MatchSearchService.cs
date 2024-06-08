using System.Collections.Generic;
using Code.Runtime.Logic.Match3;
using UnityEngine;

namespace Code.Runtime.Infrastructure.Services
{
    public class MatchSearchService : IMatchSearchService
    {
        public void FindMatches(Match3GridState grid, List<Match> matches)
        {
            FindHorizontally(grid, matches);
            FindVertically(grid, matches);
        }

        private void FindHorizontally(Match3GridState grid, List<Match> matches)
        {
            int width = grid.size.x;
            int height = grid.size.y;
            for (int y = 0; y < height; y++)
            {
                int matchLength = 1;
                Match3ShapeType current = grid[0, y];
                for (int x = 1; x < width; x++)
                {
                    Match3ShapeType next = grid[x, y];
                    if (current == next)
                    {
                        matchLength++;
                        continue;
                    }

                    if (matchLength > 2)
                        matches.Add(new Match(new Vector2Int(x - matchLength, y), matchLength, 
                            true));

                    current = next;
                    matchLength = 1;
                }
                
                if (matchLength > 2)
                    matches.Add(new Match(new Vector2Int(width - matchLength, y), matchLength, 
                        true));
            }
        }

        private void FindVertically(Match3GridState grid, List<Match> matches)
        {
            int width = grid.size.x;
            int height = grid.size.y;
            for (int x = 0; x < width; x++)
            {
                int matchLength = 1;
                Match3ShapeType current = grid[x, 0];
                for (int y = 1; y < height; y++)
                {
                    Match3ShapeType next = grid[x, y];
                    if (current == next)
                    {
                        matchLength++;
                        continue;
                    }
                    
                    if (matchLength > 2)
                        matches.Add(new Match(new Vector2Int(x, y - matchLength), matchLength));

                    current = next;
                    matchLength = 1;
                }
                
                if (matchLength > 2)
                    matches.Add(new Match(new Vector2Int(x, height - matchLength), matchLength));
            }
        }
    }
}