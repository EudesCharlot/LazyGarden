using UnityEngine;
using System;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance { get; private set; }

    [Header("Durée du cycle complet")]
    public float dayDuration = 300f;   
    public float nightDuration = 300f; 

    [Header("Heure de début")]
    [Range(0, 24)] public int startHour = 6;
    [Range(0, 59)] public int startMinute = 0;

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

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        totalGameMinutes = startHour * 60f + startMinute; 
        UpdateHourMinute();
        lastIsDay = IsDay;
        UpdateTimeSpeed();
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
            UpdateTimeSpeed(); 

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
            Debug.Log("Jour " + dayCounter);
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

    public int getDayCounter()
    {
        return dayCounter;
    }
}
