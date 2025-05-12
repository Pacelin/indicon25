namespace TSS.Utils.Randoms.Weighted
{
    [System.Serializable]
    public struct WeightedEntry<T>
    {
        public T Value;
        public float Weight;
    }
}