using UnityEngine;

namespace Code.Runtime.Logic.Match3
{
    public struct Match
    {
        public Vector2Int startPoint;
        public int length;
        public bool isHorizontal;

        public Match(Vector2Int startPoint, int length, bool isHorizontal = false)
        {
            this.startPoint = startPoint;
            this.length = length;
            this.isHorizontal = isHorizontal;
        }
    }
}