using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ScoreManager : MonoBehaviour
{
    public GameData gameData;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnIncreaseScore, OnIncreaseScore);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnIncreaseScore, OnIncreaseScore);
    }
    private void OnIncreaseScore()
    {
        //gameData.score += 50;
        DOTween.To(GetScore,ChangeScore,gameData.score+gameData.increaseScore,.5f).OnUpdate(UpdateUI);
    }

    

    private int GetScore()
    {
        return gameData.score;
    }

    private void ChangeScore(int value)
    {
        gameData.score=value;
        PlayerPrefs.SetInt("ScoreGame",gameData.score);
    }

    private void UpdateUI()
    {
        EventManager.Broadcast(GameEvent.OnUIUpdate);
    }


}
