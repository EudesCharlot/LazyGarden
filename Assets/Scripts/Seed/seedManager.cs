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
    private int consecutiveWateredDays;
    private bool readyToGrow;

    private GameObject currentPlantModel;

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
        dayLastWatered = currentDay - 1; // Permet arrosage jour 0
        dayLastState = currentDay;
        dayLastHealthyState = currentDay;
        waterStreak = 0;
        consecutiveWateredDays = 0;
        readyToGrow = false;

        UpdateVisual(); // Initialise avec cylindre visible

        Debug.Log($"🚀 {subType} AWAKE: state={state}, day={currentDay}, timers={timers.timeNextState}/{timers.timeDried}/{timers.timeFlood}/{timers.timeDead}");
    }

    void Update()
    {
        int currentDay = gameTimeManager.GetDayCounter();
        Debug.Log($"📅 {subType} Jour {currentDay}: {state} | Water:{dayLastWatered} | State:{dayLastState} | Streak:{waterStreak} | Consecutive:{consecutiveWateredDays} | ReadyToGrow:{readyToGrow}");

        if (state == PlantState.Dead) return;

        if (currentDay == spawnDay) return; // Protection spawn

        if (currentDay - dayLastWatered > 1) // Gap arrosage
        {
            consecutiveWateredDays = 0;
            readyToGrow = false;
        }

        if (state == PlantState.Flood && currentDay - dayLastState >= 1)
        {
            state = healthyState;
            dayLastState = dayLastHealthyState;
            UpdateVisual();
            Debug.Log($"🌊 {subType} Récup Flood → {state}");
        }

        if (state == PlantState.Dried && currentDay - dayLastState >= timers.timeDead)
        {
            Dead(currentDay);
            return;
        }

        if (readyToGrow && currentDay > dayLastState)
        {
            NextState(currentDay);
            readyToGrow = false;
            consecutiveWateredDays = 0;
        }

        if (state != PlantState.Dried && state != PlantState.Flood && currentDay - dayLastWatered >= timers.timeDried)
        {
            Dried(currentDay);
        }
    }

    public void Interact()
    {
        Debug.Log($"🖱️ {subType} Interact: state={state}");
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
        {
            waterStreak++;
        }
        else if (currentDay == dayLastWatered + 1)
        {
            waterStreak++;
            consecutiveWateredDays++;
        }
        else
        {
            waterStreak = 1;
            consecutiveWateredDays = 1;
        }
        dayLastWatered = currentDay;
        Debug.Log($"💧 {subType} Arrosé Jour {currentDay}, Streak={waterStreak}, Consecutive={consecutiveWateredDays}");

        if (consecutiveWateredDays >= timers.timeNextState)
        {
            readyToGrow = true;
        }

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
            UpdateVisual();
            Debug.Log($"💧 {subType} Récup Dried → {state}");
            return;
        }

        if (waterStreak >= timers.timeFlood)
        {
            Flooded(currentDay);
        }
    }

    void NextState(int currentDay)
    {
        if (state == PlantState.Seed) state = PlantState.Sprout;
        else if (state == PlantState.Sprout) state = PlantState.Seedling;
        else if (state == PlantState.Seedling) state = PlantState.Mature;
        else return;

        dayLastState = currentDay;
        dayLastHealthyState = currentDay;
        healthyState = state;
        UpdateVisual();
        Debug.Log($"🌱 {subType} → {state}");
    }

    void Flooded(int currentDay)
    {
        healthyState = state;
        dayLastHealthyState = dayLastState;
        state = PlantState.Flood;
        dayLastState = currentDay;
        UpdateVisual();
        Debug.Log($"🌊 {subType} FLOOD! Streak={waterStreak}");
    }

    void Dried(int currentDay)
    {
        healthyState = state;
        dayLastHealthyState = dayLastState;
        state = PlantState.Dried;
        dayLastState = currentDay;
        UpdateVisual();
        Debug.Log($"🌵 {subType} DRIED!");
    }

    void Dead(int currentDay)
    {
        state = PlantState.Dead;
        UpdateVisual();
        Debug.Log($"💀 {subType} DEAD!");
    }

    void Recolt()
    {
        Debug.Log($"🌾 {subType} RECOLT!");
        Destroy(gameObject);
    }

    private void UpdateVisual()
    {
        if (state == PlantState.Seed)
        {
            if (currentPlantModel) Destroy(currentPlantModel);
            currentPlantModel = null;
            GetComponent<MeshRenderer>().enabled = true; // Cylindre visible
            return;
        }

        GetComponent<MeshRenderer>().enabled = false; // Cache cylindre

        if (currentPlantModel == null)
        {
            GameObject prefab = gameValue.GetPlantPrefab(subType);
            if (prefab == null) { Debug.LogError($"❌ {subType}: Pas de prefab !"); return; }
            currentPlantModel = Instantiate(prefab, transform.position, transform.rotation, transform);
        }

        MeshRenderer renderer = currentPlantModel.GetComponentInChildren<MeshRenderer>();
        if (renderer == null) { Debug.LogError($"❌ {subType}: Pas de renderer !"); return; }

        Color color = state switch
        {
            PlantState.Flood => Color.blue,
            PlantState.Dried => Color.yellow,
            PlantState.Dead => Color.gray,
            _ => Color.green
        };
        renderer.material.color = color;
    }
}