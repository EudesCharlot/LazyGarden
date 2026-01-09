using UnityEngine;

public class giveMoney : MonoBehaviour
{
    public PlayerMoney playerMoney;
    public int amount = 1000;

    private string buffer = "";

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (char.IsLetter(c))
                buffer += char.ToLower(c);

            if (buffer.Length > 4)
                buffer = buffer.Substring(buffer.Length - 4);

            if (buffer == "give")
            {
                playerMoney.AddMoney(amount);
                buffer = "";
            }
        }
    }
}