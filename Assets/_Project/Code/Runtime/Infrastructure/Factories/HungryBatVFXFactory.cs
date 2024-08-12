using System.Threading;
using Code.Match3;
using Code.Runtime.Infrastructure.Pool;
using Code.Runtime.Match3;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Runtime.Infrastructure.Factories
{
    public class HungryBatVFXFactory : IMatch3VFXFactory
    {
        private readonly Match3AssetProvider _assetProvider;
        private GameObjectPool<Match3SqueezeParticle> _squeezeParticlesPool;

        public HungryBatVFXFactory(Match3AssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public UniTask Load(CancellationToken token)
        {
            _squeezeParticlesPool =
                new GameObjectPool<Match3SqueezeParticle>(_assetProvider.squeezeParticlePrefab);
            
            _squeezeParticlesPool.Initialize();
            return UniTask.CompletedTask;
        }

        public IMatch3Particle CreateShapeDestroyParticle(ShapeType type, Vector3 position)
        {
            Match3SqueezeParticle particle = _squeezeParticlesPool.Get(); 
            particle.Initialize(position, _assetProvider.GetSqueezeVFXColor(type));
            return particle;
        }
    }
}