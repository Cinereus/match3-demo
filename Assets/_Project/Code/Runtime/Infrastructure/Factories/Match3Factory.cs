using System.Threading;
using Code.Match3;
using Code.Runtime.Infrastructure.Pool;
using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Match3;
using Code.Runtime.UI.Screens;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Runtime.Infrastructure.Factories
{
    public class Match3Factory : ILoadUnit<Match3Factory.Params>, IMatch3Factory
    {
        public readonly struct Params
        {
            public readonly int shapePoolSize;
            public readonly int goalsPoolSize;

            public Params(int shapePoolSize, int goalsPoolSize)
            {
                this.goalsPoolSize = goalsPoolSize;
                this.shapePoolSize = shapePoolSize;
            }
        }
        
        public Vector2 shapeSize  => _assetProvider.gridShapePrefab.GetComponent<SpriteRenderer>().size;
        
        private readonly Match3AssetProvider _assetProvider;
        private GameObjectPool<Match3ShapeView> _shapePool;
        private GameObjectPool<Match3ShapeGoalView> _goalsPool;

        public Match3Factory(Match3AssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        
        public UniTask Load(Params param, CancellationToken token)
        {
            _shapePool = new GameObjectPool<Match3ShapeView>(_assetProvider.gridShapePrefab, param.shapePoolSize);
            _goalsPool = new GameObjectPool<Match3ShapeGoalView>(_assetProvider.goalShapePrefab, param.goalsPoolSize);
            _shapePool.Initialize();
            _goalsPool.Initialize();
            return UniTask.CompletedTask;
        }
        
        public GameObject CreateEnvironment(Transform parent) => 
            Object.Instantiate(_assetProvider.environmentPrefab, Vector3.zero, Quaternion.identity, parent);

        public Match3ShapeView CreateShape(Vector3 position, ShapeType type, Transform parent)
        {
            Match3ShapeView shape = _shapePool.Get();
            shape.transform.SetParent(parent);
            shape.Initialize(_assetProvider.GetShapeViewData(type), position);
            return shape;
        }

        public Match3ShapeView CreateShape(Vector3 position, ShapeBonusType bonusType, Transform parent)
        { 
            Match3ShapeView bonus = _shapePool.Get();
            bonus.transform.SetParent(parent);
            bonus.Initialize(_assetProvider.GetBonusShapeViewData(bonusType), position);
            return bonus;
        }

        public GameObject CreateGridSlot(Vector3 position, Transform parent) =>
            Object.Instantiate(_assetProvider.gridSlotPrefab, position, Quaternion.identity, parent);
        
        public Match3ShapeGoalView CreateGoalView(ShapeType shapeType, int count)
        {
            Match3ShapeGoalView goalView = _goalsPool.Get();
            goalView.Initialize(_assetProvider.GetShapeViewData(shapeType).sprite, shapeType, count);
            return goalView;
        }
    }
}