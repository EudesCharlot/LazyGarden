using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameValue gameValue;
    public PlantSubType subType;
    public SlotType slotType;
    public GameObject player;

    [Header("UI References")]
    public Image backgroundImage;
    public Image slotImage; 
    public TextMeshProUGUI counterText;

    private InteractManager interactManager;
    private PlantImages plantImages;

    private Color normalColor;
    private Color hoverColor = new Color(0.8f, 0.8f, 1f);        
    private Color selectedColor = new Color(0.6f, 0.6f, 1f);       
    private Color hoverSelectedColor = new Color(0.7f, 0.7f, 1f);   

    private bool isHovered = false;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        interactManager = player.GetComponent<InteractManager>();

        if (backgroundImage != null)
            normalColor = backgroundImage.color;

        plantImages = gameValue.GetImages(subType);

        if (plantImages != null && slotImage != null)
        {
            slotImage.sprite = slotType == SlotType.Seed
                ? plantImages.seedSprite
                : plantImages.plantSprite;
        }

        UpdateCountUI();
        UpdateBackgroundColor();
    }

    void Update()
    {
        UpdateCountUI();
        UpdateBackgroundColor();
    }

    private void UpdateCountUI()
    {
        if (counterText == null || interactManager == null) return;
        int count = interactManager.inventoryManager.GetCount(slotType, subType);
        counterText.text = count.ToString();
    }

    private void UpdateBackgroundColor()
    {
        if (backgroundImage == null) return;

        if (interactManager.currentSeedSelector == subType)
        {
            backgroundImage.color = isHovered ? hoverSelectedColor : selectedColor;
        }
        else 
        {
            backgroundImage.color = isHovered ? hoverColor : normalColor;
        }
    }

    public void OnPointerEnter(PointerEventData _)
    {
        isHovered = true;
        UpdateBackgroundColor();
    }

    public void OnPointerExit(PointerEventData _)
    {
        isHovered = false;
        UpdateBackgroundColor();
    }

    public void OnPointerClick(PointerEventData _)
    {
        if (slotType == SlotType.Seed)
        {
            interactManager.currentSeedSelector = subType;
            Debug.Log($"{subType} sélectionnée !");
        }
        else
        {
            Debug.Log($"{slotType} {subType} cliqué mais non sélectionnable pour interaction.");
        }

        UpdateBackgroundColor();
    }
}
