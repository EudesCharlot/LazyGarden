using UnityEngine;
using UnityEngine.InputSystem;

public class InteractManager : MonoBehaviour
{
    public InputActionReference interactActionRef;
    public GameObject seedPrefab;

    private GameObject currentSeed;
    private MeshCollider droneCollider;
    private seedManager currentSeedManager;
    private bool isNearSeed = false; 

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
        if (other.CompareTag("Seed"))
        {
            isNearSeed = true;
            currentSeed = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Seed") && other.gameObject == currentSeed)
        {
            isNearSeed = false;
            currentSeed = null;
        }
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        
        if (isNearSeed && currentSeed != null)
        {
            currentSeedManager = currentSeed.GetComponent<seedManager>();
            currentSeedManager.Interact();
            return;
        }
        
        Collider[] nearbySeeds = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (var col in nearbySeeds)
        {
            if (col.CompareTag("Seed"))
            {
                Debug.Log("Impossible de planter ici : une graine est déjà présente !");
                return;
            }
        }

        Vector3 seedPos = new Vector3(transform.position.x, -15.8f, transform.position.z);
        GameObject newSeed = Instantiate(seedPrefab, seedPos, Quaternion.identity);
        newSeed.tag = "Seed";
        currentSeed = newSeed;
        currentSeedManager = currentSeed.GetComponent<seedManager>();
        //currentSeedManager.subType = séléctionné par le joueur
        Debug.Log("Nouvelle graine plantée !");
    }
}
