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
    public float timeFlood;      // Temps avant Flood si trop arrosé
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
        new PlantSubTypeTimers { subType = PlantSubType.Tomatoes, timers = new PlantTimers { timeNextState = 60f, timeDried = 120f, timeFlood = 30f, timeDead = 180f } },
        new PlantSubTypeTimers { subType = PlantSubType.Apple, timers = new PlantTimers { timeNextState = 120f, timeDried = 240f, timeFlood = 60f, timeDead = 300f } },
        new PlantSubTypeTimers { subType = PlantSubType.Strawberry, timers = new PlantTimers { timeNextState = 45f, timeDried = 90f, timeFlood = 20f, timeDead = 120f } },
        new PlantSubTypeTimers { subType = PlantSubType.Banana, timers = new PlantTimers { timeNextState = 150f, timeDried = 300f, timeFlood = 60f, timeDead = 360f } },
        new PlantSubTypeTimers { subType = PlantSubType.Orange, timers = new PlantTimers { timeNextState = 130f, timeDried = 260f, timeFlood = 50f, timeDead = 320f } },

        // Légumes
        new PlantSubTypeTimers { subType = PlantSubType.Carrot, timers = new PlantTimers { timeNextState = 50f, timeDried = 100f, timeFlood = 25f, timeDead = 150f } },
        new PlantSubTypeTimers { subType = PlantSubType.Lettuce, timers = new PlantTimers { timeNextState = 40f, timeDried = 80f, timeFlood = 20f, timeDead = 120f } },
        new PlantSubTypeTimers { subType = PlantSubType.Cucumber, timers = new PlantTimers { timeNextState = 70f, timeDried = 140f, timeFlood = 30f, timeDead = 200f } },
        new PlantSubTypeTimers { subType = PlantSubType.Broccoli, timers = new PlantTimers { timeNextState = 90f, timeDried = 180f, timeFlood = 40f, timeDead = 240f } },
        new PlantSubTypeTimers { subType = PlantSubType.Spinach, timers = new PlantTimers { timeNextState = 35f, timeDried = 70f, timeFlood = 15f, timeDead = 100f } }
    };
    
    public PlantTimers GetTimers(PlantSubType subtype)
    {
        return allPlantTimers.Find(p => p.subType == subtype)?.timers;
    }
}

