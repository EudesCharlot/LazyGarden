using UnityEngine;
using UnityEngine.InputSystem;

public class InteractManager : MonoBehaviour
{
    public InputActionReference interactActionRef;

    void OnEnable()
    {
        interactActionRef.action.performed += OnInteract;
        interactActionRef.action.Enable();
    }

    void OnDisable()
    {
        interactActionRef.action.performed -= OnInteract;
        interactActionRef.action.Disable();
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        Debug.Log("J'intéragis eheh");
    }
}

