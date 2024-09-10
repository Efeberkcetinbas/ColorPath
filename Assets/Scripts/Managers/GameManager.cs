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
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);

    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlayerDead,OnPlayerDead);
        EventManager.RemoveHandler(GameEvent.OnPlayerPathComplete,OnPlayerPathComplete);
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);

    }

    
    

    private void OnNextLevel()
    {
        ClearData(false);
        
    }

    private void OnRestartLevel()
    {
        ClearData(false);
        StartCoroutine(StartAgain());
    }


    private IEnumerator StartAgain()
    {
        gameData.isGameEnd=true;
        yield return new WaitForSeconds(1);
        gameData.isGameEnd=false;
    }

    private void OnPlayerPathComplete()
    {
        if(playerData.pathCompletedCounter==playerData.numberOfPlayers)
        {
            if(playerData.successPathCompletedCounter==playerData.numberOfPlayers)
            {
                //Debug.Log("PERFECT. CONG");
                EventManager.Broadcast(GameEvent.OnSuccess);
                StartCoroutine(OpenSuccess());
                //Particle and Success Panel
                //Dotween ile duzenlenecek
                //SuccessPanel.SetActive(true);
            }

            else
            {
                //Debug.Log("END FAIL");
                if(!gameData.isPlayerDead)
                    EventManager.Broadcast(GameEvent.OnPlayerDead);
            }
        }
        else
            return;
    }

   
    private void OnPlayerDead()
    {
        gameData.isGameEnd=true;
        pathData.playersCanMove=false;
        gameData.isPlayerDead=true;
        StartCoroutine(OpenFail());
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

    private IEnumerator OpenFail()
    {
        yield return waitForSeconds;
        OpenFailPanel();
    }


    private void OpenFailPanel()
    {
        //effektif
        EventManager.Broadcast(GameEvent.OnFailUI);
    }

    
}
