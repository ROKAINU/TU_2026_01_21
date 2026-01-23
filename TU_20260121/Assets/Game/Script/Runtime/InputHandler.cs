using NUnit.Framework;
using UnityEngine;

public delegate void OnInput();

public class InputHandler : MonoBehaviour
{
    #region Singleton
    public static InputHandler ins { get; private set; }//Instance
    void Awake()
    {
        if (ins == null) ins = this;
        else 
        {
            Debug.LogWarning("InputHandlerが複数存在します");
            Destroy(this);
        }
    }
    #endregion

    public event OnInput OnClickOrTap;

    void Update()
    {
        var isClicked = false;
        
        if(Input.GetMouseButtonDown(0))
            isClicked = true;

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
                isClicked = true;
            }
        }

        if(isClicked)
            OnClickOrTap.Invoke();
    }
}