using System;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace Code.Runtime.Infrastructure.Services
{
    public class LoadingService
    {
        private readonly Stopwatch _watch = new Stopwatch();

        public async UniTask Load(ILoadUnit loadUnit, CancellationToken token = default, bool skipExceptionThrow = false)
        {
            var isError = true;
            string unitName = loadUnit.GetType().Name;

            try {
                OnLoadingBegin(unitName);
                await loadUnit.Load(token);
                isError = false;
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                if (!skipExceptionThrow)
                    throw;
            }
            finally {
                await OnLoadingFinish(unitName, isError, token);
            }
        }

        public async UniTask Load<TParam>(ILoadUnit<TParam> loadUnit, TParam param, CancellationToken token = default,
            bool skipExceptionThrow = false)
        {
            var isError = true;
            string unitName = loadUnit.GetType().Name;

            try {
                OnLoadingBegin(unitName);
                await loadUnit.Load(param, token);
                isError = false;
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                if (!skipExceptionThrow)
                    throw;
            }
            finally {
                await OnLoadingFinish(unitName, isError, token);
            }
        }

        private void OnLoadingBegin(string loadUnitName)
        {
            _watch.Restart();
            Debug.Log(loadUnitName + " loading is started");
        }

        private async UniTask OnLoadingFinish(string unitName, bool isError, CancellationToken token)
        {
            _watch.Stop();
            Debug.Log($"{unitName} is {(isError ? "NOT" : "")} loaded with time {_watch.ElapsedMilliseconds}ms");

            int currentThreadId = Thread.CurrentThread.ManagedThreadId;
            int mainThreadId = PlayerLoopHelper.MainThreadId;

            if (mainThreadId != currentThreadId) {
                _watch.Restart();
                
                if (token.IsCancellationRequested)
                    _watch.Stop();
                
                Debug.Log($"[THREAD] start switching from '{currentThreadId}' thread to main thread '{mainThreadId}'");
                await UniTask.SwitchToMainThread(cancellationToken: token);
                _watch.Stop();
                Debug.Log($"[THREAD] switch finished with time {_watch.ElapsedMilliseconds}");
            }
        }
    }
}