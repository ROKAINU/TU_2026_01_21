using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [SerializeField] private GameObject FeadOut;
    [SerializeField] private float feadOutDuration = 1.0f;

    void Start()
    {
        FeadOut.SetActive(false);
    }

    public void StartGame()
    {
        StartPlay().Forget();
    }

    public async UniTask StartPlay()
    {
        FeadOut.SetActive(true);
        float elapsedTime = 0f;
        var img = FeadOut.GetComponent<Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);

        while (elapsedTime < feadOutDuration)
        {
            elapsedTime += Time.deltaTime;
            img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Clamp01(elapsedTime / feadOutDuration));
            await UniTask.Yield();
        }
        SceneLoader.SceneLoad("PlayScene");
    }
}
