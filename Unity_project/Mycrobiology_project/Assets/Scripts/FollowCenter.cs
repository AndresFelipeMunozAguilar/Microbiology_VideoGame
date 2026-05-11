using UnityEngine;

public class FollowCenter : MonoBehaviour
{
    private Camera cam;

    private Vector3 offset = Vector3.zero;

    private void Start()
    {
        cam = Camera.main;
        ApplyPosition();
    }

    private void LateUpdate()
    {
        ApplyPosition();
    }

    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
        ApplyPosition();
    }

    public Vector3 GetOffset()
    {
        return offset;
    }

    public void ApplyPosition()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        if (cam == null)
        {
            return;
        }

        Vector3 center = cam.ViewportToWorldPoint(
            new Vector3(0.5f, 0.5f, Mathf.Abs(cam.transform.position.z))
        );

        center.z = 0f;

        transform.position = center + offset;
    }
}