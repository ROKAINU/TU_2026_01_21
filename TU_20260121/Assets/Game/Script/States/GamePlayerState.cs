namespace Game.Redux
{
    internal readonly struct GamePlayerState
    {
        public bool Invincible { get; }
        public float InvincibleTime { get; }
        public bool UseInvincibleItem { get; }
        public float NotJumpableTime { get; }//0以下でジャンプ可能
        public float SpeedUpTime { get; }
        public bool IsGameOver { get; }

        public GamePlayerState(bool invincible, float invincibleTime, bool useInvincibleItem, float notJumpableTime,float speedUpTime, bool isGameOver)
        {
            Invincible = invincible;
            InvincibleTime = invincibleTime;
            UseInvincibleItem = useInvincibleItem;
            NotJumpableTime = notJumpableTime;
            SpeedUpTime = speedUpTime;
            IsGameOver = isGameOver;
        }

        public static readonly GamePlayerState Default = new GamePlayerState(false, 0f, false, 0f, 0f, false);
    }
}