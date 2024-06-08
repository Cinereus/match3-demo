using UnityEngine;

namespace Code.Runtime.Logic.Match3
{
    public readonly struct Match3Move
    {
        public readonly Match3MoveDirection direction;
        public readonly Vector2Int from;
        public readonly Vector2Int to;
        public bool isValid => direction != Match3MoveDirection.NONE;

        public Match3Move(Match3MoveDirection direction, Vector2Int coordinates)
        {
            this.direction = direction;
            from = coordinates;
            to = coordinates + direction switch
            {
                Match3MoveDirection.UP => new Vector2Int(0, 1),
                Match3MoveDirection.DOWN => new Vector2Int(0, -1),
                Match3MoveDirection.LEFT => new Vector2Int(-1, 0),
                Match3MoveDirection.RIGHT => new Vector2Int(1, 0),
                _ => Vector2Int.zero
            };
        }
    }
}