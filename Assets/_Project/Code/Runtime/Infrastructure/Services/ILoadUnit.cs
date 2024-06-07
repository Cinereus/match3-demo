using Cysharp.Threading.Tasks;

namespace Code.Runtime.Infrastructure.Services
{
    public interface ILoadUnit
    {
        UniTask Load();
    }
    
    public interface ILoadUnit<TParam>
    {
        UniTask Load(TParam param);
    }
}