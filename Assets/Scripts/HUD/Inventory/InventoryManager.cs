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

    private const string KEY_INVENTORY = "InventoryManager_Slots";

    void Awake()
    {
        Load();
    }

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
        Save();
    }

    public bool Consume(SlotType slotType, PlantSubType subType, int amount)
    {
        var slot = slots.Find(s => s.slotType == slotType && s.subType == subType);
        if (slot == null || slot.count < amount) return false;

        slot.count -= amount;
        RefreshAllUIs();
        Save();
        return true;
    }

    public void Remove(SlotType slotType, PlantSubType subType, int amount)
    {
        var slot = slots.Find(s => s.slotType == slotType && s.subType == subType);
        if (slot == null) return;

        slot.count -= amount;
        if (slot.count <= 0)
            slots.Remove(slot);

        RefreshAllUIs();
        Save();
    }

    private void RefreshAllUIs()
    {
        foreach (var ui in inventoryUIs)
            if (ui) ui.RefreshUI();

        OnInventoryChanged?.Invoke();
    }

    private void Save()
    {
        string json = JsonUtility.ToJson(new SerializationWrapper<SlotData>(slots));
        PlayerPrefs.SetString(KEY_INVENTORY, json);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        if (!PlayerPrefs.HasKey(KEY_INVENTORY)) return;

        string json = PlayerPrefs.GetString(KEY_INVENTORY);
        var wrapper = JsonUtility.FromJson<SerializationWrapper<SlotData>>(json);
        slots = wrapper.ToList();
        RefreshAllUIs();
    }

    [Serializable]
    private class SerializationWrapper<T>
    {
        public List<T> items;

        public SerializationWrapper(List<T> list)
        {
            items = list;
        }

        public List<T> ToList()
        {
            return items ?? new List<T>();
        }
    }
}
