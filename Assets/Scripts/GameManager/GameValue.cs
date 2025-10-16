using UnityEngine;
using System.Collections.Generic;
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
    // Fruits
    Tomatoes,
    Apple,
    Strawberry,
    Banana,
    Orange,

    // Légumes
    Carrot,
    Lettuce,
    Cucumber,
    Broccoli,
    Spinach
}
[System.Serializable]
public class PlantTimers
{
    public float timeNextState;  // Temps pour passer à l'état suivant 
    public float timeDried;      // Temps avant de devenir Dried
    public float timeFlood;      // nb avant Flood si trop arrosé
    public float timeDead;       // Temps avant Dead après Dried
}

[System.Serializable]
public class PlantSubTypeTimers
{
    public PlantSubType subType;
    public PlantTimers timers;
}

[CreateAssetMenu(fileName = "GameValue", menuName = "Scriptable Objects/GameValue")]
public class GameValue : ScriptableObject
{
    public List<PlantSubTypeTimers> allPlantTimers = new List<PlantSubTypeTimers>()
    {
        // Fruits
        new PlantSubTypeTimers { subType = PlantSubType.Tomatoes, timers = new PlantTimers { timeNextState = 2, timeDried = 1, timeFlood = 4, timeDead = 5 } },
        new PlantSubTypeTimers { subType = PlantSubType.Apple, timers = new PlantTimers { timeNextState = 5, timeDried = 1, timeFlood = 6, timeDead = 6 } },
        new PlantSubTypeTimers { subType = PlantSubType.Strawberry, timers = new PlantTimers { timeNextState = 5, timeDried = 1, timeFlood = 5, timeDead = 4 } },
        new PlantSubTypeTimers { subType = PlantSubType.Banana, timers = new PlantTimers { timeNextState = 5, timeDried = 1, timeFlood = 6, timeDead = 6 } },
        new PlantSubTypeTimers { subType = PlantSubType.Orange, timers = new PlantTimers { timeNextState = 4, timeDried = 1, timeFlood = 5, timeDead = 5 } },

        // Légumes
        new PlantSubTypeTimers { subType = PlantSubType.Carrot, timers = new PlantTimers { timeNextState = 2, timeDried = 1, timeFlood = 2, timeDead = 2 } },
        new PlantSubTypeTimers { subType = PlantSubType.Lettuce, timers = new PlantTimers { timeNextState = 1, timeDried = 1, timeFlood = 2, timeDead = 1 } },
        new PlantSubTypeTimers { subType = PlantSubType.Cucumber, timers = new PlantTimers { timeNextState = 2, timeDried = 1, timeFlood = 3, timeDead = 2 } },
        new PlantSubTypeTimers { subType = PlantSubType.Broccoli, timers = new PlantTimers { timeNextState = 3, timeDried = 1, timeFlood = 2, timeDead = 2 } },
        new PlantSubTypeTimers { subType = PlantSubType.Spinach, timers = new PlantTimers { timeNextState = 1, timeDried = 1, timeFlood = 2, timeDead = 1 } }
    };

    
    public PlantTimers GetTimers(PlantSubType subtype)
    {
        return allPlantTimers.Find(p => p.subType == subtype)?.timers;
    }
}

