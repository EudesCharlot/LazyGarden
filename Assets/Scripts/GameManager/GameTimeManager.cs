using System;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance { get; private set; }

    [Header("Durée du cycle complet")]
    public float dayDuration = 300f;
    public float nightDuration = 300f;

    [Header("Heure de début")]
    [Range(0, 24)] public int startHour = 6;
    [Range(0, 59)] public int startMinute = 0;

    [Header("Speed multiplier")]
    public float speedMultiplier = 1f;

    public int CurrentHour { get; private set; }
    public int CurrentMinute { get; private set; }
    public bool IsDay => CurrentTime >= 6f && CurrentTime < 21f;

    public event Action<bool> OnDayNightChanged;
    private bool lastIsDay;

    private float accumulatedTime;
    private float totalGameMinutes;
    private float currentTimeSpeed = 1f;

    public float CurrentTime => totalGameMinutes / 60f % 24f;
    public float SmoothGameMinutes => totalGameMinutes + accumulatedTime;
    public float SmoothCurrentTime => SmoothGameMinutes / 60f % 24f;

    public int dayCounter;
    private bool lastDayChecked = false;

    private const string KEY_TOTAL_MINUTES = "GTM_TotalMinutes";
    private const string KEY_DAY_COUNTER = "GTM_DayCounter";
    private const string KEY_SPEED_MULTIPLIER = "GTM_SpeedMultiplier";

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        totalGameMinutes = PlayerPrefs.GetFloat(KEY_TOTAL_MINUTES, startHour * 60f + startMinute);
        dayCounter = PlayerPrefs.GetInt(KEY_DAY_COUNTER, 0);
        speedMultiplier = PlayerPrefs.GetFloat(KEY_SPEED_MULTIPLIER, 1f);

        UpdateHourMinute();
        lastIsDay = IsDay;
        UpdateTimeSpeed();
    }

    void Update()
    {
        accumulatedTime += Time.deltaTime * currentTimeSpeed * speedMultiplier;

        int minutesToAdd = Mathf.FloorToInt(accumulatedTime);
        if (minutesToAdd > 0)
        {
            totalGameMinutes += minutesToAdd;
            accumulatedTime -= minutesToAdd;
            UpdateHourMinute();
            UpdateTimeSpeed();
            Save();

            if (IsDay != lastIsDay)
            {
                OnDayNightChanged?.Invoke(IsDay);
                lastIsDay = IsDay;
            }
        }

        if (CurrentHour == 0 && !lastDayChecked)
        {
            dayCounter++;
            lastDayChecked = true;
            Save();
        }
        else if (CurrentHour != 0)
        {
            lastDayChecked = false;
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
        float gameDayHours = 15f;
        float gameNightHours = 9f;
        currentTimeSpeed = IsDay ? (gameDayHours * 60f / dayDuration) : (gameNightHours * 60f / nightDuration);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(KEY_TOTAL_MINUTES, totalGameMinutes);
        PlayerPrefs.SetInt(KEY_DAY_COUNTER, dayCounter);
        PlayerPrefs.SetFloat(KEY_SPEED_MULTIPLIER, speedMultiplier);
        PlayerPrefs.Save();
    }

    public float GetNormalizedTime()
    {
        return (totalGameMinutes % (24f * 60f)) / (24f * 60f);
    }

    public string GetTimeString()
    {
        int hours = Mathf.FloorToInt(CurrentTime);
        int minutes = Mathf.FloorToInt((CurrentTime - hours) * 60f);
        return string.Format("{0:00}:{1:00}", hours, minutes);
    }

    public string GetTimeStringRounded()
    {
        int hours = Mathf.FloorToInt(CurrentTime);
        int minutes = Mathf.FloorToInt((CurrentTime - hours) * 60f);
        minutes = (minutes / 10) * 10;
        return string.Format("{0}:{1:00}", hours, minutes);
    }

    public int GetDayCounter()
    {
        return dayCounter;
    }
}
