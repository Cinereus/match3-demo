using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Runtime.Infrastructure.Factories
{
    public interface IMatch3Particle
    {
        public UniTaskVoid PlayOnce(CancellationToken token);
    }
}