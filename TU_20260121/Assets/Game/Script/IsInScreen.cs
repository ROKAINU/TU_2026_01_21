using UnityEngine;

public static class IsInScreen
{
    public static bool judge(Transform transform)
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPos.x >= 0f && viewportPos.x <= 1f &&
                viewportPos.y >= 0f && viewportPos.y <= 1f;
    }
}
