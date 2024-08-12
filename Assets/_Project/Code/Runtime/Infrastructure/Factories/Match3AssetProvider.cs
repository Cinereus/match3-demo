using System;
using System.Collections.Generic;
using System.Threading;
using Code.Match3;
using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Infrastructure.StaticData;
using Code.Runtime.Match3;
using Code.Runtime.UI.Screens;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Runtime.Infrastructure.Factories
{
    public class Match3AssetProvider : ILoadUnit<Match3LevelConfig>
    {
        public GameObject gridSlotPrefab { get; private set; }
        public Match3ShapeView gridShapePrefab { get; private set; }
        public GameObject environmentPrefab { get; private set; }
        public Match3ShapeGoalView goalShapePrefab { get; private set; }
        public BlockFragmentsParticle fragmentsParticlePrefab { get; private set; }
        public Match3SqueezeParticle squeezeParticlePrefab { get; private set; }
        
        private Match3ShapeViewsConfig _shapeViewsConfig;
        private Match3VFXConfig _vfxConfig;

        public async UniTask Load(Match3LevelConfig config, CancellationToken token)
        {
            gridSlotPrefab = 
                await Resources.LoadAsync<GameObject>(RuntimeConstants.Assets.MATCH3_GRID_SLOT)
                    .WithCancellation(token) as GameObject;
            
            gridShapePrefab =
                await Resources.LoadAsync<Match3ShapeView>(RuntimeConstants.Assets.MATCH3_GRID_SHAPE)
                    .WithCancellation(token) as Match3ShapeView;
            
            squeezeParticlePrefab = 
                await Resources.LoadAsync<Match3SqueezeParticle>(RuntimeConstants.Assets.SQUEEZE_VFX)
                    .WithCancellation(token) as Match3SqueezeParticle;
            
            fragmentsParticlePrefab =
                await Resources.LoadAsync<BlockFragmentsParticle>(RuntimeConstants.Assets.FRAGMENT_VFX)
                    .WithCancellation(token) as BlockFragmentsParticle;

            var vfxDataPath = string.Empty;
            var shapeViewsPath = string.Empty;
            var goalPath = string.Empty; 
            var environmentPath = string.Empty;
            switch (config.levelType)
            {
                case Match3LevelType.HUNGRY_BAT:
                    vfxDataPath = RuntimeConstants.Assets.HUNGRY_BAT_VFX_CONFIG;
                    shapeViewsPath = RuntimeConstants.Assets.HUNGRY_BAT_SHAPE_VIEWS_CONFIG;
                    environmentPath = RuntimeConstants.Assets.HUNGRY_BAT_ENVIRONMENT;
                    goalPath = RuntimeConstants.Assets.UI_MATCH3_FRUIT_GOAL_SHAPE;
                    break;
                case Match3LevelType.BLOCK_CRUSH:
                    vfxDataPath = RuntimeConstants.Assets.BLOCK_CRUSH_VFX_CONFIG;
                    shapeViewsPath = RuntimeConstants.Assets.BLOCK_CRUSH_SHAPE_VIEWS_CONFIG;
                    environmentPath = RuntimeConstants.Assets.BLOCK_CRUSH_ENVIRONMENT;
                    goalPath = RuntimeConstants.Assets.UI_MATCH3_BLOCK_GOAL_SHAPE;
                    break;
            }
            
            _vfxConfig =
                await Resources.LoadAsync<Match3VFXConfig>(vfxDataPath).WithCancellation(token) as Match3VFXConfig;
            
            if (_vfxConfig == null)
                throw new Exception($"[{nameof(Match3AssetProvider)}] Failed to load VFX data. Config is null!");
            
            _shapeViewsConfig =
                await Resources.LoadAsync<Match3ShapeViewsConfig>(shapeViewsPath).WithCancellation(token) as
                    Match3ShapeViewsConfig;
            
            if (_shapeViewsConfig == null)
                throw new Exception($"[{nameof(Match3AssetProvider)}] Failed to load shape view data. Config is null!");
            
            if (_shapeViewsConfig.shapeViewData.Count < config.shapesTypesCount) 
                throw new Exception($"[{nameof(Match3AssetProvider)}] Failed to load shape view data. Data count less than configuration!");

            goalShapePrefab =
                await Resources.LoadAsync<Match3ShapeGoalView>(goalPath).WithCancellation(token) as Match3ShapeGoalView;
            
            environmentPrefab =
                await Resources.LoadAsync<GameObject>(environmentPath).WithCancellation(token) as GameObject;
        }

        public Match3ShapeViewData GetShapeViewData(ShapeType shapeType) => 
            _shapeViewsConfig.GetViewData(shapeType);

        public Match3ShapeViewData GetBonusShapeViewData(ShapeBonusType shapeType) =>
            _shapeViewsConfig.GetBonusViewData(shapeType);

        public List<Sprite> GetFragmentVFXSprites(ShapeType type) => 
            _vfxConfig.GetFragments(type);

        public List<Sprite> GetPieceVFXSprites(ShapeType type) => 
            _vfxConfig.GetPieces(type);

        public Color GetSqueezeVFXColor(ShapeType type) => 
            _vfxConfig.GetColor(type);
    }
}