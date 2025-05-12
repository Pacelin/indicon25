using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.AddressableAssets;

namespace TSS.Core
{
    [UsedImplicitly]
    [RuntimeOrder(ERuntimeOrder.SubsystemRegistration)]
    internal class RuntimeAddressables : IRuntimeLoader
    {
        public UniTask Initialize(CancellationToken cancellationToken) =>
            Addressables.InitializeAsync(true)
                .ToUniTask(cancellationToken: cancellationToken);

        public void Dispose() { }
    }
}