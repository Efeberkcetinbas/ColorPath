using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI score,levelText;

    [SerializeField] private List<GameObject> specialButtons=new List<GameObject>();
    public GameData gameData;
    public PlayerData playerData;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnUIUpdate, OnUIUpdate);
        EventManager.AddHandler(GameEvent.OnPlayersStartMove, OnPlayersStartMove);
        EventManager.AddHandler(GameEvent.OnNextLevel, OnNextLevel);
        EventManager.AddHandler(GameEvent.OnRestartLevel, OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnLevelUIUpdate,OnLevelUIUpdate);
        //EventManager.AddHandler(GameEvent.OnPlayerStopsMove, OnPlayerStopsMove);
        
        
    }
    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnUIUpdate, OnUIUpdate);
        EventManager.RemoveHandler(GameEvent.OnPlayersStartMove, OnPlayersStartMove);
        EventManager.RemoveHandler(GameEvent.OnNextLevel, OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel, OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnLevelUIUpdate,OnLevelUIUpdate);
        //EventManager.RemoveHandler(GameEvent.OnPlayerStopsMove, OnPlayerStopsMove);
    }

    private void Start()
    {
        gameData.score=PlayerPrefs.GetInt("ScoreGame",5);
        OnUIUpdate();
    }

    
    private void OnUIUpdate()
    {
        score.SetText(gameData.score.ToString());
        //score.transform.DOScale(new Vector3(1.5f,1.5f,1.5f),0.2f).OnComplete(()=>score.transform.DOScale(new Vector3(1,1f,1f),0.2f));
    }

    private void OnLevelUIUpdate()
    {
        levelText.SetText("LVL " + (gameData.levelNumber+1).ToString());
    }
   

    private void OnPlayerStopsMove()
    {
        CheckButtonInteractability(true);
    }
    private void OnPlayersStartMove()
    {
        CheckButtonInteractability(false);
    }

    private void OnNextLevel()
    {
        CheckButtonInteractability(true);
    }

    private void OnRestartLevel()
    {
        CheckButtonInteractability(true);
    }


    private void CheckButtonInteractability(bool val)
    {
        for (int i = 0; i < specialButtons.Count; i++)
        {
            specialButtons[i].SetActive(val);
        }
    }
    
}
