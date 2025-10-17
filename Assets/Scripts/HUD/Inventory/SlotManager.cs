using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
enum SlotType
{
    Seed,
    Plant
}
public class SlotManager : MonoBehaviour
{
    private int stack;
    private SlotType slotType;
    private PlantSubType subType;
    private PlantImages sprites;
    public GameValue gameValue;
    void Start()
    {
        sprites = gameValue.GetImages(subType);
    }
    
    void Update()
    {
        
    }
}
