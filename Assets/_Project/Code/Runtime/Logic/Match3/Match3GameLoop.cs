using System.Collections.Generic;
using Code.Runtime.Infrastructure.Factories;
using Code.Runtime.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Code.Runtime.Logic.Match3
{
    public class Match3GameLoop : ILoadUnit, IStartable, ITickable
    {
        private readonly Match3GridState _gridState;
        private readonly Match3GridView _gridView;
        private readonly Match3Factory _match3Factory;
        private readonly IMatchSearchService _matchSearchService;
        private readonly ShapesFallService _shapesFallService;
        private readonly List<Match> _matchBuffer = new List<Match>();

        public Match3GameLoop(Match3GridState gridState, Match3GridView gridView, Match3Factory match3Factory,
            IMatchSearchService matchSearchService, ShapesFallService shapesFallService)
        {
            _gridState = gridState;
            _gridView = gridView;
            _match3Factory = match3Factory;
            _matchSearchService = matchSearchService;
            _shapesFallService = shapesFallService;
        }

        public UniTask Load()
        {
            _gridState.ConfigureStartGridState();
            _gridView.GenerateStartShapes(_gridState);
            return UniTask.CompletedTask;
        }

        public void DoMove(Match3Move move)
        {
            _gridState.Swap(move.from, move.to);
            _matchSearchService.FindMatches(_gridState, _matchBuffer);
            
            if (_matchBuffer.Count == 0)
                _gridState.Swap(move.to, move.from);

            _gridView.VisualizeMove(move);
        }

        public void Tick()
        {

        }

        public void Start()
        {
        }
    }
}