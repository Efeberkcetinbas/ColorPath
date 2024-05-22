using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [Header("Indexes")]
    public int levelIndex;
    
    public List<GameObject> levels;

    private void Start()
    {
        LoadLevel();
    }
    private void LoadLevel()
    {


        levelIndex = PlayerPrefs.GetInt("LevelNumber");
        if (levelIndex == levels.Count) levelIndex = 0;
        PlayerPrefs.SetInt("LevelNumber", levelIndex);
       

        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].SetActive(false);
        }
        levels[levelIndex].SetActive(true);
    }

    public void LoadNextLevel()
    {
        PlayerPrefs.SetInt("LevelNumber", levelIndex + 1);
        PlayerPrefs.SetInt("RealLevel", PlayerPrefs.GetInt("RealLevel", 0) + 1);
        LoadLevel();
        EventManager.Broadcast(GameEvent.OnNextLevel);
    }

    public void RestartLevel()
    {
        LoadLevel();
    }


    
    
}
