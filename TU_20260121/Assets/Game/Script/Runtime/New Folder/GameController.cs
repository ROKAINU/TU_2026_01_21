using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Singleton
    public static GameController ins { get; private set; }//Instance
    void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
        else
        {
            Debug.LogWarning("GameControllerが複数存在します");
            Destroy(this.gameObject); 
        }
    }
    #endregion

    public float GameCenter;
    public float TimeLimit;

    void Start()
    {
        GameCenter = 0f;
        TimeLimit = GameSettingValues.ins.TimeLimit;
    }

    void Update()
    {
        GameCenter += GameSettingValues.ins.Speed;
        TimeLimit -= Time.deltaTime;

        if(TimeLimit < 0f)
        {
            SceneTransitionerToResultFromPlay.ToResultFromPlay();
        }
    }
}
