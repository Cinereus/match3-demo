using System;
using Code.Match3;
using UnityEngine;

namespace Code.Runtime.Infrastructure.StaticData
{
    [Serializable]
    public struct SqueezeVFXData
    {
        public ShapeType type;
        public ShapeBonusType bonusType;
        public Color color;
    }
}