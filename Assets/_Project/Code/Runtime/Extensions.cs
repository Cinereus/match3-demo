using Code.Match3;
using UnityEngine;

namespace Code.Runtime
{
    public static class Extensions
    {
        public static ShapePos ScreenToShapePos(this Camera camera, Vector3 screenPosition, Vector3 distancePoint)
        {
            var elementHalfSize = 0.5f;
            distancePoint.x -= elementHalfSize;
            distancePoint.y -= elementHalfSize;
            Vector3 worldPos =
                camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y,
                    -camera.transform.position.z));
            
            Vector3 distance = worldPos - distancePoint;
            return new ShapePos(Mathf.FloorToInt(distance.x / 1.2f), Mathf.FloorToInt(distance.y / 1.2f));
        }
    }
}