using Game.Redux;

namespace Game.Runtime.ReduxUtility
{
    internal static class GamePlayerReducer
    {
        public static GamePlayerState Reduce(GamePlayerState state, object action)
        {
            return action switch
            {
                PlayerDamageAction damageAction => new GamePlayerState(
                    true,
                    damageAction.InvincibleTime,
                    state.UseInvincibleItem,
                    state.NotJumpableTime,
                    state.SpeedUpTime,
                    state.IsGameOver),

                PlayerGetInvincibleItemAction itemAction => new GamePlayerState(
                    true,
                    itemAction.InvincibleTime,
                    true,
                    state.NotJumpableTime,
                    state.SpeedUpTime,
                    state.IsGameOver),
                
                UpdateInvincibleAction updateAction => new GamePlayerState(
                    state.Invincible,
                    state.InvincibleTime - updateAction.DeltaTime,
                    state.UseInvincibleItem,
                    state.NotJumpableTime,
                    state.SpeedUpTime,
                    state.IsGameOver),

                PlayerInvincibleEndAction => new GamePlayerState(
                    false,
                    0f,
                    false,
                    state.NotJumpableTime,
                    state.SpeedUpTime,
                    state.IsGameOver),//敵のダメージを受けて無敵になりつつ、かつ無敵アイテム使用中にさらに無敵アイテムを使った場合が存在しないから、無敵でひとくくりにしていい

                PlayerJumpableAction jumpableAction => new GamePlayerState(
                    state.Invincible,
                    state.InvincibleTime,
                    state.UseInvincibleItem,
                    jumpableAction.NotJumpableTime,
                    state.SpeedUpTime,
                    state.IsGameOver),
                
                PlayerSpeedUpAction speedUpAction => new GamePlayerState(
                    state.Invincible,
                    state.InvincibleTime,
                    state.UseInvincibleItem,
                    state.NotJumpableTime,
                    speedUpAction.SpeedUpTime,
                    state.IsGameOver),
                
                PlayerGameOverAction => new GamePlayerState(
                    state.Invincible,
                    state.InvincibleTime,
                    state.UseInvincibleItem,
                    state.NotJumpableTime,
                    state.SpeedUpTime,
                    true),

                _ => state
            };
        }
    }
    
    // アクション定義
    internal record PlayerDamageAction(float InvincibleTime);
    internal record PlayerGetInvincibleItemAction(float InvincibleTime);
    internal record UpdateInvincibleAction(float DeltaTime);
    internal record PlayerInvincibleEndAction();
    internal record PlayerJumpableAction(float NotJumpableTime);
    internal record PlayerSpeedUpAction(float SpeedUpTime);
    internal record PlayerGameOverAction();
}