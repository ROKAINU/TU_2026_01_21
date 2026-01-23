namespace Game.Redux
{
    internal readonly record struct GameState(float GameCenter = 0f, float TimeLimit = 90f, bool IsRunning = false)
    {
        public static readonly GameState Default = new(0f, 90f, false);
    }
}