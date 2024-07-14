using System;
using Code.Match3;
using Code.Runtime.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Runtime.Match3.Services
{
    public class GameMoveService : ILoadUnit<Vector3>
    {
        public event Action<ShapePos, ShapePos> onMove;
        
        private readonly InputService _inputService;
        
        private Camera _camera;
        private Vector3 _distancePoint;
        private ShapePos _startDragPosition;

        public GameMoveService(InputService inputService)
        {
            _inputService = inputService;
        }
        
        public UniTask Load(Vector3 distancePoint)
        {
            _camera = Camera.main;
            _distancePoint = distancePoint;
            _inputService.onBeginDrag += OnBeginDrag;
            _inputService.onDrag += OnDrag;
            return UniTask.CompletedTask;
        }

        private void OnBeginDrag(Vector2 position)
        {
            _startDragPosition = _camera.ScreenToShapePos(position, _distancePoint);
        }

        private void OnDrag(Vector2 position)
        {
            var dragThreshold = 0.8f;
            ShapePos dragPos = _camera.ScreenToShapePos(position, _distancePoint);
            ShapePos dragDistance = dragPos - _startDragPosition;
            ShapePos targetPos = _startDragPosition + dragDistance switch
            {
                _ when dragDistance.x > dragThreshold => new ShapePos(1, 0),
                _ when dragDistance.x < -dragThreshold => new ShapePos(-1, 0),
                _ when dragDistance.y > dragThreshold => new ShapePos(0, 1),
                _ when dragDistance.y < -dragThreshold => new ShapePos(0, -1),
                _ => ShapePos.zero
            };

            if (targetPos == _startDragPosition || _startDragPosition == -ShapePos.one)
                return;

            onMove?.Invoke(_startDragPosition, targetPos);
            _startDragPosition = -ShapePos.one;
        }
    }
}