using Game.Redux;

namespace Game.Runtime.ReduxUtility
{
    internal static class GamePlayerReducer
    {
        public static GamePlayerState Reduce(GamePlayerState state, object action)
        {
            return action switch
            {
                SetSpeed setSpeedAction => new GamePlayerState(
                    setSpeedAction.Speed,
                    state.Invincible,
                    state.InvincibleTime,
                    state.UseInvincibleItem,
                    state.NotJumpableTime,
                    state.SpeedUpTime,
                    state.IsGameOver,
                    state.JumpCount),

                PlayerDamageAction damageAction => new GamePlayerState(
                    state.Speed,
                    true,
                    damageAction.InvincibleTime,
                    state.UseInvincibleItem,
                    state.NotJumpableTime,
                    state.SpeedUpTime,
                    state.IsGameOver,
                    state.JumpCount),

                PlayerGetInvincibleItemAction itemAction => new GamePlayerState(
                    state.Speed,
                    true,
                    itemAction.InvincibleTime,
                    true,
                    state.NotJumpableTime,
                    state.SpeedUpTime,
                    state.IsGameOver,
                    state.JumpCount),
                
                UpdateInvincibleAction updateAction => new GamePlayerState(
                    state.Speed,
                    state.Invincible,
                    state.InvincibleTime - updateAction.DeltaTime,
                    state.UseInvincibleItem,
                    state.NotJumpableTime,
                    state.SpeedUpTime,
                    state.IsGameOver,
                    state.JumpCount),

                PlayerInvincibleEndAction => new GamePlayerState(
                    state.Speed,
                    false,
                    0f,
                    false,
                    state.NotJumpableTime,
                    state.SpeedUpTime,
                    state.IsGameOver,
                    state.JumpCount),//敵のダメージを受けて無敵になりつつ、かつ無敵アイテム使用中にさらに無敵アイテムを使った場合が存在しないから、無敵でひとくくりにしていい

                PlayerJumpableAction jumpableAction => new GamePlayerState(
                    state.Speed,
                    state.Invincible,
                    state.InvincibleTime,
                    state.UseInvincibleItem,
                    jumpableAction.NotJumpableTime,
                    state.SpeedUpTime,
                    state.IsGameOver,
                    state.JumpCount),
                
                PlayerSpeedUpAction speedUpAction => new GamePlayerState(
                    state.Speed,
                    state.Invincible,
                    state.InvincibleTime,
                    state.UseInvincibleItem,
                    state.NotJumpableTime,
                    speedUpAction.SpeedUpTime,
                    state.IsGameOver,
                    state.JumpCount),
                
                PlayerGameOverAction => new GamePlayerState(
                    state.Speed,
                    state.Invincible,
                    state.InvincibleTime,
                    state.UseInvincibleItem,
                    state.NotJumpableTime,
                    state.SpeedUpTime,
                    true,
                    state.JumpCount),
                
                PlayerSetJumpCountAction setJumpCountAction => new GamePlayerState(
                    state.Speed,
                    state.Invincible,
                    state.InvincibleTime,
                    state.UseInvincibleItem,
                    state.NotJumpableTime,
                    state.SpeedUpTime,
                    state.IsGameOver,
                    setJumpCountAction.JumpCount),

                _ => state
            };
        }
    }
    
    // アクション定義

    internal record SetSpeed(float Speed);
    internal record PlayerDamageAction(float InvincibleTime);
    internal record PlayerGetInvincibleItemAction(float InvincibleTime);
    internal record UpdateInvincibleAction(float DeltaTime);
    internal record PlayerInvincibleEndAction();
    internal record PlayerJumpableAction(float NotJumpableTime);
    internal record PlayerSpeedUpAction(float SpeedUpTime);
    internal record PlayerGameOverAction();
    internal record PlayerSetJumpCountAction(int JumpCount);
}