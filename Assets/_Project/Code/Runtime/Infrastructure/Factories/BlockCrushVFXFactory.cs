using System.Threading;
using Code.Match3;
using Code.Runtime.Infrastructure.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Runtime.Infrastructure.Factories
{
    public class BlockCrushVFXFactory : IMatch3VFXFactory
    {
        private readonly Match3AssetProvider _assetProvider;
        private GameObjectPool<BlockFragmentsParticle> _fragmentsParticlesPool;

        public BlockCrushVFXFactory(Match3AssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public UniTask Load(CancellationToken token)
        {
            _fragmentsParticlesPool =
                new GameObjectPool<BlockFragmentsParticle>(_assetProvider.fragmentsParticlePrefab);
            _fragmentsParticlesPool.Initialize();
            
            return UniTask.CompletedTask;
        }

        public IMatch3Particle CreateShapeDestroyParticle(ShapeType type, Vector3 position)
        {
            BlockFragmentsParticle particle = _fragmentsParticlesPool.Get();
            particle.Initialize(position, _assetProvider.GetFragmentVFXSprites(type),
                _assetProvider.GetPieceVFXSprites(type));
            return particle;
        }
    }
}