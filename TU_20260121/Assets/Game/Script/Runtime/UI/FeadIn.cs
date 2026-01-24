using Unity.VisualScripting;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class FeadIn : MonoBehaviour
{
    [SerializeField] private float feadInDuration = 1f;
    [SerializeField] private bool startFeadIn = true;
    [SerializeField] private GameObject feadInImage;

    float timer = 0f;

    void Start()
    {
        feadInImage.SetActive(false);
        timer = 0f;

        if(startFeadIn)
        {
            FeadInAsync().Forget();   
        }
    }

    async UniTask FeadInAsync()
    {
        Debug.Log("フェードイン開始");

        timer = feadInDuration;
        feadInImage.SetActive(true);

        var color = feadInImage.GetComponent<UnityEngine.UI.Image>().color;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            float alpha = timer / feadInDuration;
            color.a = alpha;
            feadInImage.GetComponent<UnityEngine.UI.Image>().color = color;

            await UniTask.Yield();
        }

        feadInImage.SetActive(false);
    }
}
