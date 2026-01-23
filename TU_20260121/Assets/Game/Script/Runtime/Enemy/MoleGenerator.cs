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

        private Store<GameState> _gameStateStore;
        private EnemyConfig _enemyConfig;

        [Inject]
        internal void Construct(Store<GameState> gameStateStore)
        {
            _gameStateStore = gameStateStore;
        }

        /*void Update()
        {
            if (appearDelay <= 0f)
            {
                Instantiate(molePrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }*/
    }
}
