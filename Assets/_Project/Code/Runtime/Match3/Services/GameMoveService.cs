using System;
using System.Threading;
using Code.Match3;
using Code.Runtime.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Runtime.Match3.Services
{
    public class GameMoveService : ILoadUnit<GameMoveService.Params>, IDisposable
    {
        public readonly struct Params
        {
            public readonly float shapeSize;
            public readonly Vector3 distancePoint;

            public Params(float shapeSize, Vector3 distancePoint)
            {
                this.shapeSize = shapeSize;
                this.distancePoint = distancePoint;
            }
        }
        
        public event Action<ShapePos, ShapePos> onMove;
        
        private readonly InputService _inputService;

        private Camera _camera;
        private float _shapeSize;
        private Vector3 _distancePoint;
        private ShapePos _startDragPosition;

        public GameMoveService(InputService inputService)
        {
            _inputService = inputService;
        }
        
        public UniTask Load(Params param, CancellationToken token)
        {
            _camera = Camera.main;
            _shapeSize = param.shapeSize;
            _distancePoint = param.distancePoint;
            _inputService.onBeginDrag += OnBeginDrag;
            _inputService.onDrag += OnDrag;
            return UniTask.CompletedTask;
        }

        public void Dispose()
        { 
            _inputService.onBeginDrag -= OnBeginDrag;
            _inputService.onDrag -= OnDrag;
        }

        private void OnBeginDrag(Vector2 position)
        {
            if (_camera == null)
                return;
            
            _startDragPosition = _camera.ScreenToShapePos(position, _distancePoint, _shapeSize);
        }

        private void OnDrag(Vector2 position)
        {
            if (_camera == null)
                return;
            
            var dragThreshold = 0.8f;
            ShapePos dragPos = _camera.ScreenToShapePos(position, _distancePoint, _shapeSize);
            ShapePos dragDistance = dragPos - _startDragPosition;
            ShapePos targetPos = _startDragPosition + dragDistance 
                switch 
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