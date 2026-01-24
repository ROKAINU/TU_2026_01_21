using System.Runtime.Serialization;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Game/DataSources/EnemyConfig")]
    internal class EnemyConfig : ScriptableObject
    {
        [Header("敵の出現確率の曲線:DontTouch!!!")]
        [SerializeField]private AnimationCurve enemyAppearRateCurve;
        [Header("敵の出現確率")]
        [SerializeField]private float enemyAppearRate;

        [Header("生成する敵のデータリスト:DontTouch!!!")]
        [SerializeField]private GenerateEnemyData[] generateEnemyDataList;

        [Header("カラスの出現警告時間")]
        [SerializeField]private float crowAppearWarningTime;

        [Header("ゴミの移動速度")]
        [SerializeField]private float dustMoveSpeed;

        [Header("モグラの警告の移動速度")]
        [SerializeField]private float moleWarningMoveSpeed;


        public AnimationCurve EnemyAppearRateCurve => enemyAppearRateCurve;
        public float EnemyAppearRate => enemyAppearRate;
        public GenerateEnemyData[] GenerateEnemyDataList => generateEnemyDataList;
        public float CrowAppearWarningTime => crowAppearWarningTime;
        public float DustMoveSpeed => dustMoveSpeed;
        public float MoleWarningMoveSpeed => moleWarningMoveSpeed;
    }

    [System.Serializable]
    public class GenerateEnemyData
    {
        public GameObject Obj;
        public float Probability = 1f;
        public EnemyType Type;
    }

    public enum EnemyType
    {
        Dust,
        Bird,
        Mole
    }
}