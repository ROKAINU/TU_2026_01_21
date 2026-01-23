using System.Runtime.Serialization;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Game/DataSources/ItemConfig")]
    internal class ItemConfig : ScriptableObject
    {
        [Header("出現確率の曲線")]
        [SerializeField]private AnimationCurve itemAppearRateCurve;
        [Header("出現回数")]
        [SerializeField]private float itemAppearCount;
        /*[Header("出現確率")]
        [SerializeField]private float itemAppearRate;*/

        public AnimationCurve ItemAppearRateCurve => itemAppearRateCurve;
        public float ItemAppearCount => itemAppearCount;
        //public float ItemAppearRate => itemAppearRate;
    }
}

