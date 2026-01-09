using System;
using UnityEngine;

public class ShowPauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject darkBackground;
    public KeyCode pauseKey = KeyCode.Escape;

    private bool isPaused;

    private void Start()
    {
        isPaused = false;
        pauseMenu.SetActive(isPaused);
        darkBackground.SetActive(isPaused);
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
            TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (pauseMenu)
            pauseMenu.SetActive(isPaused);

        if (darkBackground)
            darkBackground.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void Resume()
    {
        if (!isPaused) return;
        TogglePause();
    }
}