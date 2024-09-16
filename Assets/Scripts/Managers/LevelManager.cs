using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{

    [Header("Indexes")]
    [SerializeField] private GameData gameData;   
    public List<GameObject> levels;

    private bool isRestart=false;

    private void Awake()
    {
        LoadLevel();
        isRestart=false;
    }
    private void LoadLevel()
    {


        gameData.levelIndex = PlayerPrefs.GetInt("LevelNumber");
        if (gameData.levelIndex == levels.Count)
        {
            gameData.levelIndex = 2;
            isRestart=true;
            //SceneManager.LoadScene(0);

            //Tekrar ettigi icin event firlat bastaki yerlerine gitsin playerlar.
        }
        PlayerPrefs.SetInt("LevelNumber", gameData.levelIndex);
        
        gameData.levelNumber=PlayerPrefs.GetInt("RealLevel");

        EventManager.Broadcast(GameEvent.OnLevelUIUpdate);
       

        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].SetActive(false);
        }
        levels[gameData.levelIndex].SetActive(true);

        if(isRestart)
        {
            RestartLevel();
        }
        
    }

    

    public void LoadNextLevel()
    {
        PlayerPrefs.SetInt("LevelNumber", gameData.levelIndex + 1);
        PlayerPrefs.SetInt("RealLevel", PlayerPrefs.GetInt("RealLevel", 0) + 1);
        LoadLevel();
        EventManager.Broadcast(GameEvent.OnNextLevel);
    }

    public void RestartLevel()
    {
        
        EventManager.Broadcast(GameEvent.OnRestartLevel);
    }

    public void UndoButton()
    {
        EventManager.Broadcast(GameEvent.OnUndo);
    }

    
    
    
}
