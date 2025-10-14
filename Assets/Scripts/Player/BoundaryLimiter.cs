using UnityEngine;

public class BoundaryLimiter : MonoBehaviour
{
    public Vector2 Xlimits;
    public Vector2 Zlimits;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 pos = rb.position;

        float clampedX = Mathf.Clamp(pos.x, Xlimits.x, Xlimits.y);
        float clampedZ = Mathf.Clamp(pos.z, Zlimits.x, Zlimits.y);

        Vector3 clampedPos = new Vector3(clampedX, pos.y, clampedZ);
        rb.MovePosition(clampedPos);
    }
}