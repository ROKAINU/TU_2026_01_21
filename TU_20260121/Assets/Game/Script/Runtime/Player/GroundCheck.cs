using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private const string _groundTag = "Ground";

    [Header("Ground Check Settings")]
    [SerializeField] private float checkDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    public bool IsGrounded { get; private set; }

    //collision.tag == _groundTag

    public bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,     // 発射位置
            Vector2.down,           // 下方向
            checkDistance,          // 距離
            groundLayer             // レイヤー
        );

        return hit.collider != null;
    }

    private void OnDrawGizmos()//当たり判定のデバッグ表示用
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * checkDistance);
    }
}
