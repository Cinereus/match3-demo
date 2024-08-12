using System.Collections.Generic;
using Code.Match3;
using Code.Runtime.Match3.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Runtime.Infrastructure.StaticData
{
    [CreateAssetMenu(menuName = "Match3/Create Match3LevelConfig", fileName = "Match3LevelConfig")]
    public class Match3LevelConfig : ScriptableObject
    {
        public Match3LevelType levelType;
        [Range(1, 1.5f)]
        public float tileSpacing = 1.05f;
        public bool columnTileZSorting;
        public float columnSortCoeff = 0.1f;
        public int gridWidth = 10;
        public int gridHeight = 10;
        public int shapesTypesCount = 7;
        public int crossTargetLength = 4;
        public int bombTargetLength = 5;
        public int colorTargetBonusUse = 4;
        public List<ShapeBonusType> bonuses;
        public LevelGoalData goal;
        public Vector3 gridSpawnPosition;

        [Header("Animations")]
        public float swapDuration = 0.2f;
        public float bonusCreateShapesMoveDuration = 0.1f;
        public float fallDuration = 0.2f;
        public float baseFallDelayCoeff = 0.05f;
        public float spawnShapesOffset = 2f;
        public float matchHintDuration = 0.5f;
        public float hintMoveOffset = 0.3f;
        public float gridResetDuration = 0.25f;
        public float shapeAppearDuration = 0.2f;
        public float shapeDisappearDuration = 0.1f;

        public List<Match3GoalData> GetNewGoals()
        {
            var goals = new List<Match3GoalData>(goal.goalCount);
            var createdTypes = new HashSet<ShapeType>();
            for (int i = 0; i < goal.goalCount; i++)
            {
                var newType = GetRandomType(createdTypes);
                createdTypes.Add(newType);
                goals.Add(new Match3GoalData(newType, Random.Range(goal.minGoalValue, goal.maxGoalValue)));
            }
            return goals;
        }

        private ShapeType GetRandomType(HashSet<ShapeType> createdTypes)
        {
            int shapeRandMax = shapesTypesCount + 1;
            if (createdTypes.Count == typeof(ShapeType).GetEnumValues().Length - 1)
                return (ShapeType) Random.Range(1, shapeRandMax);
            
            while (true)
            {
                var type = (ShapeType) Random.Range(1, shapeRandMax);
                if (createdTypes.Contains(type))
                    continue;
                
                return type;
            }
        }
    }
}