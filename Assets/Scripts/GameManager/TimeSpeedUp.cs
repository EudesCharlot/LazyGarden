using UnityEngine;

public class TimeSpeedUp : MonoBehaviour
{
    public GameTimeManager gameTimeManager;
    public float speedUpFactor = 100f;

    private bool isSpeedUp = false;
    

    public void ToggleSpeed()
    {
        if (gameTimeManager == null) return;

        if (!isSpeedUp)
        {
            gameTimeManager.speedMultiplier = speedUpFactor;
            isSpeedUp = true;
        }
        else
        {
            gameTimeManager.speedMultiplier = 1f;
            isSpeedUp = false;
        }
    }
}