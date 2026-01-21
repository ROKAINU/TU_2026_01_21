using System.Runtime.Serialization;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Game/DataSources/GameConfig")]
    internal class GameConfig : ScriptableObject
    {
        [Header("スクロール設定")]
        [SerializeField]private float backgroundScrollSpeedMultiplier = 1f;

        public float BackgroundScrollSpeedMultiplier => backgroundScrollSpeedMultiplier;
    }
}