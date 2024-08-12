using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.Runtime.UI.Screens
{
    public class BonusUseProgressBarView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _progressCount;
        
        [SerializeField]
        private Image _fill;
        
        [SerializeField]
        private float _animationDuration = 0.2f;
        
        private const string PROGRESS_TEXT = "{current}/{max}";
        private int _max;
        private Tween _animationTween;

        public void Initialize(int max)
        {
            _max = max;
            _fill.fillAmount = 0;
            SetText(0);
        }

        public void SetProgress(int current)
        {
            AnimateProgress((float) current / _max);
            SetText(current);
        }

        private void SetText(int current)
        {
            _progressCount.text = PROGRESS_TEXT
                .Replace("{current}", current.ToString())
                .Replace("{max}", _max.ToString());
        }

        private void InvokeProgressDone()
        {
            AnimateProgress(0);
            SetText(0);
        }

        private void AnimateProgress(float current)
        {
            if (_animationTween != null && _animationTween.IsPlaying())
            {
                _animationTween.onComplete -= InvokeProgressDone;
                _animationTween.Kill();
            }
            
            _animationTween = _fill.DOFillAmount(current, _animationDuration);

            if (current >= 1)
            { 
                _animationTween.onComplete += InvokeProgressDone;
            }
        }
    }
}