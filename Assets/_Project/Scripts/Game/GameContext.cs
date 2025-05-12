using System.Threading;

namespace Siberian25.Game
{
    [System.Serializable]
    public static class GameContext
    {
        public static CancellationToken CancellationToken { get; set; }
    }
}