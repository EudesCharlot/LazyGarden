using UnityEngine;

public class BoundaryLimiter : MonoBehaviour
{
    public Vector2 Xlimits;
    public Vector2 Zlimits;

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, Xlimits.x, Xlimits.y);
        pos.z = Mathf.Clamp(pos.z, Zlimits.x, Zlimits.y);
        transform.position = pos;
    }
}
