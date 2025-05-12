namespace TSS.Utils.Randoms.ADHOC
{
    [System.Serializable]
    public struct ADHOCEntry<T>
    {
        public T Value;
        public float Weight;
        public float WeightIncreaseOnFail;
    }
}