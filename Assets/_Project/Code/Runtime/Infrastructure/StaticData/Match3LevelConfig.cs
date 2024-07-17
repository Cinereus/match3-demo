using System.Collections.Generic;
using Code.Match3;
using UnityEngine;

namespace Code.Runtime.Infrastructure.StaticData
{
    [CreateAssetMenu(menuName = "Match3/Create Match3LevelConfig", fileName = "Match3LevelConfig")]
    public class Match3LevelConfig : ScriptableObject
    {
        public int gridWidth;
        public int gridHeight;
        public float tileSpacing;
        public int shapesTypesCount;
        public List<ShapeBonusType> bonuses;
        public Match3VisualType visualStyle;
    }

    public enum Match3VisualType
    {
        HUNGRY_BAT
    }
}