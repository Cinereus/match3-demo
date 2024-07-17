using System;
using Code.Runtime.Infrastructure.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Runtime.Match3
{
    public class Match3SqueezeParticle : MonoBehaviour, IPoolable<Match3SqueezeParticle>
    {
        [SerializeField]
        private ParticleSystem _blot;
        
        [SerializeField]
        private ParticleSystem _drops;

        public event Action<Match3SqueezeParticle> onReturnToPool;

        private ParticleSystem.MinMaxGradient _dropsDefaultColor;
        private ParticleSystem.MinMaxGradient _blotDefaultColor;

        private void Awake()
        {
            _dropsDefaultColor = _drops.main.startColor;
            _blotDefaultColor = _blot.main.startColor;
        }

        public async UniTaskVoid PlayOnce()
        {
            var blotMain = _blot.main;
            var dropsMain = _drops.main;
            blotMain.loop = false;
            dropsMain.loop = false;
            _blot.Play();
            await UniTask.Delay((int) (blotMain.duration * 1000));
            blotMain.startColor = _blotDefaultColor;
            dropsMain.startColor = _dropsDefaultColor;
            onReturnToPool?.Invoke(this);
        }

        public void SetColor(Color color)
        {
            var blotMain = _blot.main;
            var dropsMain = _drops.main;
            blotMain.startColor = new ParticleSystem.MinMaxGradient(color);
            dropsMain.startColor = new ParticleSystem.MinMaxGradient(color);
        }
    }
}