using System.Collections.Generic;
using Code.Match3;
using Code.Runtime.Infrastructure.Factories;
using Code.Runtime.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Runtime.Match3
{
    public class Match3GridView : ILoadUnit<Vector3>, IMatch3GridView
    {
        public Vector3[,] positions => _positions;
        public Vector2 shapeSize { get; private set; }
        public Vector3 centralPoint { get; private set; }
        
        private readonly IMatch3Factory _match3Factory;
        private Match3Grid<Match3ShapeView> _shapes;
        private Vector3[,] _positions;
        private Tween _hintTween;
        
        public Match3GridView(IMatch3Factory match3Factory)
        {
            _match3Factory = match3Factory;
        }

        public UniTask Load(Vector3 spawnPoint)
        {
            const float SPACING = 1.2f;
            int gridSize = RuntimeConstants.Test.GRID_SIZE;
            centralPoint = spawnPoint;

            _positions = new Vector3[gridSize, gridSize];
            _shapes = new Match3Grid<Match3ShapeView>(new Match3ShapeView[gridSize, gridSize]);
            shapeSize = _match3Factory.shapeSize;
            float hOffset = (gridSize - shapeSize.x) / 2 * SPACING;
            float vOffset = (gridSize - shapeSize.y) / 2 * SPACING;
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    Vector3 position = spawnPoint;
                    position.x += x * SPACING - hOffset;
                    position.y += y * SPACING - vOffset;
                    _positions[x, y] = position;
                    _match3Factory.CreateGridSlot(position);
                }
            }
            return UniTask.CompletedTask;
        }

        public void CreateStartState(Match3Grid<ShapeInfo> gridState)
        {
            for (int x = 0; x < gridState.sizeX; x++)
            {
                for (int y = 0; y < gridState.sizeY; y++)
                {
                    _shapes[x, y] = CreateShape(gridState[x, y].type, _positions[x, y]);
                }
            }
        }

        public void StopActiveTweens()
        {
            _hintTween?.Kill();
        }

        public async UniTask VisualizeSwap(ShapePos first, ShapePos second)
        {
            const float TEST_DURATION = 0.2f;
            Sequence seq = DOTween.Sequence();
            seq.onComplete += () => { _shapes.Swap(first, second); };
            await seq
                .Append(_shapes[first.x, first.y].MoveTo(_positions[second.x, second.y], TEST_DURATION))
                .Join(_shapes[second.x, second.y].MoveTo(_positions[first.x, first.y], TEST_DURATION));
        }

        public async UniTask VisualizeFalseSwap(ShapePos first, ShapePos second)
        {
            const float TEST_DURATION = 0.2f;
            Sequence seq = DOTween.Sequence();
            await seq
                .Append(_shapes[first.x, first.y].MoveTo(_positions[second.x, second.y], TEST_DURATION))
                .Join(_shapes[second.x, second.y].MoveTo(_positions[first.x, first.y], TEST_DURATION))
                .Append(_shapes[first.x, first.y].MoveTo(_positions[first.x, first.y], TEST_DURATION))
                .Join(_shapes[second.x, second.y].MoveTo(_positions[second.x, second.y], TEST_DURATION));
        }

        public async UniTask VisualizeFalling(List<ShapeFallInfo> fallInfos)
        {
            const float TEST_DURATION = 0.2f;
            const float BASE_FALL_DELAY_COEFF = 0.025f;
            Sequence seq = DOTween.Sequence();
            int fallenInSameColumn = 0;
            for (var i = 0; i < fallInfos.Count; i++)
            {
                if (i > 0 && fallInfos[i - 1].x == fallInfos[i].x)
                    fallenInSameColumn++;
                else
                    fallenInSameColumn = 0;

                var from = new ShapePos(fallInfos[i].x, fallInfos[i].fromY);
                var to = new ShapePos(fallInfos[i].x, fallInfos[i].toY);
                var dur = TEST_DURATION + fallenInSameColumn * BASE_FALL_DELAY_COEFF;
                seq.Join(_shapes[from.x, from.y].MoveTo(_positions[to.x, to.y], dur));
                seq.onComplete += () => _shapes.Swap(from, to);
            }
            await seq;
        }

        public async UniTask CreateNewShapeViews(List<ShapeCreateInfo> createInfos)
        {
            const float TEST_DURATION = 0.3f;
            const float BASE_FALL_DELAY_COEFF = 0.05f;
            const int BASE_SPAWN_OFFSET = 2;
            Sequence seq = DOTween.Sequence();
            int fallenInSameColumn = 0;
            for (var i = 0; i < createInfos.Count; i++)
            {
                if (i > 0 && createInfos[i - 1].pos.x == createInfos[i].pos.x)
                    fallenInSameColumn++;
                else
                    fallenInSameColumn = 0;

                Vector3 spawnPoint = _positions[createInfos[i].pos.x, _shapes.sizeY - 1];
                spawnPoint.y += BASE_SPAWN_OFFSET + shapeSize.y * fallenInSameColumn;

                _shapes[createInfos[i].pos.x, createInfos[i].pos.y] =
                    CreateShape(createInfos[i].shape.type, spawnPoint);

                var duration = TEST_DURATION + fallenInSameColumn * BASE_FALL_DELAY_COEFF;
                seq.Join(_shapes[createInfos[i].pos.x, createInfos[i].pos.y].Appear())
                    .Join(_shapes[createInfos[i].pos.x, createInfos[i].pos.y]
                        .MoveTo(_positions[createInfos[i].pos.x, createInfos[i].pos.y], duration));
            }
            await seq;
        }
        
        public async UniTask CreateMatchBonusShapeViews(List<ShapeBonusProcessInfo> processInfos)
        {
            const float TEST_DURATION = 0.2f;
            foreach (var processInfo in processInfos)
            {
                ShapePos bonusPos = processInfo.bonusPos;
                Sequence seq = DOTween.Sequence();
                foreach (var pos in processInfo.affectedShapes)
                {
                    if (_shapes[pos.x, pos.y] == null)
                        continue;

                    seq.Join(_shapes[pos.x, pos.y].MoveTo(_positions[bonusPos.x, bonusPos.y], TEST_DURATION))
                        .Join(_shapes[pos.x, pos.y].Disappear());
                    _shapes[pos.x, pos.y] = null;
                }

                await seq;

                _shapes[bonusPos.x, bonusPos.y] =
                    CreateShape(processInfo.bonusType, _positions[bonusPos.x, bonusPos.y]);

                await _shapes[bonusPos.x, bonusPos.y].Appear();
            }
        }

        public async UniTask VisualizeDestruction(Stack<ShapeBonusProcessInfo> destructionChain)
        {
            // TODO make more complex destruction visualization
            List<ShapePos> destroyList = new List<ShapePos>();
            foreach (var destroyInfo in destructionChain)
            {
                destroyList.AddRange(destroyInfo.affectedShapes);
            }
            await VisualizeDestruction(destroyList);
        }

        public async UniTask VisualizeDestruction(List<ShapePos> destroyedShapes)
        {
            Sequence seq = DOTween.Sequence();
            foreach (var pos in destroyedShapes)
            {
                if (_shapes[pos.x, pos.y] == null)
                    continue;

                seq.Join(_shapes[pos.x, pos.y].Disappear());
                _shapes[pos.x, pos.y] = null;
            }
            await seq;
        }

        public void VisualizeMatchHint(ShapeMatchPredictionInfo matchPrediction, float delay)
        {
            ShapePos from = matchPrediction.from;
            Vector3 to = new Vector2(matchPrediction.direction.x, matchPrediction.direction.y);
            _hintTween = _shapes[from.x, from.y]
                .MoveTo(_positions[from.x, from.y] + to * 0.2f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetDelay(delay);
            _hintTween.onKill += () => _shapes[from.x, from.y]?.MoveTo(_positions[from.x, from.y], 0.2f);
        }

        public async UniTask VisualizeGridReconfiguration(Match3Grid<ShapeInfo> gridState)
        {
            Sequence seq = DOTween.Sequence();
            for (int x = 0; x < gridState.sizeX; x++)
            {
                for (int y = 0; y < gridState.sizeY; y++)
                {
                    var temp = new ShapePos(x, y);
                    seq.Join(_shapes[x, y].MoveTo(centralPoint, 0.25f))
                        .Join(_shapes[temp.x, temp.y].ChangeType(gridState[temp.x, temp.y].type));
                }
            }

            await seq;
            seq = DOTween.Sequence();
            for (int x = 0; x < gridState.sizeX; x++)
            {
                for (int y = 0; y < gridState.sizeY; y++)
                {
                    seq.Join(_shapes[x, y].MoveTo(_positions[x, y], 0.25f));
                }
            }
            await seq;
        }

        public async UniTask CreateColorBomb(ShapePos pos) => 
            await _shapes[pos.x, pos.y].ChangeType(ShapeBonusType.COLOR);

        private Match3ShapeView CreateShape(ShapeType type, Vector3 spawnPoint) => 
            _match3Factory.CreateShape(spawnPoint, type);

        private Match3ShapeView CreateShape(ShapeBonusType type, Vector3 spawnPoint) => 
            _match3Factory.CreateShape(spawnPoint, type);
    }
}