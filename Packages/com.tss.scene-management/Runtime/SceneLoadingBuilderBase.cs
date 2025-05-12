using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace TSS.SceneManagement
{
    public class SceneLoadingBuilderBase
    {
        internal SceneLoadingContext Context => _ctx;
        
        private readonly SceneLoadingContext _ctx;
        
        internal SceneLoadingBuilderBase(SceneLoadingContext context) =>
            _ctx = context;

        [PublicAPI]
        public UniTask Load(CancellationToken cancellationToken) => SceneLoadingService.Load(_ctx, cancellationToken);
    }
}