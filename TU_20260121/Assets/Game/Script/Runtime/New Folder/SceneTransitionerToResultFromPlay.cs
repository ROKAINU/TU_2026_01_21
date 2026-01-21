using UnityEngine;

public static class SceneTransitionerToResultFromPlay
{
    public static void ToResultFromPlay()
    {
        GameManager.ins.globalValues.Score = GameState.ins.Score;
        GameManager.ins.globalValues.HitCount = GameState.ins.HitCount;
        SceneLoader.SceneLoad("ResultScene");
    }
}
