using UnityEngine;

public class SunController : MonoBehaviour
{
    public Light sunLight;
    [Range(0f, 1.5f)] public float maxIntensity = 1.2f;
    [SerializeField] private float dawnDurationHours = 0.5f; 
    [SerializeField] private float duskDurationHours = 0.5f; 
    [SerializeField] private float intensityLerpSpeed = 5f; 

    private float currentIntensity; 

    void Start()
    {
        if (sunLight != null)
            currentIntensity = sunLight.intensity;
        else
            Debug.LogError("SunLight non assigné dans SunController !");
    }

    void Update()
    {
        if (sunLight == null) return;

        float currentTime = GameTimeManager.Instance.SmoothCurrentTime; 
        bool isDay = GameTimeManager.Instance.IsDay;
        
        float dayStartHour = 6f;
        float dayEndHour = 21f;
        float dawnStartHour = dayStartHour - dawnDurationHours; 
        float duskEndHour = dayEndHour + duskDurationHours; 
        float dayDurationHours = dayEndHour - dayStartHour; 
        float midDayHour = (dayStartHour + dayEndHour) / 2f; 

        float sunAngle = -90f; 

        if (currentTime >= dawnStartHour && currentTime < dayStartHour)
        {
            // Dawn: -90° à 0°
            float t = (currentTime - dawnStartHour) / dawnDurationHours;
            sunAngle = -90f + 90f * Mathf.SmoothStep(0f, 1f, t);
        }
        else if (currentTime >= dayStartHour && currentTime < dayEndHour)
        {
            float dayProgress = (currentTime - dayStartHour) / dayDurationHours; 
            sunAngle = Mathf.Sin(dayProgress * Mathf.PI) * 90f;
        }
        else if (currentTime >= dayEndHour && currentTime < duskEndHour)
        {
            float t = (currentTime - dayEndHour) / duskDurationHours;
            sunAngle = 0f - 90f * Mathf.SmoothStep(0f, 1f, t);
        }

        sunLight.transform.rotation = Quaternion.Euler(sunAngle, 0f, 0f);
        
        float targetIntensity = 0f;
        if (currentTime >= dawnStartHour && currentTime < dayStartHour)
        {
            float t = (currentTime - dawnStartHour) / dawnDurationHours;
            targetIntensity = Mathf.SmoothStep(0f, 0.5f * maxIntensity, t);
        }
        else if (currentTime >= dayStartHour && currentTime < midDayHour)
        {
            float t = (currentTime - dayStartHour) / (midDayHour - dayStartHour);
            targetIntensity = Mathf.SmoothStep(0.5f * maxIntensity, maxIntensity, t);
        }
        else if (currentTime >= midDayHour && currentTime < dayEndHour)
        {
            float t = (currentTime - midDayHour) / (dayEndHour - midDayHour);
            targetIntensity = Mathf.SmoothStep(maxIntensity, 0.5f * maxIntensity, t);
        }
        else if (currentTime >= dayEndHour && currentTime < duskEndHour)
        {
            float t = (currentTime - dayEndHour) / duskDurationHours;
            targetIntensity = Mathf.SmoothStep(0.5f * maxIntensity, 0f, t);
        }
        
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * intensityLerpSpeed);
        
        if (targetIntensity == 0f && currentIntensity < 1e-6f)
        {
            currentIntensity = 0f;
        }

        sunLight.intensity = currentIntensity;
        
    }
}