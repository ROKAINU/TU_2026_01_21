using Game.Redux;

namespace Game.Runtime.ReduxUtility
{
    internal static class GameReducer
    {
        public static GameState Reduce(GameState state, object action)
        {
            return action switch
            {
                ChangeCenterAction changeCenter => new GameState(
                    state.GameCenter + changeCenter.Value,
                    state.TimeLimit,
                    state.IsRunning),
                
                ChangeTimeLimitAction changeTime => new GameState(
                    state.GameCenter,
                    state.TimeLimit + changeTime.Value,
                    state.IsRunning),
                    
                ChangeIsRunningAction changeIsRunning => new GameState(
                    state.GameCenter,
                    state.TimeLimit,
                    changeIsRunning.IsRunning),
                
                _ => state
            };
        }
    }
    
    // アクション定義
    internal record ChangeCenterAction(float Value);
    internal record ChangeTimeLimitAction(float Value);
    internal record ChangeIsRunningAction(bool IsRunning);
}