using UnityEngine;
using System.Collections.Generic;

public enum SlotType
{
    Seed,
    Plant,
    Tool,
    Item
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
    Cherry,
    Banana,
    Orange,
    Carrot,
    Lettuce,
    Cucumber,
    Broccoli,
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
public class PlantPrefab
{
    public PlantSubType subType;
    public GameObject plantPrefab; // prefab à instancier après Seed
    public PlantImages images;
}

[CreateAssetMenu(fileName = "GameValue", menuName = "Scriptable Objects/GameValue")]
public class GameValue : ScriptableObject
{
    public List<PlantSubTypeTimers> allPlantTimers = new List<PlantSubTypeTimers>()
    {
        new PlantSubTypeTimers { subType = PlantSubType.GoldenApple, timers = new PlantTimers { timeNextState = 1, timeDried = 1, timeFlood = 1, timeDead = 1 } },
        new PlantSubTypeTimers { subType = PlantSubType.Apple, timers = new PlantTimers { timeNextState = 5, timeDried = 1, timeFlood = 6, timeDead = 6 } },
        new PlantSubTypeTimers { subType = PlantSubType.Cherry, timers = new PlantTimers { timeNextState = 5, timeDried = 1, timeFlood = 5, timeDead = 4 } },
        new PlantSubTypeTimers { subType = PlantSubType.Banana, timers = new PlantTimers { timeNextState = 5, timeDried = 1, timeFlood = 6, timeDead = 6 } },
        new PlantSubTypeTimers { subType = PlantSubType.Orange, timers = new PlantTimers { timeNextState = 4, timeDried = 1, timeFlood = 5, timeDead = 5 } },
        new PlantSubTypeTimers { subType = PlantSubType.Carrot, timers = new PlantTimers { timeNextState = 2, timeDried = 1, timeFlood = 2, timeDead = 2 } },
        new PlantSubTypeTimers { subType = PlantSubType.Lettuce, timers = new PlantTimers { timeNextState = 1, timeDried = 1, timeFlood = 2, timeDead = 1 } },
        new PlantSubTypeTimers { subType = PlantSubType.Cucumber, timers = new PlantTimers { timeNextState = 2, timeDried = 1, timeFlood = 3, timeDead = 2 } },
        new PlantSubTypeTimers { subType = PlantSubType.Broccoli, timers = new PlantTimers { timeNextState = 3, timeDried = 1, timeFlood = 2, timeDead = 2 } },
        new PlantSubTypeTimers { subType = PlantSubType.GoldenCarrot, timers = new PlantTimers { timeNextState = 7, timeDried = 1, timeFlood = 1, timeDead = 1 } }
    };

    public List<PlantPrefab> allPlantPrefabs = new List<PlantPrefab>();

    public PlantTimers GetTimers(PlantSubType subtype)
    {
        return allPlantTimers.Find(p => p.subType == subtype)?.timers;
    }

    public GameObject GetPlantPrefab(PlantSubType subtype)
    {
        return allPlantPrefabs.Find(p => p.subType == subtype)?.plantPrefab;
    }

    public PlantImages GetImages(PlantSubType subtype)
    {
        return allPlantPrefabs.Find(p => p.subType == subtype)?.images;
    }
}