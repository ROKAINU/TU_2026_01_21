using UnityEngine;

public static class IsInScreen
{
    public static bool judge(Transform transform)
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPos.x >= -0.01f && viewportPos.x <= 1.01f &&
                viewportPos.y >= -0.01f && viewportPos.y <= 1.01f;
    }
}
