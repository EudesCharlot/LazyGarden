using UnityEngine;
using UnityEngine.InputSystem;

public class InteractManager : MonoBehaviour
{
    public InputActionReference interactActionRef;
    public GameObject seedPrefab;
    public InventoryManager inventoryManager;

    [Header("Sounds")]
    public AudioClip plantWaterSound;
    public AudioClip plantHarvestSound;
    public AudioClip plantPlantSound;

    private AudioSource audioSource;

    [HideInInspector] public PlantSubType currentSeedSelector = PlantSubType.Null;

    private GameObject currentSeed;
    private seedManager currentSeedManager;
    private bool isNearSeed = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnEnable()
    {
        if (interactActionRef != null)
            interactActionRef.action.performed += OnInteract;
    }

    void OnDisable()
    {
        if (interactActionRef != null)
            interactActionRef.action.performed -= OnInteract;
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
            if (currentSeedManager.state == PlantState.Mature)
            {
                currentSeedManager.Recolt();
                PlaySound(plantHarvestSound);
            }
            else
            {
                currentSeedManager.WaterPlant();
                PlaySound(plantWaterSound);
            }
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
                    if (seed.state == PlantState.Mature)
                    {
                        seed.Recolt();
                        PlaySound(plantHarvestSound);
                    }
                    else
                    {
                        seed.WaterPlant();
                        PlaySound(plantWaterSound);
                    }
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

            PlaySound(plantPlantSound);
        }
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            OnInteract(new InputAction.CallbackContext());
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip);
    }
}
