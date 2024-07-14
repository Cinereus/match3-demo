using Code.Match3;
using Code.Runtime.Infrastructure.Pool;
using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Match3;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Runtime.Infrastructure.Factories
{
    public class Match3Factory : ILoadUnit, IMatch3Factory, IPoolCreator<Match3ShapeView>
    {
        private ObjectPool<Match3ShapeView> _shapePool;
        private GameObject _gridSlotPrefab;
        private GameObject _gridShapePrefab;

        public Vector2 shapeSize  => _gridSlotPrefab.GetComponent<SpriteRenderer>().size;
        
        public UniTask Load()
        {
            int poolSize = RuntimeConstants.Test.GRID_SIZE * RuntimeConstants.Test.GRID_SIZE;
            _shapePool = new ObjectPool<Match3ShapeView>(poolSize, this);
            _gridSlotPrefab = Resources.Load<GameObject>(RuntimeConstants.Assets.MATCH3_GRID_SLOT);
            _gridShapePrefab = Resources.Load<GameObject>(RuntimeConstants.Assets.MATCH3_GRID_SHAPE);
            return UniTask.CompletedTask;
        }

        public Match3ShapeView CreateShape(Vector3 position, ShapeType type)
        {
            Match3ShapeView shape = _shapePool.Get();
            shape.gameObject.SetActive(true);
            shape.gameObject.name = shape.gameObject.name.Replace("_in_pool", "");
            shape.Initialize(type, position);
            shape.onReturnToPool += OnShapeReturnToPool;
            return shape;
        }

        public Match3ShapeView CreateShape(Vector3 position, ShapeBonusType bonusType)
        {
            Match3ShapeView bonus = _shapePool.Get();
            bonus.gameObject.SetActive(true);
            bonus.Initialize(bonusType, position);
            bonus.onReturnToPool += OnShapeReturnToPool;
            return bonus;
        }

        private void OnShapeReturnToPool(Match3ShapeView shape)
        {
            shape.onReturnToPool -= OnShapeReturnToPool;
            shape.gameObject.SetActive(false);
            _shapePool.Return(shape);
        }

        public GameObject CreateGridSlot(Vector3 position) =>
            Object.Instantiate(_gridSlotPrefab, position, Quaternion.identity);

        public Match3ShapeView CreateNew()
        {
            GameObject shapeObj = Object.Instantiate(_gridShapePrefab, Vector3.zero, Quaternion.identity);
            return shapeObj.GetComponent<Match3ShapeView>();
        }
    }
}