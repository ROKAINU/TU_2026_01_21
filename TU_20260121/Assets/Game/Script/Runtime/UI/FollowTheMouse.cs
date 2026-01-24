using UnityEngine;
using UnityEngine.UI;

public class FollowTheMouse : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas; // このUIが属するCanvasをインスペクターで設定
    [SerializeField, Range(1f, 30f)]
    private float followSpeed = 10f; // 追従速度
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 anchoredPos;
        // スクリーン座標をUIのローカル座標に変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            mousePos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out anchoredPos
        );

        // スムーズに追従
        rectTransform.anchoredPosition = Vector2.Lerp(
            rectTransform.anchoredPosition,
            anchoredPos,
            Time.deltaTime * followSpeed
        );
    }
}
