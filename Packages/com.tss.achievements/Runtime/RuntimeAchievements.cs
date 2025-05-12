using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using TSS.Core;
using UnityEngine.AddressableAssets;

namespace TSS.Achievements
{
    [RuntimeOrder(ERuntimeOrder.SystemRegistration, 1)]
    [UsedImplicitly]
    public class RuntimeAchievements : IRuntimeLoader
    {
        public async UniTask Initialize(CancellationToken cancellationToken)
        {
            var collection = await Addressables.LoadAssetAsync<AchievementsConfig>("Achievements Config");
            Achievements.Initialize(collection);
            var collectionView = UnityEngine.Object.Instantiate(collection.ViewPrefab);
            UnityEngine.Object.DontDestroyOnLoad(collectionView.gameObject);
            
            await UniTask.NextFrame(cancellationToken);
            
            var panelView = UnityEngine.Object.Instantiate(collection.PanelPrefab);
            UnityEngine.Object.DontDestroyOnLoad(panelView.gameObject);
            panelView.gameObject.SetActive(false);
            Achievements.SetPanel(panelView);
        }

        public void Dispose()
        {
            Achievements.Dispose();
        }
    }
}