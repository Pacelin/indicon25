using JetBrains.Annotations;

namespace TSS.Utils
{
    [PublicAPI]
    public static partial class TSSUtils
    {
        public static TypeBuilder Types() => new TypeBuilder();
    }
}
