using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float movementX;
    public float movementY;
    public float speed = 1;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.MovePosition(rb.position + movement * (speed * Time.fixedDeltaTime));
    }
    
    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
 
        movementX = movementVector.x;
        movementY = movementVector.y;
    }
}
