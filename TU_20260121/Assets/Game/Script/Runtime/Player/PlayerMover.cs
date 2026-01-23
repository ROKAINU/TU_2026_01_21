using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using Game;
using Game.Redux;
using Game.Runtime.ReduxUtility;
using R3;

namespace Game.Runtime
{
    public class PlayerMover : MonoBehaviour
    {
        public float wallCheckDistance = 0.1f;
        public Rigidbody2D _rigidBody2D;
        public GroundCheck _groundCheck;
        [SerializeField] private LayerMask _wallLayer;
        [SerializeField] private PhysicsMaterial2D _noFrictionMaterial;

        private PlayerConfig _playerConfig;
        private GameConfig _gameConfig;
        private Store<GameState> _gameStateStore;
        private Store<GamePlayerState> _gamePlayerStateStore;
        
        public int _jumpCount = 0;

        [Inject]
        internal void Construct(
            PlayerConfig playerConfig,
            GameConfig gameConfig,
            Store<GameState> gameStateStore,
            Store<GamePlayerState> gamePlayerStateStore)
        {
            _playerConfig = playerConfig;
            _gameConfig = gameConfig;
            _gameStateStore = gameStateStore;
            _gamePlayerStateStore = gamePlayerStateStore;

            _jumpCount = _playerConfig.MaxJumpCount;

            //UnityEngine.Debug.Log($"GameConfig:{_gameConfig}, PlayerCongfig:{_playerConfig.JumpSpeed}, GameStateStore:{_gameStateStore}, PlayerStateStore:{_gamePlayerStateStore}");
        }

        private void Start()
        {
            if (_noFrictionMaterial != null)
                _rigidBody2D.sharedMaterial = _noFrictionMaterial;

            // 重力設定
            _rigidBody2D.gravityScale = _playerConfig.Gravity;

            // ジャンプ回数初期化
            _jumpCount = _playerConfig.MaxJumpCount;
        }

        private void Awake()
        {
            InputHandler.ins.OnClickOrTap += Jump;
        }

        private void OnDisable()
        {
            InputHandler.ins.OnClickOrTap -= Jump;
        }

        public bool IsWallTouch()
        {
            Vector2 origin = (Vector2)transform.position + Vector2.right * 0.3f;
            RaycastHit2D hit = Physics2D.Raycast(
                origin,
                Vector2.right,
                wallCheckDistance,
                _wallLayer);

            return hit.collider != null;
        }

        private void OnDrawGizmos()
        {
            // 壁判定Rayの可視化
            Vector2 origin = (Vector2)transform.position + Vector2.right * 0.3f;
            Vector2 direction = Vector2.right;
            float distance = wallCheckDistance;

            // Rayの色（壁に当たっていれば緑、当たってなければ赤）
            Color rayColor = Color.red;
        #if UNITY_EDITOR
            if (Application.isPlaying)
            {
                // プレイ中は実際の判定を使う
                RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, _wallLayer);
                if (hit.collider != null)
                    rayColor = Color.green;
            }
        #endif
            Gizmos.color = rayColor;
            Gizmos.DrawLine(origin, origin + direction * distance);
        }

        private void FixedUpdate()
        {
            // Y速度のみ保持（横移動はTransformで制御）
            var yVelocity = _rigidBody2D.linearVelocity.y;
            _rigidBody2D.linearVelocity = new Vector2(0, yVelocity);
        }

        private void Jump()
        {
            if(_gameStateStore.State.CurrentValue.IsRunning == false)
                return;
            
            // 地面にいるか、空中ジャンプ可能な場合
            if ((_groundCheck.CheckGround() || _jumpCount > 0) && _gamePlayerStateStore.State.CurrentValue.NotJumpableTime <= 0f)
            {
                _jumpCount--;
                _rigidBody2D.linearVelocity = new Vector2(0, _playerConfig.JumpSpeed);
                
                Debug.Log($"ジャンプ！残り回数: {_jumpCount}");
            }
        }
    }
}