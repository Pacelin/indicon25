namespace TSS.Utils
{
    public static partial class TSSUtils
    {
        public static void IfNotNull(this UnityEngine.Object @object, System.Action action)
        {
            if (@object)
                action.Invoke();
        }
    }
}
