using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Runtime.Infrastructure.Services
{
    public interface ILoadUnit
    {
        UniTask Load(CancellationToken token);
    }
    
    public interface ILoadUnit<in TParam>
    {
        UniTask Load(TParam param, CancellationToken token);
    }
}