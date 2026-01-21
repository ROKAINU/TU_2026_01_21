using System.Runtime.Serialization;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Game/DataSources/PlayerConfig")]
    internal class PlayerConfig : ScriptableObject
    {
        [Header("重力設定")]
        [SerializeField]private float gravity = 3f;
        [Header("ジャンプ速度設定")]
        [SerializeField]private float jumpSpeed = 3f;
        [Header("ジャンプ回数設定")]
        [SerializeField]private int maxJumpCount = 2;

        public float Gravity => gravity;
        public float JumpSpeed => jumpSpeed;
        public int MaxJumpCount => maxJumpCount;
    }
}

