using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Runtime.Infrastructure.Pool
{
    public class GameObjectPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private readonly T _prefab;
        private readonly Stack<T> _stack;
        private readonly Transform _parent;
        private readonly int _poolSize;
        private bool _isInitialized;

        public GameObjectPool(T prefab, int poolSize = 100, Transform parent = null)
        {
            _stack = new Stack<T>(poolSize);
            _prefab = prefab;
            _parent = parent;
            _poolSize = poolSize;
        }

        public void Initialize()
        {
            if (_isInitialized)
                return;
            
            for (int i = 0; i < _poolSize; i++)
                _stack.Push(Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity, _parent));

            _isInitialized = true;
        }

        public T Get()
        {
            if (_stack.TryPop(out T result))
            {
                result.gameObject.SetActive(true);
                result.transform.SetParent(null);
            }
            else
            {
                result = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity);   
            }
            
            result.onReturnToPoolRequested += Return;
            return result;
        }

        public void Return(T poolable)
        {
            poolable.onReturnToPoolRequested -= Return;
            poolable.gameObject.SetActive(false);
            poolable.transform.SetParent(_parent);
            _stack.Push(poolable);
        }
    }

    public interface IPoolable<T>
    {
        event Action<T> onReturnToPoolRequested;
    }
}