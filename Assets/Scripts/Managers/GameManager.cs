using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameData gameData;
    public PlayerData playerData;
    public PathData pathData;

    [SerializeField] private Ease ease;
    

    public float InitialDifficultyValue;

    private WaitForSeconds waitForSeconds;


    private void Awake() 
    {
        ClearData(true);
    }

    private void Start()
    {
        waitForSeconds=new WaitForSeconds(2);
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

    
    

    private void OnNextLevel()
    {
        ClearData(false);
        
    }

    private void OnPlayerPathComplete()
    {
        if(playerData.pathCompletedCounter==playerData.numberOfPlayers)
        {
            if(playerData.successPathCompletedCounter==playerData.numberOfPlayers)
            {
                Debug.Log("PERFECT. CONG");
                EventManager.Broadcast(GameEvent.OnSuccess);
                StartCoroutine(OpenSuccess());
                //Particle and Success Panel
                //Dotween ile duzenlenecek
                //SuccessPanel.SetActive(true);
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

    private IEnumerator OpenSuccess()
    {
        yield return waitForSeconds;
        OpenSuccessPanel();
    }


    private void OpenSuccessPanel()
    {
        //effektif
        EventManager.Broadcast(GameEvent.OnSuccessUI);
    }

    
}
