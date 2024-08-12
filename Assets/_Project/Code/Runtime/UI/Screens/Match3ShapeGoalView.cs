using System;
using Code.Match3;
using Code.Runtime.Infrastructure.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Runtime.UI.Screens
{
    public class Match3ShapeGoalView : MonoBehaviour, IPoolable<Match3ShapeGoalView>
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private TextMeshProUGUI _text;

        public event Action<Match3ShapeGoalView> onReturnToPoolRequested;
        public ShapeType type { get; private set; } = ShapeType.NONE;

        public void Initialize(Sprite sprite, ShapeType shape, int count)
        {
            type = shape;
            _image.sprite = sprite;
            _text.text = count.ToString();
        }

        public void SetCount(int count) =>
            _text.text = count.ToString();

        public void ReturnToPool()
        {
            type = ShapeType.NONE;
            _image.sprite = null;
            _text.text = string.Empty;
            onReturnToPoolRequested?.Invoke(this);
        }
    }
}