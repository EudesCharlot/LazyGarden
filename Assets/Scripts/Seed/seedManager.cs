using UnityEngine;
using System;

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
        gameTimeManager = GameTimeManager.Instance;
        if (gameTimeManager == null)
        {
            Debug.LogError("[seedManager] GameTimeManager manquant !");
            return;
        }

        LoadPlantData();

        timers = gameValue?.GetTimers(subType);
        if (timers == null)
        {
            timers = new PlantTimers { timeNextState = 2, timeDried = 1, timeFlood = 2, timeDead = 2 };
        }

        UpdateVisual();
    }


    void Update()
    {
        int currentDay = gameTimeManager.GetDayCounter();
        if (state == PlantState.Dead || currentDay == spawnDay) return;

        int daysSinceLastState = currentDay - dayLastState;
        int daysSinceLastWater = currentDay - dayLastWatered;

        if (state == PlantState.Flood && daysSinceLastState >= 1)
        {
            state = healthyState;
            dayLastState = dayLastHealthyState;
            UpdateVisual();
        }

        if (daysSinceLastWater >= timers.timeDried && state != PlantState.Dried && state != PlantState.Flood)
        {
            Dried(currentDay);
        }

        if (state != PlantState.Dried && state != PlantState.Flood && daysSinceLastWater < timers.timeDried)
        {
            if (daysSinceLastState >= timers.timeNextState)
                NextState(currentDay);
        }

        if (state == PlantState.Dried && daysSinceLastState >= timers.timeDead)
            Dead(currentDay);
    }

    public void WaterPlant()
    {
        int currentDay = gameTimeManager.GetDayCounter();
        if (state == PlantState.Dead) return;

        waterStreak = (currentDay == dayLastWatered) ? waterStreak + 1 : 1;
        dayLastWatered = currentDay;
        SavePlantData();

        if (waterStreak >= timers.timeFlood)
            Flooded(currentDay);

        if (state == PlantState.Flood)
            Dead(currentDay);
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
        SavePlantData();
        UpdateVisual();
    }

    void Flooded(int currentDay)
    {
        dayLastHealthyState = dayLastState;
        state = PlantState.Flood;
        dayLastState = currentDay;
        SavePlantData();
        UpdateVisual();
    }

    void Dried(int currentDay)
    {
        dayLastHealthyState = dayLastState;
        state = PlantState.Dried;
        dayLastState = currentDay;
        waterStreak = 0;
        SavePlantData();
        UpdateVisual();
    }

    void Dead(int currentDay)
    {
        state = PlantState.Dead;
        SavePlantData();
        if (!currentPlantModel)
        {
            Destroy(gameObject);
            return;
        }

        LeanTween.scale(currentPlantModel, Vector3.zero, 0.5f)
            .setEase(LeanTweenType.easeInBack)
            .setOnComplete(() =>
            {
                Destroy(currentPlantModel);
                Destroy(gameObject);
            });
    }

    public void Recolt()
    {
        if (state != PlantState.Dead)
        {
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
            inventoryManager?.Add(SlotType.Plant, subType, 1);
        }
        PlayerPrefs.DeleteKey(GetSaveKey());
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

        if (targetPrefab == null) return;

        if (currentPrefabReference != targetPrefab)
        {
            if (currentPlantModel) Destroy(currentPlantModel);
            currentPlantModel = Instantiate(targetPrefab, transform.position, transform.rotation, transform);
            currentPrefabReference = targetPrefab;
            Vector3 prefabScale = targetPrefab.transform.localScale;
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

    private void SavePlantData()
    {
        string key = GetSaveKey();
        PlayerPrefs.SetInt(key + "_state", (int)state);
        PlayerPrefs.SetInt(key + "_healthy", (int)healthyState);
        PlayerPrefs.SetInt(key + "_lastWatered", dayLastWatered);
        PlayerPrefs.SetInt(key + "_lastState", dayLastState);
        PlayerPrefs.SetInt(key + "_lastHealthyState", dayLastHealthyState);
        PlayerPrefs.SetInt(key + "_streak", waterStreak);
        PlayerPrefs.SetInt(key + "_spawnDay", spawnDay);
        PlayerPrefs.Save();
    }

    private void LoadPlantData()
    {
        string key = GetSaveKey();

        if (!PlayerPrefs.HasKey(key + "_state"))
        {
            state = PlantState.Seed;
            healthyState = PlantState.Seed;
            dayLastWatered = 0;
            dayLastState = 0;
            dayLastHealthyState = 0;
            waterStreak = 0;
            spawnDay = GameTimeManager.Instance.GetDayCounter();
            return;
        }

        state = (PlantState)PlayerPrefs.GetInt(key + "_state");
        healthyState = (PlantState)PlayerPrefs.GetInt(key + "_healthy");
        dayLastWatered = PlayerPrefs.GetInt(key + "_lastWatered");
        dayLastState = PlayerPrefs.GetInt(key + "_lastState");
        dayLastHealthyState = PlayerPrefs.GetInt(key + "_lastHealthyState");
        waterStreak = PlayerPrefs.GetInt(key + "_streak");
        spawnDay = PlayerPrefs.GetInt(key + "_spawnDay");
    }


    private string GetSaveKey()
    {
        return "plant_" + subType.ToString() + "_" + transform.position.GetHashCode();
    }
    
    public PlantSaveManager.PlantData GetSaveData()
    {
        return new PlantSaveManager.PlantData
        {
            subType = subType,
            state = state,
            dayLastWatered = dayLastWatered,
            dayLastState = dayLastState,
            dayLastHealthyState = dayLastHealthyState,
            waterStreak = waterStreak,
            posX = transform.position.x,
            posY = transform.position.y,
            posZ = transform.position.z
        };
    }

    public void SetSavedData(PlantSaveManager.PlantData data)
    {
        state = data.state;
        dayLastWatered = data.dayLastWatered;
        dayLastState = data.dayLastState;
        dayLastHealthyState = data.dayLastHealthyState;
        waterStreak = data.waterStreak;
        transform.position = new Vector3(data.posX, data.posY, data.posZ);
        UpdateVisual();
    }


}
