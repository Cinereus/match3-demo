using System;
using Code.Match3;
using Code.Runtime.Infrastructure.Pool;
using Code.Runtime.Infrastructure.StaticData;
using DG.Tweening;
using UnityEngine;

namespace Code.Runtime.Match3
{
    public class Match3ShapeView : MonoBehaviour, IPoolable<Match3ShapeView>
    {
        [SerializeField]
        private SpriteRenderer _renderer;
        
        public event Action<Match3ShapeView> onReturnToPool;
        public Color vfxColor { get; private set; } = Color.white;

        public void Initialize(Match3ShapeViewData viewData, Vector3 position)
        {
            vfxColor = viewData.vfxColor;
            _renderer.sprite = viewData.sprite;
            transform.position = position;
        }
        
        public void Initialize(Match3BonusShapeViewData viewData, Vector3 position)
        {
            vfxColor = Color.white;
            _renderer.sprite = viewData.sprite;
            transform.position = position;
        }
        
        public Tweener MoveTo(Vector3 point, float duration) =>
            transform.DOMove(point, duration);
        
        public Sequence Disappear()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOPunchScale(Vector3.one * 0.5f, 0.1f));
            seq.Append(transform.DOScale(0, 0.1f));
            seq.onComplete += () => onReturnToPool?.Invoke(this);
            return seq;
        }

        public Sequence Appear()
        {
            Sequence seq = DOTween.Sequence();
            transform.localScale = Vector3.zero;
            seq.Append(transform.DOScale(1, 0.02f));
            seq.Append(transform.DOPunchScale(Vector3.one * 0.5f, 0.02f));
            return seq;
        }
    }
}