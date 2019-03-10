using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStatus
{
    public static bool isTalking;
    private static bool isPaused;

    public static bool IsPaused
    {
        get
        {
            return isPaused;
        }
    }

    public static void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    public static void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
    }
}
