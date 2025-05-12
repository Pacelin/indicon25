using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using TSS.Core;
using UnityEngine.AddressableAssets;

namespace TSS.Rest
{
    [UsedImplicitly]
    [RuntimeOrder(ERuntimeOrder.SystemRegistration)]
    public class RuntimeRestAPI : IRuntimeLoader
    {
        public async UniTask Initialize(CancellationToken cancellationToken)
        {
            var config = await Addressables.LoadAssetAsync<RestApiConfig>("Rest Config");
            RestAPI.Initialize(config);
        }

        public void Dispose()
        {
        }
    }
}