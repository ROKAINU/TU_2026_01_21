using Game.Runtime.ReduxUtility;
using UnityEngine;
using Game.Redux;
using VContainer;

namespace Game.Runtime
{
    public class MoleGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject molePrefab;
        [SerializeField] private float xOffset;
        [SerializeField] private float yOffset;
        [SerializeField] private float moleYOffset;
        [SerializeField] private float AppearPos;

        private float moleWarningSpeed;

        internal Store<GameState> _gameStateStore;
        internal GameObject _playerObject;
        internal EnemyConfig _enemyConfig;

        internal void Init(Store<GameState> gameStateStore, GameObject playerObject, EnemyConfig enemyConfig)
        {
            _gameStateStore = gameStateStore;
            _playerObject = playerObject;
            _enemyConfig = enemyConfig;
        }

        void Start()
        {
            Vector3 pos = transform.position;
            pos.y += yOffset;
            transform.position = pos;
        }

        void Update()
        {
            if(!IsInScreen.judge(transform)) 
                return;
            
            if(_gameStateStore.State.CurrentValue.IsRunning == false) 
                return;
        
            this.transform.position += Vector3.left * _enemyConfig.MoleWarningMoveSpeed * Time.deltaTime;

            if(_playerObject.transform.position.x + AppearPos < transform.position.x) 
                return;
            
            Vector3 pos = transform.position;
            pos.y += moleYOffset;
            Instantiate(molePrefab, pos, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
