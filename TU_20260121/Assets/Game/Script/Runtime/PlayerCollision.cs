using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // アイテム取得
        if (other.tag == "ScoreItem")
        {
            GameState.ins.Score += GameSettingValues.ins.ItemScore * GameState.ins.Rate;
            Destroy(other.gameObject);
            Debug.Log("アイテム取得！");
        }
        
        // 敵との衝突
        else if (other.tag == "Enemy")
        {
            if (!PlayerState.ins.isInvincible)
            {
                PlayerState.ins.InvincibleOn();
                GameState.ins.HitCount++;
                GameState.ins.Rate *= GameSettingValues.ins.EnemyScoreRate;
                Debug.Log("敵に当たった！被弾回数: " + GameState.ins.HitCount);
            }
        }
    }
}