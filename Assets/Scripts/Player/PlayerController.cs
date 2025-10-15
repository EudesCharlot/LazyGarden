using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionReference moveActionRef;
    public float speed = 1;
    public float rotationSpeed = 1;
    void Awake()
    {
        
    }
    
    void Update()
    {
        Vector2 stickDirection = moveActionRef.action.ReadValue<Vector2>();
        transform.Translate(new(stickDirection.x * speed * Time.deltaTime,0,stickDirection.y * speed * Time.deltaTime),Space.Self);
        transform.Rotate(Vector3.up, stickDirection.x * rotationSpeed * Time.deltaTime);
    }
    

    public float GetSpeed()
    {
        return speed;
    }
}
