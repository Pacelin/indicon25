using System.Threading;
using Cysharp.Threading.Tasks;

namespace TSS.Core
{
    public interface IRuntimeLoader
    {
        UniTask Initialize(CancellationToken cancellationToken);
        void Dispose();
    }
}