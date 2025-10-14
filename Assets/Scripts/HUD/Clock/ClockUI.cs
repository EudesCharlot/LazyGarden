using UnityEngine;

public class ClockUI : MonoBehaviour
{
    public Transform clockHourHandTransform;
    public Transform clockMinuteHandTransform;

    void Update()
    {
        float elapsedMinutes = GameTimeManager.Instance.SmoothGameMinutes; // Smooth et continuous (renommé mais même chose)

        // --- Heures: 0.5° par minute (30° par heure / 60) ---
        float hourRotation = -elapsedMinutes * 0.5f % 360f;
        clockHourHandTransform.eulerAngles = new Vector3(0, 0, hourRotation);

        // --- Minutes: 6° par minute ---
        float minuteRotation = -elapsedMinutes * 6f % 360f;
        clockMinuteHandTransform.eulerAngles = new Vector3(0, 0, minuteRotation);
    }
}