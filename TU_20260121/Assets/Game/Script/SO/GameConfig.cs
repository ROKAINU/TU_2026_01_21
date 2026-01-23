using System.Runtime.Serialization;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Game/DataSources/GameConfig")]
    internal class GameConfig : ScriptableObject
    {
        [Header("ゲーム時間(/s)")]
        [SerializeField]private float timeLimit;
        [Header("入場時間(/s)")]
        [SerializeField]private float entranceTime;
        [Header("カウントダウン表示時間(/s)")]
        [SerializeField]private float showReadyCountTime;   

        [Header("ゲームスピード")]
        [SerializeField]private float speed;
        [Header("スクロール設定")]
        [SerializeField]private float backgroundScrollSpeed = 1f;

        [Header("画面外判定用オフセット(画面外までの距離)")]
        [SerializeField]private float deathZoneOffset;

        public float TimeLimit => timeLimit;
        public float EntranceTime => entranceTime;
        public float ShowReadyCountTime => showReadyCountTime;
        public float Speed => speed;
        public float BackgroundScrollSpeed => backgroundScrollSpeed;
        public float DeathZoneOffset => deathZoneOffset;
    }
}