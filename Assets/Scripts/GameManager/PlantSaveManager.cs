using System.Collections.Generic;
using UnityEngine;

public class PlantSaveManager : MonoBehaviour
{
    public GameObject seedPrefab;

    void Awake()
    {
        LoadAllPlants();
    }

    public void SaveAllPlants()
    {
        seedManager[] plants = FindObjectsOfType<seedManager>();

        PlayerPrefs.SetInt("plantCount", plants.Length);

        for (int i = 0; i < plants.Length; i++)
        {
            PlantData data = plants[i].GetSaveData();
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString($"plant_{i}", json);
        }

        PlayerPrefs.Save();
    }

    void LoadAllPlants()
    {
        int plantCount = PlayerPrefs.GetInt("plantCount", 0);

        for (int i = 0; i < plantCount; i++)
        {
            string json = PlayerPrefs.GetString($"plant_{i}", "");
            if (string.IsNullOrEmpty(json)) continue;

            PlantData data = JsonUtility.FromJson<PlantData>(json);

            Vector3 pos = new Vector3(data.posX, data.posY, data.posZ);
            GameObject plant = Instantiate(seedPrefab, pos, Quaternion.identity);

            seedManager manager = plant.GetComponent<seedManager>();
            manager.subType = data.subType;
            manager.SetSavedData(data);
        }
    }

    [System.Serializable]
    public class PlantData
    {
        public PlantSubType subType;
        public PlantState state;
        public int dayLastWatered;
        public int dayLastState;
        public int dayLastHealthyState;
        public int waterStreak;
        public float posX;
        public float posY;
        public float posZ;
    }
    
    void OnApplicationQuit()
    {
        SaveAllPlants();
    }

}