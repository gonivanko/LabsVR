using UnityEngine;

public class FollowObjectPosition : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;

        var cam = Camera.main;
        if (cam != null)
        {
            transform.LookAt(cam.transform);
            transform.Rotate(0, 180f, 0);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }
}