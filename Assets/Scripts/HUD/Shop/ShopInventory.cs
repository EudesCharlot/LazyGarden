using UnityEngine;

public class ShopInventory : MonoBehaviour
{
    public GameValue gameValue; 
    public GameObject slotPrefab;     
    public Transform contentParent;    
    public GameObject player;         
    public PlantSubType[] seedsForSale;
    public int defaultBuyAmount = 1;   

    private const string KEY_SHOP_PREFIX = "ShopInventory_";

    void Start()
    {
        LoadShop();
        GenerateShopSlots();
    }

    void GenerateShopSlots()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var seed in seedsForSale)
        {
            var slotGO = Instantiate(slotPrefab, contentParent);

            var shopItem = slotGO.GetComponent<UIShopItem>();
            shopItem.gameValue = gameValue;
            shopItem.subType = seed;
            shopItem.player = player;
            shopItem.slotType = SlotType.Seed;
            shopItem.buyAmount = GetSavedBuyAmount(seed);
            shopItem.price = GetPrice(seed);

            var images = gameValue.GetImages(seed);
            if (images != null && images.seedSprite != null && shopItem.slotImage != null)
                shopItem.slotImage.sprite = images.seedSprite;
            else
                Debug.LogWarning($"Image manquante pour {seed}");
        }
    }

    int GetPrice(PlantSubType subType)
    {
        return subType switch
        {
            PlantSubType.GoldenCarrot => 300,
            PlantSubType.Carrot => 20,
            PlantSubType.Corn => 10,
            PlantSubType.Eggplant => 15,
            _ => 10
        };
    }

    void SaveBuyAmount(PlantSubType seed, int amount)
    {
        PlayerPrefs.SetInt(KEY_SHOP_PREFIX + seed.ToString(), amount);
        PlayerPrefs.Save();
    }

    int GetSavedBuyAmount(PlantSubType seed)
    {
        return PlayerPrefs.HasKey(KEY_SHOP_PREFIX + seed.ToString())
            ? PlayerPrefs.GetInt(KEY_SHOP_PREFIX + seed.ToString())
            : defaultBuyAmount;
    }

    public void SetBuyAmount(PlantSubType seed, int amount)
    {
        SaveBuyAmount(seed, amount);
    }

    void LoadShop()
    {
        foreach (var seed in seedsForSale)
            GetSavedBuyAmount(seed);
    }
}
