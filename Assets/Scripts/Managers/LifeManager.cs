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

    private DateTime lastLifeIncreaseTime;

    [Header("Life Regeneration Settings")]
    [SerializeField] private int checkIntervalInSeconds = 60; // Interval to check life regeneration (in seconds)
    [SerializeField] private int restoreIntervalInMinutes = 15; // Interval to restore life (in minutes)

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlayerDead, OnPlayerDead);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlayerDead, OnPlayerDead);
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
            }
        }
    }
}
