using UnityEngine;

public class OpenOrCloseWindow : MonoBehaviour
{
    [SerializeField] private GameObject window;

    public void OpenOrClose(bool isOpen)
    {
        if(isOpen)
            window.SetActive(true);
        else
            window.SetActive(false);
    }
}
