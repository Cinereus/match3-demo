using System;
using UnityEngine;
using VContainer.Unity;

namespace Code.Runtime.Infrastructure.Services
{
    public class InputService : ITickable
    {
        public event Action<Vector2> onBeginDrag;
        public event Action<Vector2> onDrag;
        public event Action<Vector2> onEndDrag;

        public void Tick()
        {
            if (Input.GetMouseButtonDown(0)) 
                onBeginDrag?.Invoke(Input.mousePosition);

            if (Input.GetMouseButton(0)) 
                onDrag?.Invoke(Input.mousePosition);

            if (Input.GetMouseButtonUp(0)) 
                onEndDrag?.Invoke(Input.mousePosition);
        }
    }
}