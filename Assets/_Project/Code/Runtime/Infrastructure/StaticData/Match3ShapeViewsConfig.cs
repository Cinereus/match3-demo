using System;
using System.Collections.Generic;
using Code.Match3;
using UnityEngine;

namespace Code.Runtime.Infrastructure.StaticData
{
    [CreateAssetMenu(menuName = "Match3/Create Match3ShapeViewsConfig", fileName = "Match3ShapeViewsConfig")]
    public class Match3ShapeViewsConfig : ScriptableObject
    {
        public List<Match3ShapeViewData> shapeViewData;
        public List<Match3BonusShapeViewData> bonusShapeViewData;
    }
    
    [Serializable]
    public struct Match3ShapeViewData
    {
        public ShapeType type;
        public Sprite sprite;
        public Color vfxColor;

        public Match3ShapeViewData(ShapeType type, Sprite sprite, Color vfxColor)
        {
            this.type = type;
            this.sprite = sprite;
            this.vfxColor = vfxColor;
        }
    }

    [Serializable]
    public struct Match3BonusShapeViewData
    {
        public ShapeBonusType type;
        public Sprite sprite;

        public Match3BonusShapeViewData(ShapeBonusType type, Sprite sprite)
        {
            this.type = type;
            this.sprite = sprite;
        }
    }
}