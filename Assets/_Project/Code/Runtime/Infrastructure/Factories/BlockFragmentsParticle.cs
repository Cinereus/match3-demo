using System;
using System.Collections.Generic;
using System.Threading;
using Code.Runtime.Infrastructure.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Runtime.Infrastructure.Factories
{
    public class BlockFragmentsParticle : MonoBehaviour, IPoolable<BlockFragmentsParticle>, IMatch3Particle
    {
        [SerializeField]
        private ParticleSystem _fragmentParticles;
        
        [SerializeField]
        private ParticleSystem _pieceParticles;
        
        public event Action<BlockFragmentsParticle> onReturnToPoolRequested;

        public void Initialize(Vector3 position, List<Sprite> fragments, List<Sprite> pieces)
        {
            transform.position = position;
            ParticleSystem.TextureSheetAnimationModule fragSheetAnim = _fragmentParticles.textureSheetAnimation;
            ParticleSystem.TextureSheetAnimationModule pieceSheetAnim = _pieceParticles.textureSheetAnimation;
            
            foreach (var fragment in fragments) 
                fragSheetAnim.AddSprite(fragment);
            
            foreach (var piece in pieces)
                pieceSheetAnim.AddSprite(piece);   
        }

        public async UniTaskVoid PlayOnce(CancellationToken token)
        {
            _fragmentParticles.Play();
            
            if (token.IsCancellationRequested)
                ReturnToPool();
            
            const int SECS_MULTIPLIER = 1000;
            await UniTask.Delay((int) (_fragmentParticles.main.duration * SECS_MULTIPLIER), cancellationToken: token);
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            ParticleSystem.TextureSheetAnimationModule fragSheetAnim = _fragmentParticles.textureSheetAnimation;
            ParticleSystem.TextureSheetAnimationModule pieceSheetAnim = _pieceParticles.textureSheetAnimation;

            int fragCount = fragSheetAnim.spriteCount; 
            for (int i = 0; i < fragCount; i++)
            {
                fragSheetAnim.RemoveSprite(0);
            }
                
            int pieceCount = pieceSheetAnim.spriteCount;
            for (int i = 0; i < pieceCount; i++)
                pieceSheetAnim.RemoveSprite(0);
            
            onReturnToPoolRequested?.Invoke(this);
        }
    }
}