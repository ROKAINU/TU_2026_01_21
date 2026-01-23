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
        [SerializeField] private float AppearPos;

        internal Store<GameState> _gameStateStore;
        internal GameObject _playerObject;
        internal EnemyConfig _enemyConfig;

        internal void Init(Store<GameState> gameStateStore, GameObject playerObject, EnemyConfig enemyConfig)
        {
            _gameStateStore = gameStateStore;
            _playerObject = playerObject;
            _enemyConfig = enemyConfig;
        }

        void Update()
        {
            if(!IsInScreen.judge(transform)) 
                return;

            if(_playerObject.transform.position.x + AppearPos < transform.position.x) 
                return;
            
            Instantiate(molePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
