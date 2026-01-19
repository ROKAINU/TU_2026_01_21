using UnityEngine;

public class StartButton : MonoBehaviour
{
    public void StartGame()
    {
        SceneLoader.SceneLoad("PlayScene");
    }
}
