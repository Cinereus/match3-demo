using System.Collections.Generic;
using System.Linq;
using Code.Runtime.Infrastructure.StaticData;

namespace Code.Runtime.Match3
{
    public class Match3LevelConfigContainer
    {
        public Match3LevelType targetType;
        
        private readonly List<Match3LevelConfig> _levelConfigs;

        public Match3LevelConfigContainer(List<Match3LevelConfig> levelConfigs)
        {
            targetType = Match3LevelType.HUNGRY_BAT;
            _levelConfigs = levelConfigs;
        }

        public Match3LevelConfig GetTargetConfig() => 
            _levelConfigs.FirstOrDefault(c => c.levelType == targetType);
    }
}