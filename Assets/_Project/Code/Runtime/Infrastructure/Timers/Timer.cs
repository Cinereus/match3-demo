using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Runtime.Infrastructure.Timers
{
    public class Timer : IDisposable
    {
        public event Action<int> onTick;
        public event Action onFinished;
        public bool isActive => _time > 0;
        
        private int _time;
        private int _intervalMillis;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

        public void Start(int time, int intervalMillis = 1000)
        {
            if (_time > 0)
                _tokenSource.Cancel();
            
            _time = time;
            _intervalMillis = intervalMillis;
            StartTimer(_tokenSource.Token).Forget();
        }

        public void Stop()
        {
            _tokenSource.Cancel();
            onFinished?.Invoke();
        }

        public void Dispose()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }
        
        private async UniTaskVoid StartTimer(CancellationToken token)
        {
            while (_time > 0)
            {
                await UniTask.Delay(_intervalMillis, cancellationToken: token);

                if (token.IsCancellationRequested)
                {
                    Debug.LogError("got after cancellation");
                    break;
                }
                
                _time--;
                onTick?.Invoke(_time);
                
                if (_time == 0)
                    onFinished?.Invoke();
            }
        }
    }
}