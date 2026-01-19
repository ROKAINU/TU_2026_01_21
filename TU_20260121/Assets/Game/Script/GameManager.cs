using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager ins { get; private set; }//Instance
    void Awake()
    {
        if (ins == null)
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            debug.LogWarning("GameManagerが複数存在します");
            Destroy(this.gameObject); 
        }
    }
    #endregion

    public GameValues gameValues;
    
    void Start()
    {
        
    }
}
