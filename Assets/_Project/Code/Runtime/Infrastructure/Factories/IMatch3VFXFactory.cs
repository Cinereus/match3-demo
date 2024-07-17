using Code.Runtime.Match3;
using UnityEngine;

namespace Code.Runtime.Infrastructure.Factories
{
    public interface IMatch3VFXFactory
    {
        Match3SqueezeParticle CreateSqueezeParticle(Vector3 position);
    }
}