using Code.Runtime.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Runtime.Infrastructure.Factories
{
    public class Match3Factory : ILoadUnit
    {
        private GameObject _gridSlotPrefab;
        private GameObject _gridShapePrefab;

        public GameObject gridSlotPrefab => _gridSlotPrefab;
        
        public UniTask Load()
        {
            _gridSlotPrefab = Resources.Load<GameObject>(RuntimeConstants.Assets.MATCH3_GRID_SLOT);
            _gridShapePrefab = Resources.Load<GameObject>(RuntimeConstants.Assets.MATCH3_GRID_SHAPE);
            return UniTask.CompletedTask;
        }

        public GameObject CreateShape(Vector3 position, int spriteIndex)
        {
            GameObject shape = Object.Instantiate(_gridShapePrefab, position, Quaternion.identity);
            var shapeRenderer = shape.GetComponent<SpriteRenderer>();
            
            // test generation
            Color targetColor = Color.white;
            switch (spriteIndex)
            {
                case 0:
                    targetColor = Color.grey;
                    break;
                case 1:
                    targetColor = Color.cyan;
                    break;
                case 2:
                    targetColor = Color.yellow;
                    break;
                case 3:
                    targetColor = Color.red;
                    break;
                case 4:
                    targetColor = Color.green;
                    break;
                case 5:
                    targetColor = Color.blue;
                    break;
                case 6:
                    targetColor = Color.magenta;
                    break;
            }
            shapeRenderer.color = targetColor;
            return shape;
        }

        public GameObject CreateGridSlot(Vector3 position) =>
            Object.Instantiate(_gridSlotPrefab, position, Quaternion.identity);
    }
}