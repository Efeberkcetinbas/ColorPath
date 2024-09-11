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
    [SerializeField] private TextMeshProUGUI timerText, startLifeText, timerFailText, failLifeText;

    private DateTime lastLifeDecreaseTime; // Track when the life was last decreased

    [Header("Life Regeneration Settings")]
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
        lifeText.SetText(gameData.lifeTime.ToString());
        UpdateLifeProgress();
    }

    private void Update()
    {
        if (gameData.lifeTime < 5)
        {
            TimeSpan timeUntilNextLife = GetTimeUntilNextLife();
            timerText.SetText(timeUntilNextLife.ToString(@"mm\:ss"));
            timerFailText.SetText(timeUntilNextLife.ToString(@"mm\:ss"));

            if (timeUntilNextLife.TotalSeconds <= 0)
            {
                RestoreLifeOverTime();
                UpdateLifeProgress();
                SaveLife();
            }
        }
        else
        {
            timerText.SetText("Full");
            timerFailText.SetText("Full");
        }

        startLifeText.SetText(gameData.lifeTime.ToString());
        failLifeText.SetText(gameData.lifeTime.ToString());
    }

    

    private void CheckStore()
    {
        if (!PlayerPrefs.HasKey("LifeAmount"))
        {
            gameData.lifeTime = 5;
            PlayerPrefs.SetInt("LifeAmount", gameData.lifeTime);
            lastLifeDecreaseTime = DateTime.Now;
            PlayerPrefs.SetString("LastLifeDecreaseTime", lastLifeDecreaseTime.ToString());
        }
        else
        {
            gameData.lifeTime = PlayerPrefs.GetInt("LifeAmount");
            lastLifeDecreaseTime = DateTime.Parse(PlayerPrefs.GetString("LastLifeDecreaseTime", DateTime.Now.ToString()));

            // Calculate how much time has passed since the last life decrease
            TimeSpan timeElapsedSinceLastLifeDecrease = DateTime.Now - lastLifeDecreaseTime;
            int minutesPassed = (int)timeElapsedSinceLastLifeDecrease.TotalMinutes;
            int livesToRestore = minutesPassed / restoreIntervalInMinutes;

            if (livesToRestore > 0 && gameData.lifeTime < 5)
            {
                gameData.lifeTime += Mathf.Min(livesToRestore, 5 - gameData.lifeTime);
                lastLifeDecreaseTime = lastLifeDecreaseTime.AddMinutes(livesToRestore * restoreIntervalInMinutes);
                SaveLife();
            }
        }
    }

    private void RestoreLifeOverTime()
    {
        TimeSpan timeElapsed = DateTime.Now - lastLifeDecreaseTime;
        int minutesPassed = (int)timeElapsed.TotalMinutes;
        int livesToRestore = minutesPassed / restoreIntervalInMinutes;

        if (livesToRestore > 0 && gameData.lifeTime < 5)
        {
            gameData.lifeTime += Mathf.Min(livesToRestore, 5 - gameData.lifeTime);
            lastLifeDecreaseTime = lastLifeDecreaseTime.AddMinutes(livesToRestore * restoreIntervalInMinutes);
            SaveLife();
        }
    }

    private void OnPlayerDead()
    {
        if (gameData.lifeTime > 0)
        {
            gameData.lifeTime--;
            UpdateLifeProgress();
            SaveLife();

            // Only start tracking the timer if life is below 5, but don't reset it to 15 minutes
            if (gameData.lifeTime < 5 && lastLifeDecreaseTime == DateTime.MinValue)
            {
                lastLifeDecreaseTime = DateTime.Now;
                SaveLife();
            }
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
        PlayerPrefs.SetString("LastLifeDecreaseTime", lastLifeDecreaseTime.ToString());
    }

    private TimeSpan GetTimeUntilNextLife()
    {
        TimeSpan timeElapsed = DateTime.Now - lastLifeDecreaseTime;
        int minutesPassed = (int)timeElapsed.TotalMinutes;
        int nextLifeRestoreTime = restoreIntervalInMinutes;
        int minutesUntilNextLife = nextLifeRestoreTime - minutesPassed;
        int secondsPassed = (int)timeElapsed.TotalSeconds % 60;
        return TimeSpan.FromMinutes(minutesUntilNextLife) - TimeSpan.FromSeconds(secondsPassed);
    }

    private void OnLifeFull()
    {
        gameData.lifeTime = 5;
        startLifeText.SetText(gameData.lifeTime.ToString());
        failLifeText.SetText(gameData.lifeTime.ToString());
        UpdateLifeProgress();
        SaveLife();
        EventManager.Broadcast(GameEvent.OnLifeFullUI);
    }
}
