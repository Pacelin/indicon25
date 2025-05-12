using JetBrains.Annotations;
using System.Threading;

namespace TSS.Core.Extensions
{
    [PublicAPI]
    public static class CtsUtils
    {
        public static void CancelDisposeAndSetNull(ref CancellationTokenSource cts)
        {
            cts?.Cancel();
            cts?.Dispose();
            cts = null;
        }
        
        public static void CancelDisposeAndSetNew(ref CancellationTokenSource cts)
        {
            cts?.Cancel();
            cts?.Dispose();
            cts = new CancellationTokenSource();
        }
    }
}