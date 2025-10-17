using UnityEngine;

public class SunController : MonoBehaviour
{
    public Light sunLight;
    [Range(0f, 1.5f)] public float maxIntensity = 1.2f;
    [SerializeField] private float rotationLerpSpeed = 5f;
    [SerializeField] private float intensityLerpSpeed = 5f;

    private float currentAngle = 0f;
    private float currentIntensity = 0f;
    private bool wasDayLastFrame = false;

    void Start()
    {
        if (sunLight == null)
        {
            Debug.LogError("SunLight non assigné dans SunController !");
            enabled = false; 
        }
    }

    void Update()
    {
        float normalizedTime = GameTimeManager.Instance.GetNormalizedTime(); 
        bool isDay = GameTimeManager.Instance.IsDay; 

        float targetAngle;
        float targetIntensity;

        if (isDay)
        {
            if (!wasDayLastFrame)
            {
                currentAngle = 0f; 
            }
            
            float dayProgress = Mathf.InverseLerp(0.25f, 0.875f, normalizedTime);
            targetAngle = Mathf.Lerp(0f, 180f, dayProgress);
            targetIntensity = maxIntensity;
        }
        else
        {
            targetAngle = 180f;
            targetIntensity = 0f;
        }
        
        wasDayLastFrame = isDay;
        
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * rotationLerpSpeed);
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * intensityLerpSpeed);
        
        sunLight.transform.rotation = Quaternion.Euler(currentAngle, 0f, 0f);
        sunLight.intensity = currentIntensity;
    }
}