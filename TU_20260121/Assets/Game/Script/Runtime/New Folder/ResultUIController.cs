using TMPro;
using UnityEngine;

public class ResultUIController : MonoBehaviour
{
    public GameObject scoreTextObject;
    public GameObject hitCountTextObject;
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _hitCountText;
    [SerializeField]private string _scoreString = "Score : ";
    [SerializeField]private string _HitCountString = "HitCount : ";

    void Start()
    {
        _scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
        _hitCountText = hitCountTextObject.GetComponent<TextMeshProUGUI>();
        _scoreText.text = _scoreString + GameManager.ins.globalValues.Score.ToString("F0");
        _hitCountText.text = _HitCountString + GameManager.ins.globalValues.HitCount.ToString();
    }
}
