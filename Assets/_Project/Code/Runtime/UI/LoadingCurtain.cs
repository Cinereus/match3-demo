using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Runtime.UI
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField]
        private float _fadeDuration = 0.3f;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            gameObject.SetActive(false);
            DontDestroyOnLoad(this);
        }

        public UniTask Show(CancellationToken token = default)
        {
            _canvasGroup.alpha = 0;
            gameObject.SetActive(true);
            return _canvasGroup.DOFade(1, _fadeDuration).ToUniTask(cancellationToken: token);
        }

        public async UniTask Hide(CancellationToken token = default)
        {
            if (gameObject.activeSelf == false)
                return;

            if (token.IsCancellationRequested)
            {
                gameObject.SetActive(false);
                transform.SetParent(null);
            }

            await _canvasGroup.DOFade(0, _fadeDuration).ToUniTask(cancellationToken: token);
            gameObject.SetActive(false);
        }
    }
}