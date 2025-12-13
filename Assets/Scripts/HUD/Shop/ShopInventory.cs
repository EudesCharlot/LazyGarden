using UnityEngine;

public class ShopInventory : MonoBehaviour
{
    [Header("Setup Shop")]
    public GameValue gameValue; 
    public GameObject slotPrefab;     
    public Transform contentParent;    
    public GameObject player;         
    public PlantSubType[] seedsForSale;
    public int defaultBuyAmount = 1;   

    void Start()
    {
        GenerateShopSlots();
    }

    void GenerateShopSlots()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var seed in seedsForSale)
        {
            GameObject slotGO = Instantiate(slotPrefab, contentParent);
            
            var uiItem = slotGO.GetComponent<UIItem>();
            if (uiItem) Destroy(uiItem);
            
            var shopItem = slotGO.AddComponent<UIShopItem>();
            shopItem.gameValue = gameValue;
            shopItem.subType = seed;
            shopItem.player = player;
            shopItem.slotType = SlotType.Seed;
            shopItem.buyAmount = defaultBuyAmount;
            shopItem.price = GetPrice(seed);
        }
    }

    int GetPrice(PlantSubType subType)
    {
        return subType switch
        {
            PlantSubType.GoldenApple => 50,
            PlantSubType.GoldenCarrot => 40,
            PlantSubType.Apple => 15,
            PlantSubType.Banana => 20,
            PlantSubType.Orange => 20,
            PlantSubType.Carrot => 10,
            PlantSubType.Corn => 10,
            PlantSubType.Eggplant => 15,
            _ => 10
        };
    }
}
