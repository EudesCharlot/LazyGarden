using UnityEngine;

public class seedManager : MonoBehaviour
{
    public PlantState state;
    public PlantType plantType;
    public PlantSubType subType;
    
    public GameValue gameValue;
    
    private PlantTimers plantTimers;
    
    private GameTimeManager gameTimeManager = GameTimeManager.Instance;
    private float dayLastWaterd;
    private float dayLastState;
    private int waterCount;
    
    void OnEnable()
    {
        state =  PlantState.Seed;
        dayLastWaterd = gameTimeManager.GetNormalizedTime();
        dayLastState = gameTimeManager.GetNormalizedTime();
        plantTimers = gameValue.GetTimers(subType);
        waterCount = 0;
    }
    
    void Update()
    {
        float time = gameTimeManager.GetNormalizedTime();
    }

    public void waterPlant()
    {
        dayLastWaterd = gameTimeManager.GetNormalizedTime();
        Debug.Log("Plante bien arrosé !");
        
        waterCount++;
        if (state == PlantState.Flood)
        {
            Dead();
            return;
        }
            
        if (waterCount == plantTimers.timeFlood)
            Flooded();
    }

    void nextState()
    {
        switch(state)
        {
            case PlantState.Seed:
                state = PlantState.Sprout;
                break;
            case PlantState.Sprout:
                state = PlantState.Seedling;
                break;
            case PlantState.Seedling:
                state = PlantState.Mature;
                break;
        }
        dayLastState = gameTimeManager.GetNormalizedTime();
    }

    void Flooded()
    {
        state =  PlantState.Flood;
    }

    void Dried()
    {
        state = PlantState.Dried;
    }

    void Dead()
    {
        state = PlantState.Dead;
    }
    
    
    
}
