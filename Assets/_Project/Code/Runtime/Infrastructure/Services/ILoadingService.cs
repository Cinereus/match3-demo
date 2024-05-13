using Cysharp.Threading.Tasks;

namespace Code.Runtime.Infrastructure.Services
{
    public interface ILoadingService
    {
        UniTask Load(ILoadUnit loadUnit, bool skipExceptionThrow = false);
        
        UniTask Load<TParam>(ILoadUnit<TParam> loadUnit, TParam param, bool skipExceptionThrow = false);
    }

    public interface ILoadUnit
    {
        UniTask Load();
    }
    
    public interface ILoadUnit<TParam>
    {
        UniTask Load(TParam param);
    }
}