using System.Collections.Generic;
using Code.Match3;
using Code.Match3.Services;
using Code.Runtime.Infrastructure.StaticData;
using Random = UnityEngine.Random;

namespace Code.Runtime.Match3.Services
{
    public class BonusCreateProcessService
    {
        private readonly Match3LevelConfig _levelConfig;
        private readonly ShapesDestructProcessService _shapesDestroyService;
        private readonly List<ShapeMatchInfo> _regularMatchesBuffer = new List<ShapeMatchInfo>();

        public BonusCreateProcessService(ShapesDestructProcessService shapesDestroyService,
            Match3LevelConfig levelConfig)
        {
            _levelConfig = levelConfig;
            _shapesDestroyService = shapesDestroyService;
        }

        public void ProcessMatchBonusCreation(ref Match3Grid<ShapeInfo> gridState, ShapePos bonusPos,
            List<ShapeMatchInfo> matches, List<ShapeBonusProcessInfo> shapeInfos)
        {
            foreach (var match in matches)
            {
                bool isCross = match.length == _levelConfig.crossTargetLength;
                bool isBomb = match.length == _levelConfig.bombTargetLength;
                if ((!isCross && !isBomb))
                {
                    _regularMatchesBuffer.Add(match);
                    continue;
                }

                if ((isBomb && _levelConfig.bonuses.Contains(ShapeBonusType.BOMB) == false) ||
                    (isCross && _levelConfig.bonuses.Contains(ShapeBonusType.CROSS) == false))
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

        public bool TryCreateColorBomb(ref Match3Grid<ShapeInfo> gridState, int bonusesUsed, out ShapePos pos)
        {
            if (_levelConfig.bonuses.Contains(ShapeBonusType.COLOR) == false)
            {
                pos = default;
                return false;
            }
            
            if (bonusesUsed == _levelConfig.colorTargetBonusUse)
            {
                ShapePos newPos = new ShapePos(Random.Range(0, gridState.sizeX), Random.Range(0, gridState.sizeY));
                pos = newPos;
                gridState[pos.x, pos.y] = new ShapeInfo(gridState[pos.x, pos.y].type, ShapeBonusType.COLOR);
            }
            else
            {
                pos = default;
            }
            return bonusesUsed == _levelConfig.colorTargetBonusUse;
        }
    }
}