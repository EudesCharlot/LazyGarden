using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    [SerializeField] private int money = 0;

    void Awake()
    {
        LoadMoney();
    }

    public int GetMoney()
    {
        return money;
    }

    public void SetMoney(int amount)
    {
        money = Mathf.Max(0, amount);
        SaveMoney();
    }

    public void AddMoney(int amount)
    {
        money += Mathf.Max(0, amount);
        SaveMoney();
    }

    public bool SpendMoney(int amount)
    {
        if (amount <= 0) return false;

        if (money >= amount)
        {
            money -= amount;
            SaveMoney();
            return true;
        }

        return false;
    }

    public bool CanAfford(int amount)
    {
        return money >= amount;
    }

    private void SaveMoney()
    {
        PlayerPrefs.SetInt("playerMoney", money);
        PlayerPrefs.Save();
    }

    private void LoadMoney()
    {
        if (PlayerPrefs.HasKey("playerMoney"))
            money = PlayerPrefs.GetInt("playerMoney");
    }
}