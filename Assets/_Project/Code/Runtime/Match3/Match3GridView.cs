using System;
using System.Collections.Generic;
using System.Threading;
using Code.Match3;
using Code.Runtime.Infrastructure.Factories;
using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Infrastructure.StaticData;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Runtime.Match3
{ 
    public class Match3GridView : ILoadUnit<Match3GridView.Params>, IMatch3GridView, IDisposable
    { 
        public readonly struct Params
        {
            public readonly Transform parent;
            public readonly Match3LevelConfig levelConfig;

            public Params(Transform parent, Match3LevelConfig levelConfig)
            {
                this.parent = parent;
                this.levelConfig = levelConfig;
            }
        }
        
        public Vector3[,] positions => _positions;

        public event Action onSwap;
        public event Action<ShapeType> onShapeDestroyed;

        private readonly IMatch3Factory _factory;
        private readonly IMatch3VFXFactory _vfxFactory;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        
        private float _swapDuration;
        private float _bonusCreateShapesMoveDuration;
        private float _fallDuration;
        private float _baseFallDelayCoeff;
        private float _spawnShapesOffset;
        private float _matchHintDuration;
        private float _hintMoveOffset;
        private float _gridResetDuration;
        private float _shapeDisappearDuration;
        private float _shapeAppearDuration;
        private Vector2 _shapeSize;
        private Match3Grid<Match3ShapeView> _shapes;
        private Vector3[,] _positions;
        private Transform _parent;
        private Tween _hintTween;

        public Match3GridView(IMatch3Factory factory, IMatch3VFXFactory vfxFactory)
        {
            _factory = factory;
            _vfxFactory = vfxFactory;
        }

        public UniTask Load(Params viewParams, CancellationToken token)
        { 
            bool isColumnSorted = viewParams.levelConfig.columnTileZSorting;
            float sortCoeff = viewParams.levelConfig.columnSortCoeff;
            float spacing = viewParams.levelConfig.tileSpacing;
            int width = viewParams.levelConfig.gridWidth;
            int height = viewParams.levelConfig.gridHeight;
            
            _parent = viewParams.parent;
            _positions = new Vector3[width, height];
            _shapes = new Match3Grid<Match3ShapeView>(new Match3ShapeView[width, height]);
            _shapeSize = _factory.shapeSize;
            _swapDuration = viewParams.levelConfig.swapDuration;
            _bonusCreateShapesMoveDuration = viewParams.levelConfig.bonusCreateShapesMoveDuration;
            _fallDuration = viewParams.levelConfig.fallDuration;
            _baseFallDelayCoeff = viewParams.levelConfig.baseFallDelayCoeff;
            _spawnShapesOffset = viewParams.levelConfig.spawnShapesOffset;
            _matchHintDuration = viewParams.levelConfig.matchHintDuration;
            _hintMoveOffset = viewParams.levelConfig.hintMoveOffset;
            _gridResetDuration = viewParams.levelConfig.gridResetDuration;
            _shapeAppearDuration = viewParams.levelConfig.shapeAppearDuration;
            _shapeDisappearDuration = viewParams.levelConfig.shapeDisappearDuration;

            float hOffset = (width - _shapeSize.x) / 2 * spacing;
            float vOffset = (height - _shapeSize.y) / 2 * spacing;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 position = _parent.position;
                    position.x += x * spacing - hOffset;
                    position.y += y * spacing - vOffset;
                    
                    if (isColumnSorted)
                        position.z -= y * sortCoeff;

                    _positions[x, y] = position;
                    _factory.CreateGridSlot(position, _parent);
                }
            }
            return UniTask.CompletedTask;
        }
        
        public void Dispose()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
            StopActiveTweens();
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
            Sequence seq = DOTween.Sequence();
            seq.onComplete += () => { _shapes.Swap(first, second); };
            await seq
                .Append(_shapes[first.x, first.y].MoveTo(_positions[second.x, second.y], _swapDuration))
                .Join(_shapes[second.x, second.y].MoveTo(_positions[first.x, first.y], _swapDuration))
                .ToUniTask(cancellationToken: _tokenSource.Token);
            onSwap?.Invoke();
        }

        public async UniTask VisualizeFalseSwap(ShapePos first, ShapePos second)
        {
            Sequence seq = DOTween.Sequence();
            await seq
                .Append(_shapes[first.x, first.y].MoveTo(_positions[second.x, second.y], _swapDuration))
                .Join(_shapes[second.x, second.y].MoveTo(_positions[first.x, first.y], _swapDuration))
                .Append(_shapes[first.x, first.y].MoveTo(_positions[first.x, first.y], _swapDuration))
                .Join(_shapes[second.x, second.y].MoveTo(_positions[second.x, second.y], _swapDuration))
                .ToUniTask(cancellationToken: _tokenSource.Token);
        }

        public async UniTask VisualizeFalling(List<ShapeFallInfo> fallInfos)
        {
            Sequence seq = DOTween.Sequence();
            int fallenInSameColumn = 0;
            for (var i = 0; i < fallInfos.Count; i++)
            {
                bool isSameColumn = i > 0 && fallInfos[i - 1].x == fallInfos[i].x; 
                if (isSameColumn)
                    fallenInSameColumn++;
                else
                    fallenInSameColumn = 0;

                var from = new ShapePos(fallInfos[i].x, fallInfos[i].fromY);
                var to = new ShapePos(fallInfos[i].x, fallInfos[i].toY);
                var dur = _fallDuration + fallenInSameColumn * _baseFallDelayCoeff;
                seq.Join(_shapes[from.x, from.y].MoveTo(_positions[to.x, to.y], dur));
                seq.onComplete += () => _shapes.Swap(from, to);
            }
            await seq.ToUniTask(cancellationToken: _tokenSource.Token);
        }
        
        public async UniTask CreateNewShapeViews(List<ShapeCreateInfo> createInfos)
        {
            Sequence seq = DOTween.Sequence();
            int fallenInSameColumn = 0;
            for (var i = 0; i < createInfos.Count; i++)
            {
                if (i > 0 && createInfos[i - 1].pos.x == createInfos[i].pos.x)
                    fallenInSameColumn++;
                else
                    fallenInSameColumn = 0;

                Vector3 spawnPoint = _positions[createInfos[i].pos.x, _shapes.sizeY - 1];
                spawnPoint.y += _spawnShapesOffset + _shapeSize.y * fallenInSameColumn;

                _shapes[createInfos[i].pos.x, createInfos[i].pos.y] =
                    CreateShape(createInfos[i].shape.type, spawnPoint);

                var duration = _fallDuration + fallenInSameColumn * _baseFallDelayCoeff;
                seq.Join(_shapes[createInfos[i].pos.x, createInfos[i].pos.y].Appear(_shapeAppearDuration))
                    .Join(_shapes[createInfos[i].pos.x, createInfos[i].pos.y]
                        .MoveTo(_positions[createInfos[i].pos.x, createInfos[i].pos.y], duration));
            }
            await seq.ToUniTask(cancellationToken: _tokenSource.Token);
        }
        
        public async UniTask CreateMatchBonusShapeViews(List<ShapeBonusProcessInfo> processInfos)
        {
            foreach (var processInfo in processInfos)
            {
                ShapePos bonusPos = processInfo.bonusPos;
                Sequence seq = DOTween.Sequence();
                foreach (var pos in processInfo.affectedShapes)
                {
                    if (_shapes[pos.x, pos.y] == null)
                        continue;

                    ShapeType shapeTypeCache = _shapes[pos.x, pos.y].type;
                    Sequence disappearTween = _shapes[pos.x, pos.y].Disappear(_shapeDisappearDuration);
                    disappearTween.onComplete += () => onShapeDestroyed?.Invoke(shapeTypeCache);
                    seq.Join(_shapes[pos.x, pos.y]
                            .MoveTo(_positions[bonusPos.x, bonusPos.y], _bonusCreateShapesMoveDuration))
                        .Join(disappearTween);
                    _shapes[pos.x, pos.y] = null;
                }
                await seq.ToUniTask(cancellationToken: _tokenSource.Token);

                _shapes[bonusPos.x, bonusPos.y] =
                    CreateShape(processInfo.bonusType, _positions[bonusPos.x, bonusPos.y]);

                await _shapes[bonusPos.x, bonusPos.y].Appear(_shapeAppearDuration).ToUniTask(cancellationToken: _tokenSource.Token);
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
                
                ShapeType shapeTypeCache = _shapes[pos.x, pos.y].type;
                Sequence disappearTween = _shapes[pos.x, pos.y].Disappear(_shapeDisappearDuration);
                seq.Join(disappearTween);
                disappearTween.onComplete += () =>
                {
                    _vfxFactory
                        .CreateShapeDestroyParticle(shapeTypeCache, _positions[pos.x, pos.y])
                        .PlayOnce(_tokenSource.Token)
                        .Forget();
                    
                    onShapeDestroyed?.Invoke(shapeTypeCache);
                };
                _shapes[pos.x, pos.y] = null;
            }
            await seq.ToUniTask(cancellationToken: _tokenSource.Token);
        }
        
        public void VisualizeMatchHint(ShapeMatchPredictionInfo matchPrediction, float delay)
        {
            ShapePos from = matchPrediction.from;
            Vector3 to = new Vector2(matchPrediction.direction.x, matchPrediction.direction.y);
            
            if (_shapes[from.x, from.y] == null)
                return;
            
            _hintTween = _shapes[from.x, from.y]
                .MoveTo(_positions[from.x, from.y] + to * _hintMoveOffset, _matchHintDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetDelay(delay);
            
            _hintTween.onKill += () =>
            {
                const float MOVE_DURATION = 0.2f;
                if (_positions[from.x, from.y] != null && _shapes[from.x, from.y] != null)
                    _shapes[from.x , from.y].MoveTo(_positions[from.x, from.y], MOVE_DURATION);
            };
        }

        public async UniTask VisualizeGridReconfiguration(Match3Grid<ShapeInfo> gridState)
        {
            Sequence seq = DOTween.Sequence();
            for (int x = 0; x < gridState.sizeX; x++)
            {
                for (int y = 0; y < gridState.sizeY; y++)
                {
                    seq.Join(_shapes[x, x].MoveTo(_parent.position, _gridResetDuration))
                        .Join(_shapes[x, y].Disappear(_shapeDisappearDuration));
                }
            }
            await seq.ToUniTask(cancellationToken: _tokenSource.Token);
            
            seq = DOTween.Sequence();
            for (int x = 0; x < gridState.sizeX; x++)
            {
                for (int y = 0; y < gridState.sizeY; y++)
                {
                    _shapes[x, y] = _factory.CreateShape(_parent.position, gridState[x, y].type, _parent);
                    seq.Join(_shapes[x, y].Appear(_shapeAppearDuration))
                        .Join(_shapes[x, y].MoveTo(_positions[x, y], _gridResetDuration));
                }
            }
            await seq.ToUniTask(cancellationToken: _tokenSource.Token);
        }

        public async UniTask CreateColorBomb(ShapePos pos)
        {
            await _shapes[pos.x, pos.y].Disappear(_shapeDisappearDuration)
                .ToUniTask(cancellationToken: _tokenSource.Token);
            
            _shapes[pos.x, pos.y] = _factory.CreateShape(_positions[pos.x, pos.y], ShapeBonusType.COLOR, _parent);
            
            await _shapes[pos.x, pos.y].Appear(_shapeAppearDuration).ToUniTask(cancellationToken: _tokenSource.Token);
        }

        private Match3ShapeView CreateShape(ShapeType type, Vector3 spawnPoint) => 
            _factory.CreateShape(spawnPoint, type, _parent);

        private Match3ShapeView CreateShape(ShapeBonusType type, Vector3 spawnPoint) => 
            _factory.CreateShape(spawnPoint, type, _parent);
    }
}