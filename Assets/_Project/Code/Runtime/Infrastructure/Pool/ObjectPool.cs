using System;
using System.Collections.Generic;

namespace Code.Runtime.Infrastructure.Pool
{
    public class ObjectPool<T> where T : IPoolable<T>, new()
    {
        private readonly Stack<T> _stack;
        private readonly Func<T> _creator;

        public ObjectPool(int poolSize = 10, Func<T> creator = null)
        {
            _stack = new Stack<T>(poolSize);
            _creator = creator;
        }

        public T Get() => 
            _stack.TryPop(out T result) ? result : CreateNew();

        public void Return(T obj) => 
            _stack.Push(obj);

        private T CreateNew() => 
            _creator != null ? _creator.Invoke() : new T();
    }

    public interface IPoolable<T>
    {
        event Action<T> onReturnToPool;
    }
}