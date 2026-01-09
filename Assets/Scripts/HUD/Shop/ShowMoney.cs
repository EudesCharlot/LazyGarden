using TMPro;
using UnityEngine;

public class ShowMoney : MonoBehaviour
{
    public PlayerMoney playerMoney;     
    private TextMeshProUGUI moneyText;    

    void Awake()
    {
        moneyText = GetComponent<TextMeshProUGUI>();

        if (playerMoney == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerMoney = player.GetComponent<PlayerMoney>();
        }
    }

    void Update()
    {
        if (moneyText != null && playerMoney != null)
        {
            moneyText.text = playerMoney.GetMoney().ToString();
        }
    }
}