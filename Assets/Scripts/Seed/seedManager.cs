using UnityEngine;

public class seedManager : MonoBehaviour
{
    [Header("Config")]
    public PlantState state = PlantState.Seed;
    public PlantSubType subType = PlantSubType.Carrot;
    public GameValue gameValue;

    [Header("Debug")]
    [SerializeField] private PlantTimers timers;
    [SerializeField] private GameObject currentVisual;

    private GameTimeManager gameTimeManager;
    private int dayLastWatered;
    private int dayLastState;
    private int dayLastHealthyState;
    private int waterStreak;
    private PlantState healthyState;
    private int spawnDay;
    private bool hasBeenWatered;

    void Awake()
    {
        state = PlantState.Seed;
        healthyState = PlantState.Seed;

        gameTimeManager = GameTimeManager.Instance;
        if (gameTimeManager == null)
        {
            Debug.LogError($"❌ {subType}: GameTimeManager null !");
            return;
        }

        timers = gameValue?.GetTimers(subType);
        if (timers == null)
        {
            Debug.LogError($"❌ {subType}: timers null ! Using fallback.");
            timers = new PlantTimers { timeNextState = 2, timeDried = 1, timeFlood = 2, timeDead = 2 };
        }

        int currentDay = gameTimeManager.GetDayCounter();
        spawnDay = currentDay;
        dayLastWatered = currentDay;
        dayLastState = currentDay;
        dayLastHealthyState = currentDay;
        waterStreak = 0;
        hasBeenWatered = false;

        UpdateVisual();

        Debug.Log($"🚀 {subType} AWAKE: state={state}, day={currentDay}, timers={timers.timeNextState}/{timers.timeDried}/{timers.timeFlood}/{timers.timeDead}");
    }

    void Update()
    {
        if (gameTimeManager == null || timers == null) return;

        int currentDay = gameTimeManager.GetDayCounter();

        if (state == PlantState.Dead) return;

        // Protéger le jour du spawn
        if (currentDay == spawnDay)
            return;

        // Récup Flood
        if (state == PlantState.Flood && currentDay - dayLastState >= 1)
        {
            state = healthyState;
            dayLastState = dayLastHealthyState;
            hasBeenWatered = false;
            Debug.Log($"🌊 {subType} Récup Flood → {state}");
            UpdateVisual();
        }

        // Dried → Dead
        if (state == PlantState.Dried && currentDay - dayLastState >= timers.timeDead)
        {
            Dead(currentDay);
            return;
        }

        // Croissance
        if (state != PlantState.Flood && state != PlantState.Dried &&
            currentDay - dayLastState >= timers.timeNextState && hasBeenWatered)
        {
            NextState(currentDay);
            hasBeenWatered = false;
        }

        // Séchage
        if (state != PlantState.Dried && state != PlantState.Flood &&
            currentDay - dayLastWatered >= timers.timeDried)
        {
            Dried(currentDay);
            return;
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

    public void WaterPlant()
    {
        int currentDay = gameTimeManager.GetDayCounter();

        if (currentDay == dayLastWatered)
            waterStreak++;
        else if (currentDay == dayLastWatered + 1)
            waterStreak++;
        else
            waterStreak = 1;

        dayLastWatered = currentDay;
        hasBeenWatered = true;

        if (state == PlantState.Dead) return;

        if (state == PlantState.Flood)
        {
            Dead(currentDay);
            return;
        }

        if (state == PlantState.Dried)
        {
            state = healthyState;
            dayLastState = dayLastHealthyState;
            Debug.Log($"💧 {subType} Récup Dried → {state}");
            UpdateVisual();
            return;
        }

        if (waterStreak >= timers.timeFlood)
        {
            Flooded(currentDay);
            return;
        }

        Debug.Log($"💧 {subType} Arrosé Jour {currentDay}, Streak={waterStreak}");
    }

    void NextState(int currentDay)
    {
        if (state == PlantState.Seed)
        {
            state = PlantState.Sprout;
            GameObject prefab = gameValue.GetPlantPrefab(subType);
            if (prefab)
            {
                Instantiate(prefab, transform.position, Quaternion.identity);
                Destroy(gameObject); // remplace la graine
            }
        }
        else if (state == PlantState.Sprout) state = PlantState.Seedling;
        else if (state == PlantState.Seedling) state = PlantState.Mature;
        else return;

        dayLastState = currentDay;
        dayLastHealthyState = currentDay;
        healthyState = state;
        Debug.Log($"🌱 {subType} → {state}");
    }


    void Flooded(int currentDay)
    {
        healthyState = state;
        dayLastHealthyState = dayLastState;
        state = PlantState.Flood;
        dayLastState = currentDay;
        Debug.Log($"🌊 {subType} FLOOD! Streak={waterStreak}");
        UpdateVisual();
    }

    void Dried(int currentDay)
    {
        healthyState = state;
        dayLastHealthyState = dayLastState;
        state = PlantState.Dried;
        dayLastState = currentDay;
        Debug.Log($"🌵 {subType} DRIED!");
        UpdateVisual();
    }

    void Dead(int currentDay)
    {
        state = PlantState.Dead;
        Debug.Log($"💀 {subType} DEAD!");
        UpdateVisual();
    }

    void Recolt()
    {
        Debug.Log($"🌾 {subType} RECOLT!");
        Destroy(gameObject);
    }

    // ---------------------------
    //  🔹 VISUAL HANDLING
    // ---------------------------
    // ReSharper disable Unity.PerformanceAnalysis
    void UpdateVisual()
    {
        if (currentVisual != null)
            Destroy(currentVisual);

        var prefabs = gameValue.GetPlantPrefab(subType);
        if (prefabs == null)
        {
            Debug.LogWarning($"⚠️ Aucun prefab pour {subType}");
            return;
        }

        GameObject prefabToSpawn = null;

        // Seed prefab si état graine, sinon plante
        if (state == PlantState.Seed)
            prefabToSpawn = prefabs.seedPrefab;
        else
            prefabToSpawn = prefabs.plantPrefab;

        if (prefabToSpawn != null)
        {
            currentVisual = Instantiate(prefabToSpawn, transform);
            currentVisual.transform.localPosition = Vector3.zero;
            currentVisual.transform.localRotation = Quaternion.identity;
        }

        // Assombrir si plante malade ou morte
        if (state == PlantState.Flood || state == PlantState.Dried || state == PlantState.Dead)
        {
            foreach (var renderer in currentVisual.GetComponentsInChildren<Renderer>())
                renderer.material.color = Color.gray;
        }
    }
}
