using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameValue gameValue;
    public PlantSubType subType;
    public SlotType slotType = SlotType.Seed;
    public GameObject player;

    public int buyAmount = 1;
    public int price;

    public Image backgroundImage;
    public Image slotImage;
    public TextMeshProUGUI priceText;

    private Color normalColor;
    private Color hoverColor = new Color(0.8f, 1f, 0.8f);

    void Start()
    {
        if (backgroundImage)
            normalColor = backgroundImage.color;
        
        var images = gameValue.GetImages(subType);
        if (images != null && slotImage != null)
            slotImage.sprite = images.seedSprite;
        
        if (priceText)
            priceText.text = price.ToString();
        
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");
    }
    
    public void OnPointerEnter(PointerEventData _)
    {
        if (backgroundImage)
            backgroundImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData _)
    {
        if (backgroundImage)
            backgroundImage.color = normalColor;
    }
    
    public void OnPointerClick(PointerEventData _)
    {
        if (!player) return;

        var inventory = player.GetComponent<InteractManager>().inventoryManager;
        if (inventory == null) return;
    
        var playerMoney = player.GetComponent<PlayerMoney>();
        if (playerMoney != null && playerMoney.CanAfford(price))
        {
            playerMoney.SpendMoney(price); 
            inventory.Add(slotType, subType, buyAmount); 
            Debug.Log($"Achat : {subType} x{buyAmount}, Prix : {price}, Argent restant : {playerMoney.GetMoney()}");
        }
    }
}