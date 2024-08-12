using System;
using System.Collections.Generic;
using Code.Match3;
using UnityEngine;

namespace Code.Runtime.Infrastructure.StaticData
{
    [Serializable]
    public struct FragmentsVFXData
    {
        public ShapeType type;
        public ShapeBonusType bonusType;
        public  List<Sprite> fragments;
        public  List<Sprite> pieces;
    }
}