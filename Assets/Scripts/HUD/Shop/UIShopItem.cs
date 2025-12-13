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
    public TextMeshProUGUI quantityText;

    private InventoryManager inventory;
    private Color normalColor;
    private Color hoverColor = new Color(0.8f, 1f, 0.8f);
    private bool isHovered;

    void Start()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");

        inventory = player.GetComponent<InteractManager>().inventoryManager;

        if (backgroundImage)
            normalColor = backgroundImage.color;

        var images = gameValue.GetImages(subType);
        if (images != null && slotImage != null)
            slotImage.sprite = images.seedSprite;

        if (priceText)
            priceText.text = price.ToString();

        UpdateQuantity();
    }

    void Update()
    {
        UpdateQuantity();
    }

    void UpdateQuantity()
    {
        if (!quantityText || inventory == null) return;
        int owned = inventory.GetCount(SlotType.Seed, subType);
        quantityText.text = owned.ToString();
    }

    public void OnPointerEnter(PointerEventData _)
    {
        isHovered = true;
        if (backgroundImage)
            backgroundImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData _)
    {
        isHovered = false;
        if (backgroundImage)
            backgroundImage.color = normalColor;
    }

    public void OnPointerClick(PointerEventData _)
    {
        inventory.Add(SlotType.Seed, subType, buyAmount);
        Debug.Log($"Achat : {subType} x{buyAmount}");
    }
}