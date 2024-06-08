using Code.Runtime.Infrastructure.Factories;
using Code.Runtime.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace Code.Runtime.Logic.Match3
{
    public class Match3GridView : ILoadUnit<Vector3>, ITickable
    {
        private readonly Match3Factory _match3Factory;
        private Match3Grid<Match3ShapeView> _shapes;
        private Vector3[,] _positions;

        public Match3GridView(Match3Factory match3Factory)
        {
            _match3Factory = match3Factory;
        }

        public UniTask Load(Vector3 spawnPoint)
        {
            const float SPACING = 1.2f;
            int size = RuntimeConstants.Test.GRID_LENGTH;

            _positions = new Vector3[size, size];
            _shapes = new Match3Grid<Match3ShapeView>(new Match3ShapeView[size, size]);
            float elementSize = _match3Factory.gridSlotPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
            float offset = (RuntimeConstants.Test.GRID_LENGTH - elementSize) / 2 * SPACING;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Vector3 position = spawnPoint;
                    position.x += i * SPACING - offset;
                    position.y += j * SPACING - offset;

                    _positions[i, j] = position;
                    _match3Factory.CreateGridSlot(position);
                }
            }
            return UniTask.CompletedTask;
        }

        public void Tick()
        {
        }
        
        public async void VisualizeMove(Match3Move move)
        {
            
        }
        
        public void GenerateStartShapes(Match3GridState gridState)
        {
            for (int i = 0; i < gridState.size.x; i++)
            {
                for (int j = 0; j < gridState.size.y; j++)
                {
                    _shapes[i, j] = _match3Factory.CreateShape(_positions[i, j], gridState[i, j])
                        .GetComponent<Match3ShapeView>();
                }   
            }
        }
    }
}