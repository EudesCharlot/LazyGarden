using UnityEngine;

public class SeedSelector : MonoBehaviour
{
    public PlantSubType selectedSeed;

    public void SelectSeed(PlantSubType newSeed)
    {
        selectedSeed = newSeed;
        Debug.Log($"Nouvelle graine sélectionnée : {selectedSeed}");
    }
}