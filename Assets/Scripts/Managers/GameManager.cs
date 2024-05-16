using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameData gameData;
    public PlayerData playerData;
    public PathData pathData;

    [SerializeField] private GameObject FailPanel;
    [SerializeField] private Ease ease;
    

    public float InitialDifficultyValue;


    private void Awake() 
    {
        ClearData();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlayerDead,OnPlayerDead);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlayerDead,OnPlayerDead);

    }

    
    void OnGameOver()
    {
        FailPanel.SetActive(true);
        FailPanel.transform.DOScale(Vector3.one,1f).SetEase(ease);
        gameData.isGameEnd=true;

    }

   
    private void OnPlayerDead()
    {
        gameData.isGameEnd=true;
        pathData.playersCanMove=false;
    }

    void ClearData()
    {
        gameData.isGameEnd=false;
        pathData.playersCanMove=false;
        playerData.successPathCompletedCounter=0;
        playerData.pathCompletedCounter=0;
    }

    
}
