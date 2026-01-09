using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionReference moveActionRef;
    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;

    void Awake()
    {
        LoadPosition();
    }

    void Update()
    {
        Vector2 stick = moveActionRef.action.ReadValue<Vector2>();
        float moveInput = stick.y;
        float rotateInput = stick.x;
        
        transform.position += -transform.up * (moveInput * moveSpeed * Time.deltaTime);
        transform.Rotate(0f, 0f, -rotateInput * rotateSpeed * Time.deltaTime, Space.Self);
        
        SavePosition();
    }

    public float GetSpeed()
    {
        return moveSpeed;
    }

    void SavePosition()
    {
        string jsonPos = JsonUtility.ToJson(transform.position);
        PlayerPrefs.SetString("playerPos", jsonPos);
        PlayerPrefs.Save();
    }

    void LoadPosition()
    {
        if (PlayerPrefs.HasKey("playerPos"))
        {
            string jsonPos = PlayerPrefs.GetString("playerPos");
            transform.position = JsonUtility.FromJson<Vector3>(jsonPos);
        }
    }
}