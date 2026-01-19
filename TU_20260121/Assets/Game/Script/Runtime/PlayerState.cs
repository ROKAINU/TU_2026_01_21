using UnityEngine;

public class PlayerState : MonoBehaviour
{
    #region Singleton
    public static PlayerState ins { get; private set; }//Instance
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

    public bool isInvincible = false;
    private float invincibleTime = 2f;
    
    void Start()
    {
        invincibleTime = GameSettingValues.ins.InvincibleTime;
    }

    void Update()
    {
        if(invincibleTime > 0f)
        {
            invincibleTime -= Time.deltaTime;
        }
        else
        {
            InvincibleOff();
        }
    }

    public void InvincibleOn()
    {
        isInvincible = true;
        invincibleTime = GameSettingValues.ins.InvincibleTime;
        GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
    }

    public void InvincibleOff()
    {
        isInvincible = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
