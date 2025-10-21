using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameValue gameValue;
    public PlantSubType subType;
    public SlotType slotType; 
    public GameObject player;

    private SeedSelector seedSelector;
    private Image background;
    private PlantImages plantImages;

    private Color normalColor = Color.white;
    private Color hoverColor = new Color(0.8f, 0.8f, 1f);
    private Color clickColor = new Color(0.6f, 0.6f, 1f);

    void Start()
    {
        background = GetComponent<Image>();
        plantImages = gameValue.GetImages(subType);
        seedSelector = player.GetComponent<SeedSelector>();

        if (plantImages != null)
        {
            background.sprite = slotType == SlotType.Seed
                ? plantImages.seedSprite.sprite
                : plantImages.plantSprite.sprite;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        background.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        background.color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        background.color = clickColor;

        if (player != null)
        {
            seedSelector.SelectSeed(subType);
            Debug.Log($"{subType} sélectionné !");
        }
    }
}