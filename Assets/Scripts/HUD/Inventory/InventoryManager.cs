using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryManager : MonoBehaviour
{
    public Action OnInventoryChanged;

    [System.Serializable]
    public class SlotData
    {
        public SlotType slotType;
        public PlantSubType subType;
        public int count;
    }

    public List<SlotData> slots = new List<SlotData>();
    
    public List<InventoryUI> inventoryUIs = new List<InventoryUI>();

    public int GetCount(SlotType slotType, PlantSubType subType)
    {
        var slot = slots.Find(s => s.slotType == slotType && s.subType == subType);
        return slot?.count ?? 0;
    }

    public void Add(SlotType slotType, PlantSubType subType, int amount)
    {
        var slot = slots.Find(s => s.slotType == slotType && s.subType == subType);
        if (slot != null) slot.count += amount;
        else slots.Add(new SlotData { slotType = slotType, subType = subType, count = amount });

        RefreshAllUIs();
    }

    public bool Consume(SlotType slotType, PlantSubType subType, int amount)
    {
        var slot = slots.Find(s => s.slotType == slotType && s.subType == subType);
        if (slot == null || slot.count < amount) return false;

        slot.count -= amount;
        RefreshAllUIs();
        return true;
    }

    private void RefreshAllUIs()
    {
        foreach (var ui in inventoryUIs)
            if (ui) ui.RefreshUI();
    }
}