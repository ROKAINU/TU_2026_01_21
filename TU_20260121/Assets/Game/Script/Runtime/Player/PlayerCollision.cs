using UnityEngine;
using VContainer;
using Game.Redux;
using Game.Runtime.ReduxUtility;

namespace Game.Runtime
{
    public class PlayerCollision : MonoBehaviour
    {
        private PlayerConfig _playerConfig;
        private Store<GameGlobalState> _gameGlobalStore;
        private Store<GamePlayerState> _gamePlayerStateStore;

        [Inject]
        internal void Construct(
            PlayerConfig playerConfig,
            Store<GameGlobalState> gameGlobalStore, 
            Store<GamePlayerState> gamePlayerStateStore)
        {
            _playerConfig = playerConfig;
            _gameGlobalStore = gameGlobalStore;
            _gamePlayerStateStore = gamePlayerStateStore;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // アイテム取得
            if (other.CompareTag("ScoreItem"))
            {
                _gameGlobalStore.Dispatch(new CollectItemAction());
                Destroy(other.gameObject);
                Debug.Log("アイテム取得！");
            }
            else if (other.CompareTag("InvincibleItem"))
            {
                _gamePlayerStateStore.Dispatch(new PlayerGetInvincibleItemAction(_playerConfig.InvincibleItemTime));
                Destroy(other.gameObject);
                Debug.Log("無敵アイテム取得！");
            }
            else if(other.CompareTag("ItemForClear"))
            {
                _gameGlobalStore.Dispatch(new CollectClearItemAction());
                Destroy(other.gameObject);
                Debug.Log("クリア用アイテム取得！");
            }
            else if(other.CompareTag("SpeedUpItem"))
            {
                _gamePlayerStateStore.Dispatch(new PlayerSpeedUpAction(_playerConfig.SpeedUpTime));
                Destroy(other.gameObject);
                Debug.Log("スピードアップアイテム取得！");
            }
            // 敵との衝突
            else if (other.CompareTag("Enemy") || other.CompareTag("CantJumpEnemy"))
            {
                var playerState = _gamePlayerStateStore.State.CurrentValue;
                
                if (!playerState.Invincible)
                {
                    _gamePlayerStateStore.Dispatch(new PlayerDamageAction(_playerConfig.InvincibleTime));
                    _gameGlobalStore.Dispatch(new IncrementHitCountAction());
                    Debug.Log("敵に当たった！");

                    if(other.CompareTag("CantJumpEnemy"))
                    {
                        _gamePlayerStateStore.Dispatch(new PlayerJumpableAction(_playerConfig.CantJumpTime));
                        Debug.Log("ジャンプ不可状態になった！");
                    }
                }
            }
        }
    }
}