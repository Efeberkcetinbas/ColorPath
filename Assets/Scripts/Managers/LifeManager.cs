using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class LifeManager : MonoBehaviour
{
    [Header("DATA")]
    [SerializeField] private GameData gameData;

    [Header("Life")]
    [SerializeField] private Image lifeAmountProgress;
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI timerText,timerCounterText,timerFailText,timerFailCounterText; // Add a reference for the timer UI

    private DateTime lastLifeIncreaseTime;

    [Header("Life Regeneration Settings")]
    [SerializeField] private int checkIntervalInSeconds = 60; // Interval to check life regeneration (in seconds)
    [SerializeField] private int restoreIntervalInMinutes = 15; // Interval to restore life (in minutes)

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlayerDead, OnPlayerDead);
        EventManager.AddHandler(GameEvent.OnLifeFull, OnLifeFull);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlayerDead, OnPlayerDead);
        EventManager.RemoveHandler(GameEvent.OnLifeFull, OnLifeFull);
    }

    private void Start()
    {
        CheckStore();
        RestoreLifeOverTime();
        lifeText.SetText(gameData.lifeTime.ToString());
        UpdateLifeProgress();
        StartCoroutine(LifeRegenerationCoroutine());
    }

    private void CheckStore()
    {
        if (!PlayerPrefs.HasKey("LifeAmount"))
        {
            // Initialize default values
            gameData.lifeTime = 5;
            PlayerPrefs.SetInt("LifeAmount", gameData.lifeTime);
            lastLifeIncreaseTime = DateTime.Now;
            PlayerPrefs.SetString("LastLifeIncreaseTime", lastLifeIncreaseTime.ToString());
        }
        else
        {
            gameData.lifeTime = PlayerPrefs.GetInt("LifeAmount");
            lastLifeIncreaseTime = DateTime.Parse(PlayerPrefs.GetString("LastLifeIncreaseTime", DateTime.Now.ToString()));
        }
    }

    private void RestoreLifeOverTime()
    {
        TimeSpan timeElapsed = DateTime.Now - lastLifeIncreaseTime;
        int minutesPassed = (int)timeElapsed.TotalMinutes;
        int livesToRestore = minutesPassed / restoreIntervalInMinutes;

        if (livesToRestore > 0 && gameData.lifeTime < 5)
        {
            gameData.lifeTime += Mathf.Min(livesToRestore, 5 - gameData.lifeTime);
            lastLifeIncreaseTime = DateTime.Now.AddMinutes(-(minutesPassed % restoreIntervalInMinutes));
            SaveLife();
        }
    }

    private void OnPlayerDead()
    {
        if (gameData.lifeTime > 0) // Prevent life count from going negative
        {
            gameData.lifeTime--;
            UpdateLifeProgress();
            UpdateTimer();
            SaveLife();

        }
    }

    private void UpdateLifeProgress()
    {
        float progress = (float)gameData.lifeTime / 5;
        lifeAmountProgress.DOFillAmount(progress, 0.5f);
        lifeText.SetText(gameData.lifeTime.ToString());
    }

    private void SaveLife()
    {
        PlayerPrefs.SetInt("LifeAmount", gameData.lifeTime);
        PlayerPrefs.SetString("LastLifeIncreaseTime", lastLifeIncreaseTime.ToString());
    }

    private IEnumerator LifeRegenerationCoroutine()
    {
        while (true)
        {
            UpdateTimer(); // Update the timer UI every check interval

            yield return new WaitForSecondsRealtime(checkIntervalInSeconds); // Wait for the specified check interval

            // Recalculate the time elapsed to handle cases where the game is not running
            TimeSpan timeElapsed = DateTime.Now - lastLifeIncreaseTime;
            int minutesPassed = (int)timeElapsed.TotalMinutes;
            int livesToRestore = minutesPassed / restoreIntervalInMinutes;

            if (livesToRestore > 0 && gameData.lifeTime < 5)
            {
                gameData.lifeTime += Mathf.Min(livesToRestore, 5 - gameData.lifeTime);
                lastLifeIncreaseTime = DateTime.Now.AddMinutes(-(minutesPassed % restoreIntervalInMinutes));
                UpdateLifeProgress();
                SaveLife();
                EventManager.Broadcast(GameEvent.OnUpdateLife);
            }

            

        }
    }

    private void OnLifeFull()
    {
        gameData.lifeTime = 5; // Set the life amount to maximum
        UpdateLifeProgress(); // Update the UI to reflect the change
        SaveLife(); // Save the updated life amount to PlayerPrefs
    }

    private void UpdateTimer()
    {
        if (gameData.lifeTime >= 5)
        {
            timerText.SetText("Full"); // Display "Full" when lifeTime is 5
            timerFailText.SetText("Full"); // Display "Full" in the fail timer as well
        }
        else
        {
            TimeSpan timeUntilNextLife = GetTimeUntilNextLife();
            timerText.SetText(timeUntilNextLife.ToString(@"hh\:mm")); // Display in hh:mm format
            timerFailText.SetText(timeUntilNextLife.ToString(@"hh\:mm")); // Display in hh:mm format
        }
        timerCounterText.SetText(gameData.lifeTime.ToString()); // Update life count
        timerFailCounterText.SetText(gameData.lifeTime.ToString()); // Update life count in fail text
    }

    private TimeSpan GetTimeUntilNextLife()
    {
        // Calculate the time until the next life restoration
        TimeSpan timeElapsed = DateTime.Now - lastLifeIncreaseTime;
        int minutesPassed = (int)timeElapsed.TotalMinutes;
        int nextLifeRestoreTime = ((minutesPassed / restoreIntervalInMinutes) + 1) * restoreIntervalInMinutes;
        int minutesUntilNextLife = nextLifeRestoreTime - minutesPassed;
        return TimeSpan.FromMinutes(minutesUntilNextLife);
    }
}
