using UnityEngine;

public class SellAll : MonoBehaviour
{
    public GameObject player;
    public PlayerMoney playerMoney;  
    public InventoryManager inventory;
    public GameValue gameValue;     

    public int GetPrice(PlantSubType subType)
    {
        return subType switch
        {
            PlantSubType.GoldenCarrot => 900,
            PlantSubType.Carrot => 30,
            PlantSubType.Corn => 15,
            PlantSubType.Eggplant => 25,
            _ => 5
        };
    }

    public void SellAllPlants()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (playerMoney == null)
            playerMoney = player.GetComponent<PlayerMoney>();

        if (inventory == null)
            inventory = player.GetComponent<InteractManager>().inventoryManager;

        int totalGain = 0;
        
        foreach (PlantSubType subType in System.Enum.GetValues(typeof(PlantSubType)))
        {
            if (subType == PlantSubType.Null) continue; 
            int count = inventory.GetCount(SlotType.Plant, subType); 
            if (count > 0)
            {
                totalGain += count * GetPrice(subType);
                inventory.Remove(SlotType.Plant, subType, count); 
            }
        }
        
        playerMoney.AddMoney(totalGain);

        Debug.Log($"Vente de toutes les plantes : +{totalGain} coins");
    }
}