using UnityEngine;
using UnityEngine.InputSystem;

public class InteractManager : MonoBehaviour
{
    public InputActionReference interactActionRef;
    public GameObject seedPrefab;

    private GameObject currentSeed;
    private MeshCollider droneCollider;

    private seedManager currentSeedManager;

    void OnEnable()
    {
        interactActionRef.action.performed += OnInteract;
        droneCollider = GetComponent<MeshCollider>();
    }

    void OnDisable()
    {
        interactActionRef.action.performed -= OnInteract;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SeedCollider"))
        {
            currentSeed = other.transform.parent.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        currentSeed = null;
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (currentSeed != null)
        {   
            currentSeedManager = currentSeed.GetComponent<seedManager>();
            currentSeedManager.waterPlant();
            return;
        }

        Vector3 seedPos = new Vector3(transform.position.x, -15.8f, transform.position.z);
        GameObject newSeed = Instantiate(seedPrefab, seedPos, transform.rotation);
        
        newSeed.tag = "Seed";
        currentSeedManager = newSeed.GetComponent<seedManager>();
        //currentSeedManager.subType = graine Séléctionné Par Le Joueur

        Debug.Log("Nouvelle graine plantée !");
    }
}