using System.Threading;
using Code.Runtime.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Code.Runtime.Infrastructure.Services
{
    public class LoadSceneService : ILoadUnit<string>
    {
        private readonly LoadingCurtain _loadingCurtain;

        public LoadSceneService(LoadingCurtain loadingCurtain)
        {
            _loadingCurtain = loadingCurtain;
        }

        public async UniTask Load(string name, CancellationToken token = default)
        {
            await _loadingCurtain.Show(token);
            await SceneManager.LoadSceneAsync(name).WithCancellation(token);
        }
    }
}