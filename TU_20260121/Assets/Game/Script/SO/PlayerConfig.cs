using System.Runtime.Serialization;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Game/DataSources/PlayerConfig")]
    internal class PlayerConfig : ScriptableObject
    {
        [Header("初期位置")]
        [SerializeField]private Vector2 initialPosition = Vector2.zero;
        [Header("入場アニメーション速度")]
        [SerializeField]private float entranceAnimSpeed = 1f;
        [Header("通常アニメーション速度")]
        [SerializeField]private float animationSpeed = 1f;
        [Header("重力設定")]
        [SerializeField]private float gravity = 3f;
        [Header("ジャンプ速度設定")]
        [SerializeField]private float jumpSpeed = 3f;
        [Header("追加ジャンプ回数設定")]
        [SerializeField]private int maxJumpCount = 2;
        [Header("ジャンプ不可時間(/s)")]
        [SerializeField]private float cantJumpTime = 0.2f;
        [Header("ダメージを受けた際の移動速度倍率")]
        [SerializeField]private float damagedSpeedRate = 0.5f;
        [Header("ダメージを受けた際の無敵時間(/s)")]
        [SerializeField]private float invincibleTime = 2f;
        [Header("無敵アイテムの効果時間(/s)")]
        [SerializeField]private float invincibleItemTime = 5f;
        [Header("無敵アイテム/スピードアップアイテム使用中の点滅速度(/s)")]
        [SerializeField]private float invincibleItemFlickerFrequency = 10f;
        [Header("スピードアップアイテムの効果倍率")]
        [SerializeField]private float speedUpRate = 1.5f;
        [Header("スピードアップアイテムの効果時間(/s)")]
        [SerializeField]private float speedUpTime = 5f;
        [Header("残像の数")]
        [SerializeField]private int afterimageCount = 5;
        [Header("残像の間隔時間(/s)")]
        [SerializeField]private float afterimageInterval = 0.1f;
        [Header("減速速度の倍率曲線:DontTouch!!!")]
        [SerializeField]private AnimationCurve speedRateCurve;
        [Header("減速入力の最大時間(/s)")]
        [SerializeField]private float maxDecelerateInputTime = 2f;

        public Vector2 InitialPosition => initialPosition;
        public float EntranceAnimSpeed => entranceAnimSpeed;
        public float AnimationSpeed => animationSpeed;
        public float Gravity => gravity;
        public float JumpSpeed => jumpSpeed;
        public int MaxJumpCount => maxJumpCount;
        public float InvincibleTime => invincibleTime;
        public float DamagedSpeedRate => damagedSpeedRate;
        public float CantJumpTime => cantJumpTime;
        public float InvincibleItemTime => invincibleItemTime;
        public float SpeedUpRate => speedUpRate;
        public float SpeedUpTime => speedUpTime;
        public float InvincibleItemFlickerFrequency => invincibleItemFlickerFrequency;
        public int AfterimageCount => afterimageCount;
        public float AfterimageInterval => afterimageInterval;
        public AnimationCurve SpeedRateCurve => speedRateCurve;
        public float MaxDecelerateInputTime => maxDecelerateInputTime;
    }
}

