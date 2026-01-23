namespace Game.Redux
{
    internal readonly struct GameGlobalState
    {
        public int ItemCount { get; }
        public int ClearItemCount { get; }
        public int HitCount { get; }

        public GameGlobalState(int itemCount, int clearItemCount, int hitCount)
        {
            ItemCount = itemCount;
            ClearItemCount = clearItemCount;
            HitCount = hitCount;
        }

        public static readonly GameGlobalState Default = new GameGlobalState(0, 0, 0);
    }
}