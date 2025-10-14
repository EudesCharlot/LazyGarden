using UnityEngine;
using System;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance { get; private set; }

    [Header("Durée du cycle complet")]
    public float dayDuration = 300f;   // Durée réelle pour la période de jour (6h-21h, 15h jeu)
    public float nightDuration = 180f; // Durée réelle pour la période de nuit (21h-6h, 9h jeu)

    [Header("Heure de début")]
    [Range(0, 24)] public int startHour = 6;
    [Range(0, 59)] public int startMinute = 0;

    public int CurrentHour { get; private set; }
    public int CurrentMinute { get; private set; }
    public bool IsDay => CurrentTime >= 6f && CurrentTime < 21f;

    public event Action<bool> OnDayNightChanged;
    private bool lastIsDay;

    private float accumulatedTime; // Accumule le temps réel pour progression (en minutes jeu fractionnelles)
    private float totalGameMinutes; // Total de minutes jeu entières écoulées depuis le début
    private float currentTimeSpeed = 1f; // Vitesse actuelle (minutes jeu par seconde réelle)

    public float CurrentTime => totalGameMinutes / 60f % 24f; // Heure fractionnelle (0-24) basée sur minutes entières
    public float SmoothGameMinutes => totalGameMinutes + accumulatedTime; // Minutes jeu smooth pour animations
    public float SmoothCurrentTime => SmoothGameMinutes / 60f % 24f; // Heure fractionnelle smooth

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        totalGameMinutes = startHour * 60f + startMinute; // Initialisation
        UpdateHourMinute();
        lastIsDay = IsDay;
        UpdateTimeSpeed(); // Initialiser la vitesse
    }

    void Update()
    {
        accumulatedTime += Time.deltaTime * currentTimeSpeed;

        int minutesToAdd = Mathf.FloorToInt(accumulatedTime);
        if (minutesToAdd > 0)
        {
            totalGameMinutes += minutesToAdd;
            accumulatedTime -= minutesToAdd;
            UpdateHourMinute();
            UpdateTimeSpeed(); // Vérifier si on change de période

            if (IsDay != lastIsDay)
            {
                OnDayNightChanged?.Invoke(IsDay);
                lastIsDay = IsDay;
            }
        }
    }

    private void UpdateHourMinute()
    {
        float totalHours = totalGameMinutes / 60f;
        CurrentHour = Mathf.FloorToInt(totalHours) % 24;
        CurrentMinute = Mathf.FloorToInt(totalGameMinutes % 60f);
    }

    private void UpdateTimeSpeed()
    {
        float gameDayHours = 15f; // 6h à 21h
        float gameNightHours = 9f; // 21h à 6h
        currentTimeSpeed = IsDay ? (gameDayHours * 60f / dayDuration) : (gameNightHours * 60f / nightDuration);
    }

    public float GetNormalizedTime()
    {
        return (totalGameMinutes % (24f * 60f)) / (24f * 60f);
    }
}