using UnityEngine;

public class seedManager : MonoBehaviour
{
    public PlantState state;
    public PlantType plantType;
    public PlantSubType subType;
    
    public GameValue gameValue;
    
    private PlantTimers plantTimers;
    
    private GameTimeManager gameTimeManager = GameTimeManager.Instance;
    private float timeLastWaterd;
    private float timeLastState;
    void OnEnable()
    {
        state =  PlantState.Seed;
        timeLastWaterd = gameTimeManager.GetNormalizedTime();
        timeLastState = gameTimeManager.GetNormalizedTime();
        plantTimers = gameValue.GetTimers(subType);
    }
    
    void Update()
    {
        float time = gameTimeManager.GetNormalizedTime();
    }

    public void waterPlant()
    {
        timeLastWaterd = gameTimeManager.GetNormalizedTime();
        Debug.Log("Plante bien arrosé !");
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
        timeLastState = gameTimeManager.GetNormalizedTime();
    }

    void flooded()
    {
        state =  PlantState.Flood;
    }

    void dried()
    {
        state = PlantState.Dried;
    }

    void Dead()
    {
        state = PlantState.Dead;
    }
    
    
    
}
