using UnityEngine;

public class ClockUI : MonoBehaviour
{
    public Transform clockHourHandTransform;
    public Transform clockMinuteHandTransform;

    void Update()
    {
        float elapsedMinutes = GameTimeManager.Instance.SmoothGameMinutes; 
        
        float hourRotation = -elapsedMinutes * 0.5f % 360f;
        clockHourHandTransform.eulerAngles = new Vector3(0, 0, hourRotation);
        
        float minuteRotation = -elapsedMinutes * 6f % 360f;
        clockMinuteHandTransform.eulerAngles = new Vector3(0, 0, minuteRotation);
    }
}