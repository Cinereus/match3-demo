using Code.Match3;
using Code.Runtime.Match3;
using Code.Runtime.UI.Screens;
using UnityEngine;

namespace Code.Runtime.Infrastructure.Factories
{
    public interface IMatch3Factory
    {
        Vector2 shapeSize { get; }
        Match3ShapeView CreateShape(Vector3 position, ShapeType type, Transform parent);
        Match3ShapeView CreateShape(Vector3 position, ShapeBonusType bonusType, Transform parent);
        GameObject CreateGridSlot(Vector3 position, Transform parent);
        Match3ShapeGoalView CreateGoalView(ShapeType shapeType, int count);
    }
}