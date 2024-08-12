using System;
using System.Threading;
using Code.Runtime.Infrastructure.Factories;
using Code.Runtime.Infrastructure.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Runtime.Match3
{
    public class Match3SqueezeParticle : MonoBehaviour, IPoolable<Match3SqueezeParticle>, IMatch3Particle
    {
        [SerializeField]
        private ParticleSystem _blot;
        
        [SerializeField]
        private ParticleSystem _drops;

        public event Action<Match3SqueezeParticle> onReturnToPoolRequested;

        private ParticleSystem.MinMaxGradient _dropsDefaultColor;
        private ParticleSystem.MinMaxGradient _blotDefaultColor;

        private void Awake()
        {
            _dropsDefaultColor = _drops.main.startColor;
            _blotDefaultColor = _blot.main.startColor;
        }

        public void Initialize(Vector3 position, Color color)
        {
            transform.position = position;
            SetColor(color);
        }

        public async UniTaskVoid PlayOnce(CancellationToken token)
        {
            _blot.Play();
            
            if (token.IsCancellationRequested)
                ReturnToPool();

            const int SECS_MULTIPLIER = 1000;
            await UniTask.Delay((int) (_blot.main.duration * SECS_MULTIPLIER), cancellationToken: token);
            ReturnToPool();
        }

        private void SetColor(Color color)
        {
            ParticleSystem.MainModule blotMain = _blot.main;
            ParticleSystem.MainModule dropsMain = _drops.main;
            blotMain.startColor = new ParticleSystem.MinMaxGradient(color);
            dropsMain.startColor = new ParticleSystem.MinMaxGradient(color);
        }

        private void ReturnToPool()
        {
            ParticleSystem.MainModule blotMain = _blot.main;
            ParticleSystem.MainModule dropsMain = _drops.main;
            blotMain.startColor = _blotDefaultColor;
            dropsMain.startColor = _dropsDefaultColor;
            onReturnToPoolRequested?.Invoke(this);
        }
    }
}