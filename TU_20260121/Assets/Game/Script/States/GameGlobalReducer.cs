using Game.Redux;

namespace Game.Runtime.ReduxUtility
{
    internal static class GameGlobalReducer
    {
        public static GameGlobalState Reduce(GameGlobalState state, object action)
        {
            return action switch
            {
                // アイテム取得
                CollectItemAction => new GameGlobalState(
                    state.ItemCount + 1,
                    state.ClearItemCount,
                    state.HitCount),
                
                // クリアアイテム取得
                CollectClearItemAction => new GameGlobalState(
                    state.ItemCount,
                    state.ClearItemCount + 1,
                    state.HitCount),
                
                // 被弾回数増加
                IncrementHitCountAction => new GameGlobalState(
                    state.ItemCount,
                    state.ClearItemCount,
                    state.HitCount + 1),
                
                _ => state
            };
        }
    }
    
    // アクション定義
    internal record CollectItemAction();
    internal record CollectClearItemAction();
    internal record IncrementHitCountAction();
}