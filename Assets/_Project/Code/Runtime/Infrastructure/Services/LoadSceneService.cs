using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Code.Runtime.Infrastructure.Services
{
    public class LoadSceneService : ILoadUnit<string>
    {
        public UniTask Load(string name) => 
            SceneManager.LoadSceneAsync(name).ToUniTask();
    }
}