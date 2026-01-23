using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LisenceAttach : MonoBehaviour
{
    [SerializeField] private TextAsset lisenceText;
    
    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = lisenceText.text;
    }
}
