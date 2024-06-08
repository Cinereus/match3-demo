using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Logic.Match3;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

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

        public GameObject CreateShape(Vector3 position, Match3ShapeType type)
        {
            GameObject shape = Object.Instantiate(_gridShapePrefab, position, Quaternion.identity);
            var shapeView = shape.GetComponent<Match3ShapeView>();
            
            // test generation
            switch (type)
            {
                case Match3ShapeType.A:
                    shapeView.type = Match3ShapeType.A;
                    shapeView.color = Color.grey;
                    break;
                case Match3ShapeType.B:
                    shapeView.type = Match3ShapeType.B;
                    shapeView.color = Color.cyan;
                    break;
                case Match3ShapeType.C:
                    shapeView.type = Match3ShapeType.C;
                    shapeView.color = Color.yellow;
                    break;
                case Match3ShapeType.D:
                    shapeView.type = Match3ShapeType.D;
                    shapeView.color = Color.red;
                    break;
                case Match3ShapeType.E:
                    shapeView.type = Match3ShapeType.E;
                    shapeView.color = Color.green;
                    break;
                case Match3ShapeType.F:
                    shapeView.type = Match3ShapeType.F;
                    shapeView.color = Color.blue;
                    break;
                case Match3ShapeType.G:
                    shapeView.type = Match3ShapeType.G;
                    shapeView.color = Color.magenta;
                    break;
            }
            return shape;
        }

        public GameObject CreateGridSlot(Vector3 position) =>
            Object.Instantiate(_gridSlotPrefab, position, Quaternion.identity);
    }
}