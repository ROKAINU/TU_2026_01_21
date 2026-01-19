using UnityEngine;
using UnityEngine.UI;

public class UIBackgroundScroller : MonoBehaviour
{
    private float scrollSpeedMultiplier = 1f;
    private RawImage rawImage;
    private float currentOffsetX;
    
    void Start()
    {
        scrollSpeedMultiplier = GameSettingValues.ins.BackgroundScrollSpeedMultiplier;
        rawImage = GetComponent<RawImage>();
        
        if (rawImage == null)
        {
            Debug.LogError("RawImageコンポーネントがありません！");
            return;
        }
    }
    
    void Update()
    {
        if (rawImage == null || GameSettingValues.ins == null) return;
        
        // スクロールオフセットを計算
        currentOffsetX += GameSettingValues.ins.Speed * scrollSpeedMultiplier * Time.deltaTime;
        
        // UVRectを更新してスクロール
        Rect uvRect = rawImage.uvRect;
        uvRect.x = currentOffsetX;
        rawImage.uvRect = uvRect;
    }
}