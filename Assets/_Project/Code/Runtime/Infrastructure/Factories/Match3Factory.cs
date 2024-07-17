using System;
using System.Collections.Generic;
using Code.Match3;
using Code.Runtime.Infrastructure.Pool;
using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Infrastructure.StaticData;
using Code.Runtime.Match3;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Runtime.Infrastructure.Factories
{
    public class Match3Factory : ILoadUnit<Match3LevelConfig>, IMatch3Factory
    {
        private readonly Dictionary<ShapeType, Match3ShapeViewData> _shapeViewData = new Dictionary<ShapeType, Match3ShapeViewData>();
        private readonly Dictionary<ShapeBonusType, Match3BonusShapeViewData> _bonusShapeViewData = new Dictionary<ShapeBonusType, Match3BonusShapeViewData>();
        private ObjectPool<Match3ShapeView> _shapePool;
        private GameObject _gridSlotPrefab;
        private GameObject _gridShapePrefab;
        private GameObject _environmentPrefab;

        public Vector2 shapeSize  => _gridShapePrefab.GetComponent<SpriteRenderer>().size;
        
        public UniTask Load(Match3LevelConfig config)
        {
            int poolSize = config.gridWidth * config.gridHeight;
            _shapePool = new ObjectPool<Match3ShapeView>(poolSize, CreateShapePoolInstance);
            _gridSlotPrefab = Resources.Load<GameObject>(RuntimeConstants.Assets.MATCH3_GRID_SLOT);
            _gridShapePrefab =  Resources.Load<GameObject>(RuntimeConstants.Assets.MATCH3_GRID_SHAPE);
            
            string environmentPath = string.Empty;
            string shapeViewsPath = string.Empty;
            if (config.visualStyle == Match3VisualType.HUNGRY_BAT)
            {
                environmentPath = RuntimeConstants.Assets.HUNGRY_BAT_ENVIRONMENT;
                shapeViewsPath = RuntimeConstants.Assets.HUNGRY_BAT_SHAPE_VIEWS;
            }

            Match3ShapeViewsConfig shapeViewsConfig = Resources.Load<Match3ShapeViewsConfig>(shapeViewsPath);
            if (shapeViewsConfig.shapeViewData.Count < config.shapesTypesCount) 
                throw new Exception($"[{nameof(Match3Factory)}] Failed to load shape view data. Data count less than configuration!");
            
            foreach (var data in shapeViewsConfig.shapeViewData) 
                _shapeViewData[data.type] = data;
            
            foreach (var data in shapeViewsConfig.bonusShapeViewData) 
                _bonusShapeViewData[data.type] = data;
            
            _environmentPrefab = Resources.Load<GameObject>(environmentPath);
            return UniTask.CompletedTask;
        }
        
        public GameObject CreateEnvironment(Transform parent) => 
            Object.Instantiate(_environmentPrefab, Vector3.zero, Quaternion.identity, parent);

        public Match3ShapeView CreateShape(Vector3 position, ShapeType type, Transform parent)
        {
            if (_shapeViewData.TryGetValue(type, out Match3ShapeViewData data))
            {
                Match3ShapeView shape = _shapePool.Get();
                shape.transform.SetParent(parent);
                shape.gameObject.SetActive(true);
                shape.Initialize(data, position);
                shape.onReturnToPool += OnShapeReturnToPool;
                return shape;
            }
            Debug.LogError($"[{nameof(Match3Factory)}] Failed to create shape. View data for type \"{type}\" not found!");
            return null;
        }

        public Match3ShapeView CreateShape(Vector3 position, ShapeBonusType bonusType, Transform parent)
        {
            if (_bonusShapeViewData.TryGetValue(bonusType, out Match3BonusShapeViewData data))
            {
                Match3ShapeView bonus = _shapePool.Get();
                bonus.transform.SetParent(parent);
                bonus.gameObject.SetActive(true);
                bonus.Initialize(data, position);
                bonus.onReturnToPool += OnShapeReturnToPool;
                return bonus;
            }
            Debug.LogError($"[{nameof(Match3Factory)}] Failed to create shape. View data for type \"{bonusType}\" not found!");
            return null;
        }

        private void OnShapeReturnToPool(Match3ShapeView shape)
        {
            shape.onReturnToPool -= OnShapeReturnToPool;
            shape.gameObject.SetActive(false);
            _shapePool.Return(shape);
        }

        public GameObject CreateGridSlot(Vector3 position, Transform parent) =>
            Object.Instantiate(_gridSlotPrefab, position, Quaternion.identity, parent);

        private Match3ShapeView CreateShapePoolInstance() =>
            Object.Instantiate(_gridShapePrefab, Vector3.zero, Quaternion.identity)
                .GetComponent<Match3ShapeView>();
    }
}