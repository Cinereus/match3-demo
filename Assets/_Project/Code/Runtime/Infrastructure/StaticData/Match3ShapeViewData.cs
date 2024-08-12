using System;
using Code.Match3;
using UnityEngine;

namespace Code.Runtime.Infrastructure.StaticData
{
    [Serializable]
    public struct Match3ShapeViewData
    {
        public ShapeType type;
        public ShapeBonusType bonusType;
        public Sprite sprite;
    }
}