using System.Collections.Generic;
using Code.Match3;
using Code.Match3.Services;
using Random = UnityEngine.Random;

namespace Code.Runtime.Match3.Services
{
    public class BonusCreateProcessService
    {
        // TODO Move to config
        private readonly int _crossTargetLength = 4;
        private readonly int _bombTargetLength = 5;
        private readonly int _colorTargetScore = 25;
        //
        private readonly ShapesDestructProcessService _shapesDestroyService;
        private readonly List<ShapeMatchInfo> _regularMatchesBuffer = new List<ShapeMatchInfo>();

        public BonusCreateProcessService(ShapesDestructProcessService shapesDestroyService)
        {
            _shapesDestroyService = shapesDestroyService;
        }

        public void ProcessMatchBonusCreation(ref Match3Grid<ShapeInfo> gridState, ShapePos bonusPos,
            List<ShapeMatchInfo> matches, List<ShapeBonusProcessInfo> shapeInfos)
        {
            foreach (var match in matches)
            {
                bool isCross = match.length == _crossTargetLength;
                bool isBomb = match.length == _bombTargetLength;
                if (!isCross && !isBomb)
                {
                    _regularMatchesBuffer.Add(match);
                    continue;
                }
                
                ShapePos spawnPos = -ShapePos.one;
                for (int i = 0; i < match.length; i++)
                {
                    int x = match.isHorizontal ? match.startPoint.x + i : match.startPoint.x;
                    int y = match.isHorizontal ? match.startPoint.y : match.startPoint.y + i;
                    
                    if (spawnPos != bonusPos)
                        spawnPos = bonusPos.x == x && bonusPos.y == y ? bonusPos : match.startPoint;
                    
                    if (gridState[x, y].isBonus)
                        break;
                    
                    if (i == match.length - 1)
                    {
                        var affectedShapes = new List<ShapePos>();
                        _shapesDestroyService.ProcessMatch(ref gridState, match, affectedShapes);
                        gridState[spawnPos.x, spawnPos.y] = new ShapeInfo(gridState[spawnPos.x, spawnPos.y].type,
                            isCross ? ShapeBonusType.CROSS : ShapeBonusType.BOMB);
                        shapeInfos.Add(new ShapeBonusProcessInfo(spawnPos, gridState[spawnPos.x, spawnPos.y].bonusType,
                            affectedShapes));
                    }
                }
            }
            matches.Clear();
            matches.AddRange(_regularMatchesBuffer);
            _regularMatchesBuffer.Clear();
        }

        public bool TryCreateColorBomb(ref Match3Grid<ShapeInfo> gridState, out ShapePos pos)
        {
            if (true) // debug
            {
                pos = new ShapePos(Random.Range(0, gridState.sizeX), Random.Range(0, gridState.sizeY));
                gridState[pos.x, pos.y] = new ShapeInfo(gridState[pos.x, pos.y].type, ShapeBonusType.COLOR);
            }
            return true;
        }
    }
}