using UnityEngine;

public class seedManager : MonoBehaviour
{
    [Header("Config")]
    public PlantState state = PlantState.Seed;
    public PlantSubType subType = PlantSubType.Carrot;
    public GameValue gameValue;

    [Header("Debug")]
    [SerializeField] private PlantTimers timers;

    private GameTimeManager gameTimeManager;
    private int dayLastWatered;
    private int dayLastState;
    private int dayLastHealthyState;
    private int waterStreak;
    private PlantState healthyState;
    private int spawnDay;

    private GameObject currentPlantModel;
    private GameObject currentPrefabReference;

    private bool HasGrown =>
        healthyState == PlantState.Sprout ||
        healthyState == PlantState.Seedling ||
        healthyState == PlantState.Mature;

    void Awake()
    {
        state = PlantState.Seed;
        healthyState = PlantState.Seed;

        gameTimeManager = GameTimeManager.Instance;
        if (gameTimeManager == null)
        {
            Debug.LogError($"[seedManager] GameTimeManager manquant !");
            return;
        }

        timers = gameValue?.GetTimers(subType);
        if (timers == null)
        {
            Debug.LogError($"[seedManager] Timers manquants pour {subType}");
            timers = new PlantTimers { timeNextState = 2, timeDried = 1, timeFlood = 2, timeDead = 2 };
        }

        int currentDay = gameTimeManager.GetDayCounter();
        spawnDay = currentDay;
        dayLastWatered = currentDay - 1;
        dayLastState = currentDay;
        dayLastHealthyState = currentDay;
        waterStreak = 0;

        UpdateVisual();
        Debug.Log($"[seedManager] {subType} créé | Jour {currentDay}");
    }

    void Update()
    {
        int currentDay = gameTimeManager.GetDayCounter();

        if (state == PlantState.Dead || currentDay == spawnDay) return;

        int daysSinceLastState = currentDay - dayLastState;
        int daysSinceLastWater = currentDay - dayLastWatered;

        // Gestion Flood
        if (state == PlantState.Flood && daysSinceLastState >= 1)
        {
            state = healthyState;
            dayLastState = dayLastHealthyState;
            UpdateVisual();
            Debug.Log($"[seedManager] {subType} récupéré du Flood");
        }

        // Gestion Dried si pas arrosée depuis trop longtemps
        if (daysSinceLastWater >= timers.timeDried && state != PlantState.Dried && state != PlantState.Flood)
        {
            Dried(currentDay);
        }

        // Gestion croissance si arrosée
        if (state != PlantState.Dried && state != PlantState.Flood && daysSinceLastWater < timers.timeDried)
        {
            if (daysSinceLastState >= timers.timeNextState)
            {
                NextState(currentDay);
            }
        }

        // Gestion mort si Dried trop longtemps
        if (state == PlantState.Dried && daysSinceLastState >= timers.timeDead)
        {
            Dead(currentDay);
        }
    }

    public void WaterPlant()
    {
        int currentDay = gameTimeManager.GetDayCounter();
        if (state == PlantState.Dead) return;

        waterStreak = (currentDay == dayLastWatered) ? waterStreak + 1 : 1;
        dayLastWatered = currentDay;

        Debug.Log($"[seedManager] {subType} arrosé | Streak: {waterStreak}");

        // Flood si trop arrosé
        if (waterStreak >= timers.timeFlood)
        {
            Flooded(currentDay);
        }

        // Si était Flooded et arrosé → mort immédiate
        if (state == PlantState.Flood)
        {
            Dead(currentDay);
        }
    }


    public void Interact()
    {
        if (state == PlantState.Mature)
        {
            Recolt();
            return;
        }
        WaterPlant();
    }
    

    void NextState(int currentDay)
    {
        PlantState next = healthyState switch
        {
            PlantState.Seed => PlantState.Sprout,
            PlantState.Sprout => PlantState.Seedling,
            PlantState.Seedling => PlantState.Mature,
            _ => healthyState
        };

        if (next == healthyState) return;

        healthyState = next;
        state = healthyState;
        dayLastState = currentDay;
        dayLastHealthyState = currentDay;
        waterStreak = 0;

        UpdateVisual();
        Debug.Log($"[seedManager] {subType} → {state} (Jour {currentDay})");
    }

    void Flooded(int currentDay)
    {
        dayLastHealthyState = dayLastState;
        state = PlantState.Flood;
        dayLastState = currentDay;
        UpdateVisual();
        Debug.Log($"[seedManager] {subType} FLOODED !");
    }

    void Dried(int currentDay)
    {
        dayLastHealthyState = dayLastState;
        state = PlantState.Dried;
        dayLastState = currentDay;
        waterStreak = 0;
        UpdateVisual();
        Debug.Log($"[seedManager] {subType} DRIED !");
    }
    
    void Dead(int currentDay)
    {
        state = PlantState.Dead;
        UpdateVisual();
    }

    public void Recolt()
    {
        if (state != PlantState.Dead)
        {
            if (gameValue != null && gameValue.GetImages(subType) != null)
            {
                var interactManager = FindObjectOfType<InteractManager>();
                if (interactManager != null && interactManager.inventoryManager != null)
                {
                    interactManager.inventoryManager.Add(SlotType.Plant, subType, 1);
                }
            }
        }
        Destroy(gameObject);
    }


    private void UpdateVisual()
    {
        if (!HasGrown)
        {
            if (currentPlantModel) Destroy(currentPlantModel);
            currentPlantModel = null;
            currentPrefabReference = null;
            GetComponent<MeshRenderer>().enabled = true;
            return;
        }

        GetComponent<MeshRenderer>().enabled = false;

        GameObject targetPrefab = healthyState switch
        {
            PlantState.Sprout => gameValue.GetSproutPrefab(subType),
            PlantState.Seedling => gameValue.GetSproutPrefab(subType),
            PlantState.Mature => gameValue.GetMaturePrefab(subType),
            _ => null
        };

        if (targetPrefab == null)
        {
            Debug.LogError($"[seedManager] Prefab manquant pour {subType} ({healthyState})");
            return;
        }

        if (currentPrefabReference != targetPrefab)
        {
            if (currentPlantModel) Destroy(currentPlantModel);

            currentPlantModel = Instantiate(targetPrefab, transform.position, transform.rotation, transform);
            currentPrefabReference = targetPrefab;

            Vector3 prefabScale = targetPrefab.transform.localScale;

            // Animation pop
            currentPlantModel.transform.localScale = prefabScale * 0.01f;
            LeanTween.scale(currentPlantModel, prefabScale, 0.5f)
                     .setEase(LeanTweenType.easeOutBack);
        }

        MeshRenderer[] renderers = currentPlantModel.GetComponentsInChildren<MeshRenderer>();
        Color col = state switch
        {
            PlantState.Flood => new Color(0.1f, 0.4f, 0.9f),
            PlantState.Dried => new Color(0.85f, 0.7f, 0.3f),
            PlantState.Dead => new Color(0.35f, 0.35f, 0.35f),
            _ => Color.white
        };

        foreach (var r in renderers)
        {
            if (r.material.HasProperty("_BaseColor"))
                r.material.SetColor("_BaseColor", col);
            else if (r.material.HasProperty("_Color"))
                r.material.SetColor("_Color", col);
        }
    }
}
