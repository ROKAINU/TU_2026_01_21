using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using NUnit.Framework;
using Game;
using System;

namespace Game.Runtime
    {
    public class DustMover : MonoBehaviour
    {
        bool isInScreen = false;
        Rigidbody2D rb;
        Transform thisTransform;
        [SerializeField] private EnemyConfig enemyConfig;
        [SerializeField] private float destroyOffsetX;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            thisTransform = GetComponent<Transform>();
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

            thisTransform.position = new Vector3(transform.position.x + destroyOffsetX, transform.position.y, transform.position.z);

            while(!IsInScreen.judge(thisTransform))
                await UniTask.Yield();

            isInScreen = true;

            while (IsInScreen.judge(thisTransform))
                await UniTask.Yield();

            Destroy(gameObject);
        }
    }
}