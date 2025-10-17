using UnityEngine;
using UnityEngine.InputSystem;

public class cours_CharacterController : MonoBehaviour
{
    public InputActionReference moveActionRef;
    public float speed = 10f;
    public float rotateSpeed = 50f;
    private CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        Vector2 stickDirection = moveActionRef.action.ReadValue<Vector2>();
        characterController.SimpleMove(transform.forward * (stickDirection.y * speed));
        
        transform.Rotate(Vector3.up, stickDirection.x * rotateSpeed * Time.deltaTime);
    }
}
