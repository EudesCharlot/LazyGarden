using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameValue gameValue;
    public GameObject slotPrefab;
    public Transform contentParent;
    public InteractManager player;
    public SlotType inventoryType;
    public InventoryManager inventory;

    void Start() => RefreshUI();

    public void RefreshUI()
    {
        Debug.Log("RefreshUI");
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // Filtrer les slots correspondant au type sélectionné
        var list = inventory.slots.FindAll(s => s.slotType == inventoryType);

        foreach (var slot in list)
        {
            var newSlot = Instantiate(slotPrefab, contentParent);
            var uiItem = newSlot.GetComponent<UIItem>();
            uiItem.gameValue = gameValue;
            uiItem.subType = slot.subType;
            uiItem.slotType = inventoryType;
            uiItem.player = player.gameObject;
        }

    }
}