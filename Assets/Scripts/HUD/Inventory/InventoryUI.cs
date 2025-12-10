using System;
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
        Debug.Log($"[InventoryUI] RefreshUI called. inventory.slots total = {inventory.slots.Count}");
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        var list = inventory.slots.FindAll(s => s.slotType == inventoryType);
        Debug.Log($"[InventoryUI] slots matching type {inventoryType}: {list.Count}");

        foreach (var slot in list)
        {
            Debug.Log($"  create UI for {slot.subType} x{slot.count}");
            var newSlot = Instantiate(slotPrefab, contentParent);
            var uiItem = newSlot.GetComponent<UIItem>();
            uiItem.gameValue = gameValue;
            uiItem.subType = slot.subType;
            uiItem.slotType = inventoryType;
            uiItem.player = player.gameObject;
            
            var qtyField = uiItem.GetType().GetField("quantity");
            if (qtyField != null) qtyField.SetValue(uiItem, slot.count);
        }
    }

}