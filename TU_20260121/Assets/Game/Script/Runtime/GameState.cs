using UnityEngine;

public class GameState : MonoBehaviour
{
    #region Singleton
    public static GameState ins { get; private set; }//Instance
    void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
        else
        {
            Debug.LogWarning("GameStateが複数存在します");
            Destroy(this.gameObject); 
        }
    }
    #endregion

    public int HitCount = 0;
    public float Score = 0f;
    public float Rate;
    

    void Start()
    {
        Score = 0f;
        Rate = GameSettingValues.ins.Rate;
    }

    void Update()
    {
        Score += GameSettingValues.ins.TimeScore * Rate * Time.deltaTime;
    }
}
