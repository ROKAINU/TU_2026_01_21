namespace Game.Redux
{
    internal readonly struct GameState
    {
        
        public float GameCenter { get; }
        public float TimeLimit { get; }
        public bool IsRunning { get; }

        public GameState(float gameCenter, float timeLimit, bool isRunning)
        {
            GameCenter = gameCenter;
            TimeLimit = timeLimit;
            IsRunning = isRunning;
        }

        public static readonly GameState Default = new GameState(0f, 90f, false);
    }
}