using UnityEngine;
using UnityEngine.InputSystem;

public class InteractManager : MonoBehaviour
{
    public InputActionReference interactActionRef;
    public GameObject seedPrefab;
    public InventoryManager inventoryManager;

    [HideInInspector] public PlantSubType currentSeedSelector = PlantSubType.Null;

    private GameObject currentSeed;
    private seedManager currentSeedManager;
    private bool isNearSeed = false;

    void OnEnable()
    {
        if (interactActionRef != null) interactActionRef.action.performed += OnInteract;
    }

    void OnDisable()
    {
        if (interactActionRef != null) interactActionRef.action.performed -= OnInteract;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Seed"))
        {
            isNearSeed = true;
            currentSeed = other.gameObject;
            currentSeedManager = currentSeed.GetComponent<seedManager>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Seed") && other.gameObject == currentSeed)
        {
            isNearSeed = false;
            currentSeed = null;
            currentSeedManager = null;
        }
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (isNearSeed && currentSeed != null && currentSeedManager != null)
        {
            currentSeedManager.Interact();
            return;
        }

        Collider[] nearbySeeds = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (var col in nearbySeeds)
        {
            if (col.CompareTag("Seed"))
            {
                var seed = col.GetComponent<seedManager>();
                if (seed != null)
                {
                    seed.Interact();
                    return;
                }
            }
        }

        if (currentSeedSelector != PlantSubType.Null &&
            inventoryManager.Consume(SlotType.Seed, currentSeedSelector, 1))
        {
            Vector3 seedPos = new Vector3(transform.position.x, -13.5f, transform.position.z);
            GameObject newSeed = Instantiate(seedPrefab, seedPos, Quaternion.identity);
            newSeed.tag = "Seed";
            currentSeed = newSeed;
            currentSeedManager = currentSeed.GetComponent<seedManager>();
            currentSeedManager.subType = currentSeedSelector;

        }
        else
        {
            
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnInteract(new InputAction.CallbackContext());
        }
    }
}