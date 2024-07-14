using Cysharp.Threading.Tasks;

namespace Code.Runtime.Infrastructure.Services
{
    public interface ILoadUnit
    {
        UniTask Load();
    }
    
    public interface ILoadUnit<in TParam>
    {
        UniTask Load(TParam param);
    }
}