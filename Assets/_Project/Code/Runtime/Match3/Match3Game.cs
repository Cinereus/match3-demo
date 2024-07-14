using System;
using System.Collections.Generic;
using Code.Match3;
using Code.Match3.Services;
using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Match3.Services;
using Cysharp.Threading.Tasks;

namespace Code.Runtime.Match3
{
    public class Match3Game : ILoadUnit, IDisposable
    {
        private readonly IMatch3GridView _gridView;
        private readonly GameMoveService _gameMoveService;
        private readonly FindMatchService _findMatchService;
        private readonly ShapesFallService _shapesFallService;
        private readonly MatchPredictionService _matchPredictService;
        private readonly BonusCreateProcessService _bonusCreateService;
        private readonly ShapeStatesCreationService _shapeStatesCreateService;
        private readonly ShapesDestructProcessService _shapesDestroyService;
        private readonly List<ShapePos> _matchedPosesBuffer = new List<ShapePos>();
        private readonly List<ShapeFallInfo> _fallInfoBuffer = new List<ShapeFallInfo>();
        private readonly List<ShapeMatchInfo> _matchInfoBuffer = new List<ShapeMatchInfo>();
        private readonly List<ShapeCreateInfo> _createInfoBuffer = new List<ShapeCreateInfo>();
        private readonly List<ShapeBonusProcessInfo> _bonusGenInfoBuffer = new List<ShapeBonusProcessInfo>();
        private readonly Stack<ShapeBonusProcessInfo> _bonusDestructBuffer = new Stack<ShapeBonusProcessInfo>();

        private bool _isBusy;
        private Match3Grid<ShapeInfo> _gridState;

        public Match3Game(FindMatchService findMatchService, ShapesFallService shapesFallService,
            ShapesDestructProcessService shapesDestroyService, ShapeStatesCreationService shapeStatesCreateService,
            GameMoveService gameMoveService, BonusCreateProcessService bonusCreateService,
            MatchPredictionService matchPredictService, IMatch3GridView gridView)
        {
            _gridView = gridView;
            _bonusCreateService = bonusCreateService;
            _gameMoveService = gameMoveService;
            _findMatchService = findMatchService;
            _shapesFallService = shapesFallService;
            _matchPredictService = matchPredictService;
            _shapesDestroyService = shapesDestroyService;
            _shapeStatesCreateService = shapeStatesCreateService;
        }

        public UniTask Load()
        {
            int gridSize = RuntimeConstants.Test.GRID_SIZE;
            _gridState = new Match3Grid<ShapeInfo>(new ShapeInfo[gridSize, gridSize]);
            _shapeStatesCreateService.ConfigureNewGridState(ref _gridState);
            _gridView.CreateStartState(_gridState);
            _gameMoveService.onMove += OnMove;
            
            if (_matchPredictService.TryFindMatchPrediction(_gridState, out var matchPrediction))
                _gridView.VisualizeMatchHint(matchPrediction, RuntimeConstants.Test.HINT_TIME_DELAY);
            
            return UniTask.CompletedTask;
        }
        
        public void Dispose()
        { 
            _gameMoveService.onMove -= OnMove;
            _gridView.StopActiveTweens();
        }

        private async void OnMove(ShapePos from, ShapePos to)
        {
            if (_isBusy)
                return;

            if (_gridState.IsValid(from) == false || _gridState.IsValid(to) == false)
                return;

            _gridView.StopActiveTweens();
            _isBusy = true;
            bool isLoopActive = await TrySwap(from, to, _matchInfoBuffer);

            if (isLoopActive && _gridState[from.x, from.y].isBonus)
                ProcessBonusDestruction(from, _gridState[to.x, to.y].type, _bonusDestructBuffer);

            if (isLoopActive && _gridState[to.x, to.y].isBonus)
                ProcessBonusDestruction(to, _gridState[from.x, from.y].type, _bonusDestructBuffer);

            while (isLoopActive)
            {
                await ProcessMatchBonusCreation(_matchInfoBuffer, to);
                ProcessMatchesDestruction(_matchInfoBuffer, _matchedPosesBuffer);
                await VisualizeDestruction(_matchedPosesBuffer, _bonusDestructBuffer);
                await ProcessFalling();

                isLoopActive = TryFindMatches(_matchInfoBuffer);
                if (isLoopActive)
                    continue;

                await ProcessShapeCreation();
                isLoopActive = TryFindMatches(_matchInfoBuffer);
            }

            if (_matchPredictService.TryFindMatchPrediction(_gridState, out var matchPrediction))
            {
                _gridView.VisualizeMatchHint(matchPrediction, RuntimeConstants.Test.HINT_TIME_DELAY);
            }
            else
            {
                _shapeStatesCreateService.ConfigureNewGridState(ref _gridState);
                await _gridView.VisualizeGridReconfiguration(_gridState);
            }

            _isBusy = false;
        }

        private bool TryFindMatches(List<ShapeMatchInfo> matches)
        {
            matches.Clear();
            _findMatchService.FindMatches(_gridState, matches);
            return matches.Count > 0;
        }

        private async UniTask<bool> TrySwap(ShapePos from, ShapePos to, List<ShapeMatchInfo> matches)
        {
            bool isBonusSwap = _gridState.IsValid(to) &&
                               (_gridState[from.x, from.y].isBonus || _gridState[to.x, to.y].isBonus);

            _gridState.Swap(from, to);
            if (isBonusSwap)
            {
                await _gridView.VisualizeSwap(from, to);
                return true;
            }

            if (TryFindMatches(matches))
            {
                await _gridView.VisualizeSwap(from, to);
                return true;
            }

            if (_gridState.IsValid(to))
                await _gridView.VisualizeFalseSwap(from, to);

            _gridState.Swap(to, from);
            return false;
        }

        private void ProcessMatchesDestruction(List<ShapeMatchInfo> matches, List<ShapePos> destroyedShapes)
        {
            foreach (var match in matches)
                _shapesDestroyService.ProcessMatch(ref _gridState, match, destroyedShapes);
        }

        private void ProcessBonusDestruction(ShapePos bonus, ShapeType target, Stack<ShapeBonusProcessInfo> chain) =>
            _shapesDestroyService.ProcessBonus(ref _gridState, bonus, target, chain);

        private async UniTask VisualizeDestruction(List<ShapePos> matchedShapes,
            Stack<ShapeBonusProcessInfo> destructionChain)
        {
            if (matchedShapes.Count > 0)
            {
                await _gridView.VisualizeDestruction(matchedShapes);
                matchedShapes.Clear();
            }

            if (destructionChain.Count > 0)
            {
                await _gridView.VisualizeDestruction(destructionChain);
                destructionChain.Clear();
            }
        }

        private async UniTask ProcessFalling()
        {
            _fallInfoBuffer.Clear();
            _shapesFallService.ProcessFalling(ref _gridState, _fallInfoBuffer);
            if (_fallInfoBuffer.Count > 0)
            {
                await _gridView.VisualizeFalling(_fallInfoBuffer);
                _fallInfoBuffer.Clear();
            }
        }

        private async UniTask ProcessShapeCreation()
        {
            _createInfoBuffer.Clear();
            _shapeStatesCreateService.CreateNewShapeStates(ref _gridState, _createInfoBuffer);
            if (_createInfoBuffer.Count > 0)
            {
                await _gridView.CreateNewShapeViews(_createInfoBuffer);
                _createInfoBuffer.Clear();
            }
        }

        private async UniTask ProcessMatchBonusCreation(List<ShapeMatchInfo> matches, ShapePos bonusPos)
        {
            if (matches.Count == 0)
                return;

            _bonusGenInfoBuffer.Clear();
            _bonusCreateService.ProcessMatchBonusCreation(ref _gridState, bonusPos, matches, _bonusGenInfoBuffer);
            if (_bonusGenInfoBuffer.Count > 0)
            {
                await _gridView.CreateMatchBonusShapeViews(_bonusGenInfoBuffer);
                _bonusGenInfoBuffer.Clear();
            }
        }
    }
}