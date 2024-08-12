using System.Collections.Generic;
using System.Linq;
using Code.Match3;
using UnityEngine;

namespace Code.Runtime.Infrastructure.StaticData
{
    [CreateAssetMenu(menuName = "Match3/Create Match3VFXConfig", fileName = "Match3VFXConfig")]
    public class Match3VFXConfig : ScriptableObject
    {
        [SerializeField]
        private List<SqueezeVFXData> _squeezeVFXData;
        
        [SerializeField]
        private List<FragmentsVFXData> _fragmentsVFXData;
        
        public Color GetColor(ShapeType type) => 
            _squeezeVFXData.FirstOrDefault(c => c.type == type).color;
        
        public Color GetColor(ShapeBonusType type) => 
            _squeezeVFXData.FirstOrDefault(c => c.bonusType == type).color;
        
        public List<Sprite> GetFragments(ShapeType type) => 
            _fragmentsVFXData.FirstOrDefault(c => c.type == type).fragments;

        public List<Sprite> GetFragments(ShapeBonusType type) => 
            _fragmentsVFXData.FirstOrDefault(c => c.bonusType == type).fragments;

        public List<Sprite> GetPieces(ShapeType type) =>
            _fragmentsVFXData.FirstOrDefault(c => c.type == type).pieces;

        public List<Sprite> GetPieces(ShapeBonusType type) =>
            _fragmentsVFXData.FirstOrDefault(c => c.bonusType == type).pieces;
    }
}