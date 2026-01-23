using Game.Runtime.ReduxUtility;
using UnityEngine;
using Game.Redux;
using Unity.VisualScripting;

namespace Game.Runtime
{
    public class CrowGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject crowPrefab;
        [SerializeField] private float appearDelay;
        [SerializeField] private float xOffset;

        internal Store<GameState> _gameStateStore;
        internal EnemyConfig _enemyConfig;

        internal void Init(Store<GameState> gameStateStore, EnemyConfig enemyConfig)
        {
            _gameStateStore = gameStateStore;
            _enemyConfig = enemyConfig;
            appearDelay = _enemyConfig.CrowAppearWarningTime;
        }

        void Update()
        {
            if(!IsInScreen.judge(transform)) return;
            if (_gameStateStore == null || _gameStateStore.State == null || _gameStateStore.State.CurrentValue == null)
            {
                Debug.LogError("CrowGenerator: gameStateStore or its members are null!");
                return;
            }

            transform.position = new Vector3(_gameStateStore.State.CurrentValue.GameCenter + xOffset, transform.position.y, 0f);
            appearDelay -= Time.deltaTime;
            Debug.Log($"カラスの出現までの時間: {appearDelay}");
            if (appearDelay <= 0f)
            {
                Instantiate(crowPrefab, transform.position, Quaternion.identity);
                Debug.Log($"カラスを生成しました: 位置 {transform.position}");
                Destroy(gameObject);
            }
        }
    }
}