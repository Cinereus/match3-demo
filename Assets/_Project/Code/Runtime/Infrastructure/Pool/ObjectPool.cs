using System;
using System.Collections.Generic;

namespace Code.Runtime.Infrastructure.Pool
{
    public class ObjectPool<T> where T : IPoolable<T>
    {
        private readonly Stack<T> _stack;
        private readonly IPoolCreator<T> _creator;

        public ObjectPool(int poolSize, IPoolCreator<T> creator)
        {
            _stack = new Stack<T>(poolSize);
            _creator = creator;
        }

        public T Get() => 
            _stack.TryPop(out T result) ? result : _creator.CreateNew();

        public void Return(T obj) => 
            _stack.Push(obj);
    }

    public interface IPoolable<T>
    {
        event Action<T> onReturnToPool;
    }

    public interface IPoolCreator<T>
    {
        T CreateNew();
    }
}