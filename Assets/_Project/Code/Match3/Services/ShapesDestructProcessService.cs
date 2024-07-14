using System.Collections.Generic;

namespace Code.Match3.Services
{
    public class ShapesDestructProcessService
    {
        public void ProcessMatch(ref Match3Grid<ShapeInfo> gridState, ShapeMatchInfo match,
            ICollection<ShapePos> matchedShapes)
        {
            for (int i = 0; i < match.length; i++)
            {
                int x = match.isHorizontal ? match.startPoint.x + i : match.startPoint.x;
                int y = match.isHorizontal ? match.startPoint.y : match.startPoint.y + i;
                if (gridState[x, y].isBonus)
                    continue;
                
                gridState[x, y] = new ShapeInfo();
                matchedShapes.Add(new ShapePos(x, y));
            }
        }

        public void ProcessBonus(ref Match3Grid<ShapeInfo> gridState, ShapePos bonusPos, ShapeType target,
            Stack<ShapeBonusProcessInfo> chain)
        {
            ShapeBonusType bonusType = gridState[bonusPos.x, bonusPos.y].bonusType;
            gridState[bonusPos.x, bonusPos.y] = new ShapeInfo();
            switch (bonusType)
            {
                case ShapeBonusType.BOMB:
                    ProcessBomb(ref gridState, bonusPos, chain);
                    break;
                case ShapeBonusType.CROSS:
                    ProcessCross(ref gridState, bonusPos, chain);
                    break;
                case ShapeBonusType.COLOR:
                    ProcessColor(ref gridState, bonusPos, target, chain);
                    break;
            }
        }

        private void ProcessBomb(ref Match3Grid<ShapeInfo> gridState, ShapePos bonusPos,
            Stack<ShapeBonusProcessInfo> chain)
        {
            var destroyedShapes = new List<ShapePos>();
            const int RANGE = 3;
            for (int x = bonusPos.x - RANGE; x <= bonusPos.x + RANGE; x++)
            {
                for (int y = bonusPos.y - RANGE; y <= bonusPos.y + RANGE; y++)
                {
                    var cur = new ShapePos(x, y);
                    if (gridState.IsValid(cur) == false)
                        continue;
                    
                    if (gridState[cur.x, cur.y].isBonus)
                    {
                        ProcessBonus(ref gridState, cur, gridState[cur.x, cur.y].type, chain);
                    }
                    else
                    {
                        destroyedShapes.Add(cur);
                        gridState[cur.x, cur.y] = new ShapeInfo();   
                    }
                }
            }
            chain.Push(new ShapeBonusProcessInfo(bonusPos, ShapeBonusType.BOMB, destroyedShapes));
        }

        private void ProcessCross(ref Match3Grid<ShapeInfo> gridState, ShapePos bonusPos,
            Stack<ShapeBonusProcessInfo> chain)
        {
            var destroyedShapes = new List<ShapePos>();
            const int RANGE = 2;
            for (int x = bonusPos.x - RANGE; x <= bonusPos.x + RANGE; x++)
            {
                var cur = new ShapePos(x, bonusPos.y);
                if (gridState.IsValid(cur) == false)
                    continue;
                
                if (gridState[cur.x, cur.y].isBonus)
                { 
                    ProcessBonus(ref gridState, cur, gridState[cur.x, cur.y].type, chain);   
                }
                else
                {
                    destroyedShapes.Add(cur);
                    gridState[cur.x, cur.y] = new ShapeInfo();
                }
            }
            
            for (int y = bonusPos.y - RANGE; y <= bonusPos.y + RANGE; y++)
            {
                var cur = new ShapePos(bonusPos.x, y);
                if (gridState.IsValid(cur) == false) 
                    continue;
                
                if (gridState[cur.x, cur.y].isBonus)
                {
                    ProcessBonus(ref gridState, cur, gridState[cur.x, cur.y].type, chain);
                }
                else
                {
                    destroyedShapes.Add(cur);
                    gridState[cur.x, cur.y] = new ShapeInfo();
                }
            }

            chain.Push(new ShapeBonusProcessInfo(bonusPos, ShapeBonusType.CROSS, destroyedShapes));
        }

        private void ProcessColor(ref Match3Grid<ShapeInfo> gridState, ShapePos bonusPos, ShapeType targetType, 
            Stack<ShapeBonusProcessInfo> chain)
        {
            var destroyedShapes = new List<ShapePos> { bonusPos };
            for (int x = 0; x < gridState.sizeX; x++)
            {
                for (int y = 0; y < gridState.sizeY; y++)
                {
                    if (gridState[x, y].type != targetType || gridState[x, y].isBonus)
                        continue;

                    destroyedShapes.Add(new ShapePos(x, y));
                    gridState[x, y] = new ShapeInfo();
                }
            }
            chain.Push(new ShapeBonusProcessInfo(bonusPos, ShapeBonusType.COLOR, destroyedShapes));
        }
    }
}