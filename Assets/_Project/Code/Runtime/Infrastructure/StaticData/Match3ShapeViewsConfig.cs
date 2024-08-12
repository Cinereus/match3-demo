using System.Collections.Generic;
using System.Linq;
using Code.Match3;
using UnityEngine;

namespace Code.Runtime.Infrastructure.StaticData
{
    [CreateAssetMenu(menuName = "Match3/Create Match3ShapeViewsConfig", fileName = "Match3ShapeViewsConfig")]
    public class Match3ShapeViewsConfig : ScriptableObject
    {
        public List<Match3ShapeViewData> shapeViewData;
        public List<Match3ShapeViewData> shapeBonusViewData;

        public Match3ShapeViewData GetViewData(ShapeType type) => 
            shapeViewData.FirstOrDefault(b => b.type == type);
        
        public Match3ShapeViewData GetBonusViewData(ShapeBonusType type) => 
            shapeBonusViewData.FirstOrDefault(b => b.bonusType == type);
    }
}