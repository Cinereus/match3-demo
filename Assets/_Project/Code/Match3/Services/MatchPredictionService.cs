namespace Code.Match3.Services
{
    public class MatchPredictionService
    {
        public bool TryFindMatchPrediction(Match3Grid<ShapeInfo> gridState, out ShapeMatchPredictionInfo predictionInfo)
        {
            int width = gridState.sizeX;
            int height = gridState.sizeY;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var cur = new ShapePos(x, y);

                    bool isBonus = gridState[cur.x, cur.y].isBonus;
                    
                    bool isLeft = cur.x - 2 >= 0 && cur.x - 3 >= 0 &&
                                  ((gridState[cur.x, cur.y].Equals(gridState[cur.x - 2, cur.y]) && 
                                    gridState[cur.x, cur.y].Equals(gridState[cur.x - 3, cur.y])) || isBonus);

                    bool isLeftUp = cur.x - 1 >= 0 && cur.y + 2 < height && cur.y + 2 < height &&
                                    ((gridState[cur.x, cur.y].Equals(gridState[cur.x - 1, cur.y + 1]) && 
                                      gridState[cur.x, cur.y].Equals(gridState[cur.x - 1, cur.y + 2])) || isBonus);

                    bool isLeftDown = cur.x - 1 >= 0 && cur.y - 1 >= 0 && cur.y - 2 >= 0 &&
                                      ((gridState[cur.x, cur.y].type == gridState[cur.x - 1, cur.y - 1].type && 
                                        gridState[cur.x, cur.y].Equals(gridState[cur.x - 1, cur.y - 2])) || isBonus);

                    bool isRight = cur.x + 2 < width && cur.x + 3 < width &&
                                   ((gridState[cur.x, cur.y].Equals(gridState[cur.x + 2, cur.y]) && 
                                     gridState[cur.x, cur.y].Equals(gridState[cur.x + 3, cur.y])) || isBonus);

                    bool isRightUp = cur.x + 1 < width && cur.y + 1 < height && cur.y + 2 < height &&
                                     ((gridState[cur.x, cur.y].Equals(gridState[cur.x + 1, cur.y + 1]) && 
                                       gridState[cur.x, cur.y].Equals(gridState[cur.x + 1, cur.y + 2])) || isBonus);

                    bool isRightDown = cur.x + 1 < width && cur.y - 1 >= 0 && cur.y - 2 >= 0 &&
                                       ((gridState[cur.x, cur.y].Equals(gridState[cur.x + 1, cur.y - 1]) && 
                                         gridState[cur.x, cur.y].Equals(gridState[cur.x + 1, cur.y - 2])) || isBonus);

                    bool isUp = cur.y + 2 < height && cur.y + 3 < height &&
                                ((gridState[cur.x, cur.y].Equals(gridState[cur.x, cur.y + 2]) && 
                                  gridState[cur.x, cur.y].Equals(gridState[cur.x, cur.y + 3])) || isBonus);

                    bool isUpLeft = cur.y + 1 < height && cur.x - 1 >= 0 && cur.x - 2 >= 0 &&
                                    ((gridState[cur.x, cur.y].Equals(gridState[cur.x - 1, cur.y + 1]) && 
                                      gridState[cur.x, cur.y].Equals(gridState[cur.x - 2, cur.y + 1])) || isBonus);

                    bool isUpRight = cur.y + 1 < height && cur.x + 1 < width && cur.x + 2 < width &&
                                     ((gridState[cur.x, cur.y].Equals(gridState[cur.x + 1, cur.y + 1]) && 
                                       gridState[cur.x, cur.y].Equals(gridState[cur.x + 2, cur.y + 1])) || isBonus);

                    bool isDown = cur.y - 2 >= 0 && cur.y - 3 >= 0 &&
                                  ((gridState[cur.x, cur.y].Equals(gridState[cur.x, cur.y - 2]) && 
                                    gridState[cur.x, cur.y].Equals(gridState[cur.x, cur.y - 3])) || isBonus);

                    bool isDownLeft = cur.y - 1 >= 0 && cur.x - 1 >= 0 && cur.x - 2 >= 0 &&
                                      ((gridState[cur.x, cur.y].Equals(gridState[cur.x - 1, cur.y - 1]) && 
                                        gridState[cur.x, cur.y].Equals(gridState[cur.x - 2, cur.y - 1])) || isBonus);

                    bool isDownRight = cur.y - 1 >= 0 && cur.x + 1 < width && cur.x + 2 < width &&
                                       ((gridState[cur.x, cur.y].Equals(gridState[cur.x + 1, cur.y - 1]) && 
                                         gridState[cur.x, cur.y].Equals(gridState[cur.x + 2, cur.y - 1])) || isBonus);
                    
                    if (isLeft || isLeftUp || isLeftDown)
                    {
                        predictionInfo = new ShapeMatchPredictionInfo(cur, new ShapePos(-1, 0));
                        return true;
                    }
                    if (isRight || isRightUp || isRightDown)
                    {
                        predictionInfo = new ShapeMatchPredictionInfo(cur, new ShapePos(1, 0));
                        return true;
                    }
                    if (isUp || isUpLeft || isUpRight)
                    {
                        predictionInfo = new ShapeMatchPredictionInfo(cur, new ShapePos(0, 1));
                        return true;
                    }
                    if (isDown || isDownLeft || isDownRight)
                    {
                        predictionInfo = new ShapeMatchPredictionInfo(cur, new ShapePos(0, -1));
                        return true;
                    }
                }
            }
            predictionInfo = default;
            return false;
        }
    }
}