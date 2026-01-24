namespace Game.Redux
{
    internal readonly struct GamePlayerState
    {
        public float Speed { get; }
        public bool Invincible { get; }
        public float InvincibleTime { get; }
        public bool UseInvincibleItem { get; }
        public float NotJumpableTime { get; }//0以下でジャンプ可能
        public float SpeedUpTime { get; }
        public bool IsGameOver { get; }
        public int JumpCount { get; }

        public GamePlayerState(float speed, bool invincible, float invincibleTime, bool useInvincibleItem, float notJumpableTime,float speedUpTime, bool isGameOver, int jumpCount)
        {
            Speed = speed;
            Invincible = invincible;
            InvincibleTime = invincibleTime;
            UseInvincibleItem = useInvincibleItem;
            NotJumpableTime = notJumpableTime;
            SpeedUpTime = speedUpTime;
            IsGameOver = isGameOver;
            JumpCount = jumpCount;
        }

        public static readonly GamePlayerState Default = new GamePlayerState(0f, false, 0f, false, 0f, 0f, false, 0);
    }
}