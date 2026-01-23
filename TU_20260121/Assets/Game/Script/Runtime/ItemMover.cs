using NUnit.Framework;
using UnityEngine;
using UnityEngine.Analytics;

public class ItemMover : MonoBehaviour
{
    float speed = 400f;

    void Update()
    {
        if (IsInScreen.judge(transform))
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + Time.deltaTime * speed, 0);
    }
}
