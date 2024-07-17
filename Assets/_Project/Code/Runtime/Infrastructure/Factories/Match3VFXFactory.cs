using Code.Runtime.Infrastructure.Pool;
using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Match3;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Runtime.Infrastructure.Factories
{
    public class Match3VFXFactory : ILoadUnit, IMatch3VFXFactory
    {
        private GameObject _squeezeParticlePrefab;
        private ObjectPool<Match3SqueezeParticle> _squeezeParticles;

        public UniTask Load()
        {
            _squeezeParticlePrefab = Resources.Load<GameObject>(RuntimeConstants.Assets.MATCH3_VFX_SQUEEZE);
            _squeezeParticles = new ObjectPool<Match3SqueezeParticle>(creator: CreateSqueezeParticleInstance);
            return UniTask.CompletedTask;
        }

        public Match3SqueezeParticle CreateSqueezeParticle(Vector3 position)
        {
            Match3SqueezeParticle squeezeParticle = _squeezeParticles.Get();
            squeezeParticle.gameObject.SetActive(true);
            squeezeParticle.transform.position = position;
            squeezeParticle.onReturnToPool += ReturnToPool;
            return squeezeParticle;
        }

        private void ReturnToPool(Match3SqueezeParticle squeezeParticle)
        {
            squeezeParticle.gameObject.SetActive(false);
            squeezeParticle.onReturnToPool -= ReturnToPool;
            _squeezeParticles.Return(squeezeParticle);
        }

        private Match3SqueezeParticle CreateSqueezeParticleInstance() =>
            Object.Instantiate(_squeezeParticlePrefab, Vector3.zero, Quaternion.identity)
                .GetComponent<Match3SqueezeParticle>();
    }
}