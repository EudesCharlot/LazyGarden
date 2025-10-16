using UnityEngine;
using TMPro;

public class DayCounterUI : MonoBehaviour
{
    private TextMeshProUGUI counterText;

    void Start()
    {
        counterText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {

        int dayCounter = GameTimeManager.Instance.dayCounter;
        counterText.text = dayCounter.ToString() + " jours";
    }
}