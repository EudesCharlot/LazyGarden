using TMPro;
using UnityEngine;

public class ElectronicUI : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        string normalizedTime = GameTimeManager.Instance.GetTimeStringRounded();
        timerText.text = normalizedTime;
    }
}