using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionReference moveActionRef;
    public float moveSpeed = 5f;
    public float rotateSpeed = 180f; // degrés par seconde

    void Update()
    {
        Vector2 stick = moveActionRef.action.ReadValue<Vector2>();
        float moveInput = stick.y;    // avancer/reculer
        float rotateInput = stick.x;  // rotation sur Z

        // Déplacement avant/arrière dans l'axe local inversé Y (forward = -up)
        transform.position += -transform.up * (moveInput * moveSpeed * Time.deltaTime);

        // Rotation sur l'axe Z
        transform.Rotate(0f, 0f, -rotateInput * rotateSpeed * Time.deltaTime, Space.Self);

        // Sauvegarde de la position
        string jsonPos = JsonUtility.ToJson(transform.position);
        PlayerPrefs.SetString("dronePos", jsonPos);
        PlayerPrefs.Save();
    }



    public float GetSpeed()
    {
        return moveSpeed;
    }
}