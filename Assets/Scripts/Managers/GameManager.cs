using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameData gameData;
    public PlayerData playerData;
    public PathData pathData;

    [SerializeField] private GameObject FailPanel,SuccessPanel;
    [SerializeField] private Ease ease;
    

    public float InitialDifficultyValue;


    private void Awake() 
    {
        ClearData(true);
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlayerDead,OnPlayerDead);
        EventManager.AddHandler(GameEvent.OnPlayerPathComplete,OnPlayerPathComplete);
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlayerDead,OnPlayerDead);
        EventManager.RemoveHandler(GameEvent.OnPlayerPathComplete,OnPlayerPathComplete);
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);

    }

    
    private void OnGameOver()
    {
        FailPanel.SetActive(true);
        FailPanel.transform.DOScale(Vector3.one,1f).SetEase(ease);
        gameData.isGameEnd=true;

    }

    private void OnNextLevel()
    {
        SuccessPanel.SetActive(false);
        ClearData(false);
        
    }

    private void OnPlayerPathComplete()
    {
        if(playerData.pathCompletedCounter==playerData.numberOfPlayers)
        {
            if(playerData.successPathCompletedCounter==playerData.numberOfPlayers)
            {
                Debug.Log("PERFECT. CONG");
                //Dotween ile duzenlenecek
                SuccessPanel.SetActive(true);
            }

            else
            {
                Debug.Log("END FAIL");
            }
        }
        else
            return;
    }

   
    private void OnPlayerDead()
    {
        gameData.isGameEnd=true;
        pathData.playersCanMove=false;
    }

    void ClearData(bool val)
    {
        gameData.isGameEnd=val;
        pathData.playersCanMove=false;
        playerData.successPathCompletedCounter=0;
        playerData.pathCompletedCounter=0;
    }

    
}
