using System;
using Code.Match3;
using Code.Runtime.Infrastructure.Pool;
using DG.Tweening;
using UnityEngine;

namespace Code.Runtime.Match3
{
    public class Match3ShapeView : MonoBehaviour, IPoolable<Match3ShapeView>
    {
        [SerializeField]
        private Sprite _defaultSprite;
        
        [SerializeField]
        private Sprite _crossSprite;
        
        [SerializeField]
        private Sprite _bombSprite;
        
        [SerializeField]
        private Sprite _colorSprite;
        
        [SerializeField]
        private SpriteRenderer _renderer;

        [SerializeField]
        private Color _customShapeColor;
        
        [SerializeField]
        private Color _bombColor;
        
        [SerializeField]
        private Color _crossColor;
        
        [SerializeField]
        private Color coloredBonusColor;
        
        
        public ShapeType type = ShapeType.NONE;
        public ShapeBonusType bonusType = ShapeBonusType.NONE;
        
        public event Action<Match3ShapeView> onReturnToPool;

        public void Initialize(ShapeType shape, Vector3 position)
        {
            type = shape;
            transform.position = position;
            _renderer.sprite = _defaultSprite;
            switch (shape)
            {
                case ShapeType.A:
                    _renderer.color = _customShapeColor;
                    break;
                case ShapeType.B:
                    _renderer.color = Color.cyan;
                    break;
                case ShapeType.C:
                    _renderer.color = Color.yellow;
                    break;
                case ShapeType.D:
                    _renderer.color = Color.red;
                    break;
                case ShapeType.E:
                    _renderer.color = Color.green;
                    break;
                case ShapeType.F:
                    _renderer.color = Color.blue;
                    break;
                case ShapeType.G:
                    _renderer.color = Color.magenta;
                    break;
            }
        }
        
        public void Initialize(ShapeBonusType shape, Vector3 position)
        {
            bonusType = shape;
            transform.position = position;
            switch (shape)
            {
                case ShapeBonusType.BOMB:
                    _renderer.sprite = _bombSprite;
                    _renderer.color = _bombColor;
                    break;
                case ShapeBonusType.CROSS:
                    _renderer.sprite = _crossSprite;
                    _renderer.color = _crossColor;
                    break;
                case ShapeBonusType.COLOR:
                    _renderer.sprite = _colorSprite;
                    _renderer.color = coloredBonusColor;
                    break;
            }
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

        public Tween ChangeType(ShapeType shapeType)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOPunchScale(Vector3.one * 0.5f, 0.1f));
            var scaleTween = transform.DOScale(0, 0.1f);
            scaleTween.onComplete += () => Initialize(shapeType, transform.position);
            seq.Append(scaleTween);
            seq.Append(transform.DOScale(1, 0.02f));
            seq.Append(transform.DOPunchScale(Vector3.one * 0.5f, 0.02f));
            return seq;
        }
        
        public Tween ChangeType(ShapeBonusType shapeType)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOPunchScale(Vector3.one * 0.5f, 0.1f));
            var scaleTween = transform.DOScale(0, 0.1f);
            scaleTween.onComplete += () => Initialize(shapeType, transform.position);
            seq.Append(scaleTween);
            seq.Append(transform.DOScale(1, 0.02f));
            seq.Append(transform.DOPunchScale(Vector3.one * 0.5f, 0.02f));
            return seq;
        }
        
        public override string ToString() =>
            $"(s:{type}, b:{bonusType != ShapeBonusType.NONE})";
    }
}