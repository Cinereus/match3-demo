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

        public event Action<Match3ShapeView> onReturnToPoolRequested;
        public ShapeType type { get; private set; } = ShapeType.NONE;
        public ShapeBonusType bonusType { get; private set; } = ShapeBonusType.NONE;

        public void Initialize(Match3ShapeViewData viewData, Vector3 position)
        {
            type = viewData.type;
            bonusType = viewData.bonusType;
            transform.position = position;
            _renderer.sprite = viewData.sprite;
        }

        public Tweener MoveTo(Vector3 point, float duration) => 
            transform.DOMove(point, duration);
        
        public Sequence Appear(float duration)
        {
            Sequence seq = DOTween.Sequence();
            transform.localScale = Vector3.zero;
            seq.Append(transform.DOScale(1, duration));
            seq.Append(transform.DOPunchScale(Vector3.one * 0.5f, duration));
            return seq;
        }
        
        public Sequence Disappear(float duration)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOScale(0.2f, duration));
            seq.onComplete += ReturnToPool;
            return seq;
        }

        private void ReturnToPool()
        {
            type = ShapeType.NONE;
            bonusType = ShapeBonusType.NONE;
            _renderer.sprite = null;
            onReturnToPoolRequested?.Invoke(this);
        }
    }
}