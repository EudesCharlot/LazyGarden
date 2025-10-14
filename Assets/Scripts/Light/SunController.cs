using UnityEngine;

public class SunController : MonoBehaviour
{
    public Light sunLight;
    [Range(0f, 1.5f)] public float maxIntensity = 1.2f;
    [SerializeField] private float dawnDurationHours = 0.5f; // Durée crépuscule matin (heures jeu)
    [SerializeField] private float duskDurationHours = 0.5f; // Durée crépuscule soir (heures jeu)
    [SerializeField] private float intensityLerpSpeed = 5f; // Vitesse d'interpolation douce

    private float currentIntensity; // Intensité actuelle lissée

    void Start()
    {
        if (sunLight != null)
            currentIntensity = sunLight.intensity; // Initialiser
        else
            Debug.LogError("SunLight non assigné dans SunController !");
    }

    void Update()
    {
        if (sunLight == null) return;

        float currentTime = GameTimeManager.Instance.SmoothCurrentTime; // Utiliser la version smooth pour fluidité
        bool isDay = GameTimeManager.Instance.IsDay;

        // --- Rotation du soleil avec transitions dawn/dusk ---
        float dayStartHour = 6f;
        float dayEndHour = 21f;
        float dawnStartHour = dayStartHour - dawnDurationHours; // ex: 5.5
        float duskEndHour = dayEndHour + duskDurationHours; // ex: 21.5
        float dayDurationHours = dayEndHour - dayStartHour; // 15h
        float midDayHour = (dayStartHour + dayEndHour) / 2f; // 13.5

        float sunAngle = -90f; // Par défaut sous l'horizon (nuit)

        if (currentTime >= dawnStartHour && currentTime < dayStartHour)
        {
            // Dawn: -90° à 0°
            float t = (currentTime - dawnStartHour) / dawnDurationHours;
            sunAngle = -90f + 90f * Mathf.SmoothStep(0f, 1f, t);
        }
        else if (currentTime >= dayStartHour && currentTime < dayEndHour)
        {
            // Jour: 0° à 90° à 0°
            float dayProgress = (currentTime - dayStartHour) / dayDurationHours; // 0 à 1
            sunAngle = Mathf.Sin(dayProgress * Mathf.PI) * 90f;
        }
        else if (currentTime >= dayEndHour && currentTime < duskEndHour)
        {
            // Dusk: 0° à -90°
            float t = (currentTime - dayEndHour) / duskDurationHours;
            sunAngle = 0f - 90f * Mathf.SmoothStep(0f, 1f, t);
        }
        // Sinon nuit: -90°

        sunLight.transform.rotation = Quaternion.Euler(sunAngle, 0f, 0f);

        // --- Intensité cible avec transitions continues ---
        float targetIntensity = 0f;
        if (currentTime >= dawnStartHour && currentTime < dayStartHour)
        {
            // Crépuscule matin: 0 à 0.5 * max
            float t = (currentTime - dawnStartHour) / dawnDurationHours;
            targetIntensity = Mathf.SmoothStep(0f, 0.5f * maxIntensity, t);
        }
        else if (currentTime >= dayStartHour && currentTime < midDayHour)
        {
            // Matin: 0.5 * max à max
            float t = (currentTime - dayStartHour) / (midDayHour - dayStartHour);
            targetIntensity = Mathf.SmoothStep(0.5f * maxIntensity, maxIntensity, t);
        }
        else if (currentTime >= midDayHour && currentTime < dayEndHour)
        {
            // Après-midi: max à 0.5 * max
            float t = (currentTime - midDayHour) / (dayEndHour - midDayHour);
            targetIntensity = Mathf.SmoothStep(maxIntensity, 0.5f * maxIntensity, t);
        }
        else if (currentTime >= dayEndHour && currentTime < duskEndHour)
        {
            // Crépuscule soir: 0.5 * max à 0
            float t = (currentTime - dayEndHour) / duskDurationHours;
            targetIntensity = Mathf.SmoothStep(0.5f * maxIntensity, 0f, t);
        }
        // Sinon (nuit complète): 0

        // --- Interpolation douce frame par frame ---
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * intensityLerpSpeed);

        // Snap à 0 si très proche pour éviter valeurs absurdes comme 1e-30
        if (targetIntensity == 0f && currentIntensity < 1e-6f)
        {
            currentIntensity = 0f;
        }

        sunLight.intensity = currentIntensity;

        // Debug
        Debug.Log($"Heure: {currentTime:F2}, Angle: {sunAngle:F2}°, Cible: {targetIntensity:F2}, Intensité: {currentIntensity:F2}, IsDay: {isDay}");
    }
}