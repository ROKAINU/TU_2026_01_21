using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{        
    public static void SceneLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}