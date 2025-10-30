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

    private InteractManager interactManager;
    private Image background;
    private PlantImages plantImages;
    private TextMeshProUGUI counterText;

    private Color normalColor = Color.white;
    private Color hoverColor = new Color(0.8f, 0.8f, 1f);
    private Color clickColor = new Color(0.6f, 0.6f, 1f);

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        interactManager = player.GetComponent<InteractManager>();
        background = GetComponentInChildren<Image>();
        counterText = GetComponentInChildren<TextMeshProUGUI>();

        plantImages = gameValue.GetImages(subType);

        if (plantImages != null)
        {
            background.sprite = slotType == SlotType.Seed
                ? plantImages.seedSprite
                : plantImages.plantSprite;
        }

        UpdateCountUI();
    }

    void Update() => UpdateCountUI();

    private void UpdateCountUI()
    {
        if (counterText == null || interactManager == null) return;
        int count = interactManager.inventoryManager.GetCount(slotType, subType);
        counterText.text = count.ToString();
    }

    public void OnPointerEnter(PointerEventData _) => background.color = hoverColor;
    public void OnPointerExit(PointerEventData _) => background.color = normalColor;

    public void OnPointerClick(PointerEventData _)
    {
        background.color = clickColor;

        if (slotType == SlotType.Seed)
        {
            interactManager.currentSeedSelector = subType;
            Debug.Log($"{subType} sélectionnée !");
        }
        else
        {
            Debug.Log($"{slotType} {subType} cliqué mais non sélectionnable pour interaction.");
        }
    }
}
