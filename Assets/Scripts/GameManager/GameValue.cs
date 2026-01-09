using UnityEngine;
using System.Collections.Generic;

public enum SlotType
{
    Seed,
    Plant
}

public enum PlantState
{
    Seed,
    Sprout,
    Seedling,
    Mature,
    Flood,
    Dried,
    Dead
}

public enum PlantType
{
    Fruit,
    Vegetable
}

public enum PlantSubType
{
    GoldenApple,
    Apple,
    Banana,
    Orange,
    Carrot,
    Corn,
    Eggplant,
    GoldenCarrot,
    Null
}

[System.Serializable]
public class PlantTimers
{
    public int timeNextState;
    public int timeDried;
    public int timeFlood;
    public int timeDead;
}

[System.Serializable]
public class PlantSubTypeTimers
{
    public PlantSubType subType;
    public PlantTimers timers;
}

[System.Serializable]
public class PlantImages
{
    public Sprite seedSprite;
    public Sprite plantSprite;
}

[System.Serializable]
public class PlantPrefabs 
{
    public PlantSubType subType;
    public GameObject sproutPrefab;
    public GameObject maturePrefab;
    public PlantImages images;
}

[CreateAssetMenu(fileName = "GameValue", menuName = "Scriptable Objects/GameValue")]
public class GameValue : ScriptableObject
{
    public List<PlantSubTypeTimers> allPlantTimers = new List<PlantSubTypeTimers>();

    public List<PlantPrefabs> allPlantPrefabs = new List<PlantPrefabs>();
    
    public GameObject GetSproutPrefab(PlantSubType subtype)
    {
        return allPlantPrefabs.Find(p => p.subType == subtype)?.sproutPrefab;
    }

    public GameObject GetMaturePrefab(PlantSubType subtype)
    {
        return allPlantPrefabs.Find(p => p.subType == subtype)?.maturePrefab;
    }

    public PlantTimers GetTimers(PlantSubType subtype)
    {
        return allPlantTimers.Find(p => p.subType == subtype)?.timers;
    }

    public PlantImages GetImages(PlantSubType subtype)
    {
        foreach (var p in allPlantPrefabs)
        {
            if (p.subType == subtype)
            {
                if (p.images == null)
                    Debug.LogError($"Images NULL pour {subtype}");

                return p.images;
            }
        }

        Debug.LogError($"Aucune entrée PlantPrefabs pour {subtype}");
        return null;
    }

}