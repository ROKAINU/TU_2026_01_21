using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject timeTextObject;
    public GameObject scoreTextObject;
    public GameObject scoreRateTextObject;
    private TextMeshProUGUI _timeText;
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _scoreRateext;
    [SerializeField]private string _timeString = "RemainTime : ";
    [SerializeField]private string _scoreString = "Score : ";
    [SerializeField]private string _scoreRateString = "ScoreRate : ";

    void Start()
    {
        _timeText = timeTextObject.GetComponent<TextMeshProUGUI>();
        _scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
        _scoreRateext = scoreRateTextObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _timeText.text = _timeString + GameController.ins.TimeLimit.ToString("F2") + "s";
        _scoreText.text = _scoreString + GameState.ins.Score.ToString("F0");
        _scoreRateext.text = _scoreRateString + GameState.ins.Rate.ToString("F2") + "x";
    }
}
