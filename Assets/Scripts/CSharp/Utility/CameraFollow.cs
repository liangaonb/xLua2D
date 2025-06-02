using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (target)
        {
            Vector3 targetPosition = new Vector3(target.position.x + offset.x,  target.position.y + offset.y, transform.position.z);
            transform.position = targetPosition;
        }
    }
}
