using UnityEngine;

namespace Code.Runtime.Logic.Match3
{
    public class Match3ShapeView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _renderer;

        public Match3ShapeType type = Match3ShapeType.NONE;
        
        public Color color
        {
            get => _renderer.color;
            set => _renderer.color = value;
        }

        public void SetPoint(Vector3 target, bool isAnimated = false)
        {
            transform.position = target;
        }
    }
}