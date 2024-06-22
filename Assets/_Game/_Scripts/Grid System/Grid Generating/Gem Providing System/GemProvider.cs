namespace Match3
{
    public abstract class GemProvider<T>
    {
        public abstract T[] GemPrefabs { get; protected set; }
        public GemProvider(T[] prefabs)
        {
            this.GemPrefabs = prefabs;
        }
        public abstract T GetGem(GemType gemType);
        public abstract T GetRandomGem();
        public abstract void ReturnGem(T item);
    }
}