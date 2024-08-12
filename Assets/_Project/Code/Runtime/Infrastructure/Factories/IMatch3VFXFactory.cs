using Code.Match3;
using Code.Runtime.Infrastructure.Services;
using UnityEngine;

namespace Code.Runtime.Infrastructure.Factories
{
    public interface IMatch3VFXFactory : ILoadUnit
    {
        public IMatch3Particle CreateShapeDestroyParticle(ShapeType type, Vector3 position);
    }
}