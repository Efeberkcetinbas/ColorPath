using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameData gameData;
    public PlayerData playerData;
    public PathData pathData;

    [SerializeField] private Ease ease;
    

    public float InitialDifficultyValue;

    private WaitForSeconds waitForSeconds;

    [Header("Exit Confirmation")]
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;


    private void Awake() 
    {
        ClearData(true);
    }

    private void Start()
    {
        waitForSeconds=new WaitForSeconds(2);
        StartExitPanel();
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

    #region Graceful Exit
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // Show the confirmation panel when back button is pressed
            if (confirmationPanel.activeSelf == false)
            {
                confirmationPanel.SetActive(true);
            }
        }
    }

    private void StartExitPanel()
    {
        confirmationPanel.SetActive(false);

        // Add listeners for the buttons
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
    }
    
    private void OnYesButtonClicked()
    {
        // Quit the application
        Application.Quit();

        // For Android, make sure to use the AndroidJavaObject for exiting
        #if UNITY_ANDROID
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call("finish");
        #endif
    }

    private void OnNoButtonClicked()
    {
        // Hide the confirmation panel and resume the game
        confirmationPanel.SetActive(false);
    }

    #endregion

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
