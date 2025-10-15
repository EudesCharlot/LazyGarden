using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionReference moveActionRef;
    public float speed = 1;
    public float rotationSpeed = 1;
    void Start()
    {
        transform.position = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString("dronePos"));
    }
    
    void Update()
    {
        Vector2 stickDirection = moveActionRef.action.ReadValue<Vector2>();
        transform.Translate(new(stickDirection.x * speed * Time.deltaTime,0,stickDirection.y * speed * Time.deltaTime),Space.Self);
        transform.Rotate(Vector3.up, stickDirection.x * rotationSpeed * Time.deltaTime);
        
        string JSONTransform = JsonUtility.ToJson(transform.position);
        PlayerPrefs.SetString("dronePos", JSONTransform);
        PlayerPrefs.Save();
    }
    

    public float GetSpeed()
    {
        return speed;
    }
}
