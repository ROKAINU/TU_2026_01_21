using UnityEngine;

public class CameraMover : MonoBehaviour
{
    void LateUpdate()
    {
        transform.position = new Vector3(GameController.ins.GameCenter, transform.position.y, transform.position.z);
    }
}
