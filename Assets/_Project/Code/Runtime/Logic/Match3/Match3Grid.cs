using Code.Runtime.Infrastructure.Factories;
using Code.Runtime.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Runtime.Logic.Match3
{
    public struct Match3Grid : ILoadUnit<Transform>
    {
        private readonly Match3Slot[,] _slots;
        private readonly Match3ShapeView[,] _shapeViews;
        private readonly Match3Factory _factory;
        
        // todo move to config
        private const int LENGTH = 12;

        public Match3Grid(Match3Factory factory)
        {
            _slots = new Match3Slot[LENGTH, LENGTH];
            _shapeViews = new Match3ShapeView[LENGTH, LENGTH];
            _factory = factory;
        }

        public UniTask Load(Transform spawnPoint)
        {
            const int HALF = 2;
            const float SPACING = 1.2f;
            
            float elementSize = _factory.gridSlotPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
            float hOffset = (LENGTH - elementSize) * SPACING / HALF;
            float vOffset = (LENGTH - elementSize) * SPACING / HALF;
            
            for (int i = 0; i < LENGTH; i++)
            {
                for (int j = 0; j < LENGTH; j++)
                {
                    var cellPos = spawnPoint.position;
                    cellPos.x += i * SPACING - hOffset;
                    cellPos.y += j * SPACING - vOffset;
                    _slots[i, j] = _factory.CreateGridSlot(cellPos).GetComponent<Match3Slot>();
                    _shapeViews[i, j] = _factory.CreateShape(cellPos, Random.Range(0, 7))
                        .GetComponent<Match3ShapeView>();
                }
            }
            
            return UniTask.CompletedTask;
        }
    }
}