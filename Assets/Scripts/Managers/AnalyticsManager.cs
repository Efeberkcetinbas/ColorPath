using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    
    private bool isInitialized=false;
    [SerializeField] private GameData gameData;

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        isInitialized=true;
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
    }

    private void OnNextLevel()
    {
        if(!isInitialized)
        {
            return;
        }

        CustomEvent myEvent=new CustomEvent("nextLevel")
        {
            {"Level",gameData.levelNumber}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        AnalyticsService.Instance.Flush();
    }

    private void OnRestartLevel()
    {
        AnalyticsService.Instance.RecordEvent("restartLevel");
        AnalyticsService.Instance.Flush();
    }
}
