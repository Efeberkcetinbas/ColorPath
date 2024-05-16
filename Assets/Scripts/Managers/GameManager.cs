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

    

    
    void OnGameOver()
    {
        FailPanel.SetActive(true);
        FailPanel.transform.DOScale(Vector3.one,1f).SetEase(ease);
        gameData.isGameEnd=true;

    }

   
    
    void ClearData()
    {
        gameData.isGameEnd=false;
        pathData.playersCanMove=false;
        playerData.successPathCompletedCounter=0;
        playerData.pathCompletedCounter=0;
    }

    
}
