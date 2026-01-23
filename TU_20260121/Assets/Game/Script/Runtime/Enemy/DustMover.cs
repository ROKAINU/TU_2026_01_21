using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using NUnit.Framework;
using Game;

namespace Game.Runtime
    {
    public class DustMover : MonoBehaviour
    {
        bool isInScreen = false;
        Rigidbody2D rb;
        [SerializeField] private EnemyConfig enemyConfig;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            MoveDust().Forget();
        }

        void FixedUpdate()
        {
            if(this == null) return;

            if(isInScreen)
            {
                rb.linearVelocityX = enemyConfig.DustMoveSpeed;
            }
        }

        private async UniTask MoveDust()
        {
            if(!this) return;
            
            isInScreen = false;

            while(!IsInScreen.judge(transform))
                await UniTask.Yield();

            isInScreen = true;

            while (IsInScreen.judge(transform))
                await UniTask.Yield();

            Destroy(gameObject);
        }
    }
}