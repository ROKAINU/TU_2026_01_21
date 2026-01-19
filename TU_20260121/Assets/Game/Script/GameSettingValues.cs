using UnityEngine;

public class GameSettingValues : MonoBehaviour
{
    #region Singleton
    public static GameSettingValues ins { get; private set; }//Instance
    void Awake()
    {
        if (ins == null)
        {
            ins = this;
            Speed = Speed * 0.01f;
        }
        else
        {
            Debug.LogWarning("GameSettingValuesが複数存在します");
            Destroy(this.gameObject); 
        }
    }
    #endregion

    [Header("入力してください")]
    [Header("ゲーム時間(/s)")]
    public float TimeLimit;

    [Header("ゲームスピード")]
    public float Speed;

    [Header("画面外判定用オフセット")]
    public float DeathZoneOffset;

    [Header("プレイヤー情報")]
    public float Gravity;
    public float JumpSpeed;
    public int MaxJumpCount;

    [Header("スクロール設定")]
    public float BackgroundScrollSpeedMultiplier = 1f;

    [Header("無敵時間")]
    
    public float InvincibleTime = 2f;

    [Header("スコア")]
    public float Rate;//倍率
    public float TimeScore;
    public float ItemScore;
    public float EnemyScoreRate;
}
