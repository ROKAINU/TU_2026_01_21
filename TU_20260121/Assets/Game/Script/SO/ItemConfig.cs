using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Game/DataSources/ItemConfig")]
    internal class ItemConfig : ScriptableObject
    {
        [Header("出現確率の曲線:DontTouch!!!")]
        [SerializeField]private AnimationCurve itemAppearRateCurve;
        [Header("出現確率")]
        [SerializeField]private float itemAppearRate;
        [Header("出現回数")]
        [SerializeField]private float itemAppearCount;
        [Header("生成するアイテムの高さ")]
        [SerializeField]private float generateItemOffsetY;
        [Header("クリアアイテムの出現確率:DontTouch!!!")]
        [SerializeField]private float clearItemAppearRate;

        [SerializeField]private GameObject generateItemPrefab;
        [SerializeField]private GameObject generateClearItemPrefab;

        public AnimationCurve ItemAppearRateCurve => itemAppearRateCurve;
        public float ItemAppearRate => itemAppearRate;
        public float ItemAppearCount => itemAppearCount;
        public GameObject GenerateItemPrefab => generateItemPrefab;
        public GameObject GenerateClearItemPrefab => generateClearItemPrefab;
        public float GenerateItemOffsetY => generateItemOffsetY;
        public float ClearItemAppearRate => clearItemAppearRate;
    }
}

