using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private RectTransform StartPanel,ScenePanel,SuccessPanel,FailPanel;

    [SerializeField] private List<GameObject> SceneUIs=new List<GameObject>();
    [SerializeField] private List<GameObject> SuccessElements=new List<GameObject>();
    [SerializeField] private List<GameObject> FailElements=new List<GameObject>();
    [SerializeField] private List<GameObject> SpecialElements=new List<GameObject>();
    [SerializeField] private Image Fade,ColorSquare;
    [SerializeField] private float sceneX,sceneY,oldSceneX,oldSceneY,duration;

    [SerializeField] private Button playButton,resetButton;
    [SerializeField] private Button startGameButton,restartLevelButton;

    [SerializeField] private GameObject startLifePanel,failLifePanel,falseDragPanel;


    public GameData gameData;
    public PlayerData playerData;

    private WaitForSeconds waitForSeconds,waitforSecondsSpecial;

    private void Start()
    {
        waitForSeconds=new WaitForSeconds(.25f);
        waitforSecondsSpecial=new WaitForSeconds(1);
        ColorSquare.color=Color.white;
        //ResetPlayButton(); 
        CheckLifeCounter(startGameButton,startLifePanel);

        
        
    }

   


    private void CheckLifeCounter(Button button,GameObject lifePanelObject)
    {
        if(gameData.lifeTime==0)
        {
            button.interactable=false;
            lifePanelObject.SetActive(true);
            
        }

        else
        {
            button.interactable=true;
            lifePanelObject.SetActive(true);
        }
            
    }

    

    private void OnUpdateLife()
    {
        CheckLifeCounter(startGameButton,startLifePanel);
        CheckLifeCounter(restartLevelButton,failLifePanel);
    }





    private void OnEnable() 
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnSuccessUI,OnSuccessUI);
        EventManager.AddHandler(GameEvent.OnPlayerDead,OnPlayerDead);
        EventManager.AddHandler(GameEvent.OnFailUI,OnFailUI);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnPlayerSelection,OnPlayerSelection);
        EventManager.AddHandler(GameEvent.OnPlayerNull,OnPlayerNull);
        //EventManager.AddHandler(GameEvent.OnOpenPlayButton,OnOpenPlayButton);
        EventManager.AddHandler(GameEvent.OnUpdateLife,OnUpdateLife);
        EventManager.AddHandler(GameEvent.OnFalseDrag,OnFalseDrag);
        EventManager.AddHandler(GameEvent.OnLifeFullUI,OnLifeFullUI);
        EventManager.AddHandler(GameEvent.OnLifeIncrease,OnLifeIncrease);

    }


    private void OnDisable() 
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnSuccessUI,OnSuccessUI);
        EventManager.RemoveHandler(GameEvent.OnPlayerDead,OnPlayerDead);
        EventManager.RemoveHandler(GameEvent.OnFailUI,OnFailUI);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnPlayerSelection,OnPlayerSelection);
        EventManager.RemoveHandler(GameEvent.OnPlayerNull,OnPlayerNull);
        //EventManager.RemoveHandler(GameEvent.OnOpenPlayButton,OnOpenPlayButton);
        EventManager.RemoveHandler(GameEvent.OnUpdateLife,OnUpdateLife);
        EventManager.RemoveHandler(GameEvent.OnFalseDrag,OnFalseDrag);
        EventManager.RemoveHandler(GameEvent.OnLifeFullUI,OnLifeFullUI);
        EventManager.RemoveHandler(GameEvent.OnLifeIncrease,OnLifeIncrease);

    }

    
    public void StartGame() 
    {
        gameData.isGameEnd=false;
        StartPanel.gameObject.SetActive(false);
        ScenePanel.gameObject.SetActive(true);
        SetSceneUIPosition(sceneX,sceneY);

       
        StartCoroutine(SetElementsDotween(SpecialElements));
        EventManager.Broadcast(GameEvent.OnGameStart);
        
    }

    private void OnLifeFullUI()
    {
        OnUpdateLife();
        EventManager.Broadcast(GameEvent.OnRestartLevel);
    }
    

    private void OnRestartLevel()
    {
        ColorSquare.color=Color.white;
        FailPanel.gameObject.SetActive(false);
        StartCoroutine(Blink(Fade.gameObject,Fade));
        SetActivity(SceneUIs,true);
        StartCoroutine(SetElementsDotween(SpecialElements));
        //ResetPlayButton();
    }

    

    private void OnNextLevel()
    {
        /*StartPanel.gameObject.SetActive(true);
        StartPanel.DOAnchorPos(Vector2.zero,0.1f);*/
        ColorSquare.color=Color.white;
        SuccessPanel.gameObject.SetActive(false);
        StartCoroutine(Blink(Fade.gameObject,Fade));
        SetActivity(SceneUIs,true);
        StartCoroutine(SetElementsDotween(SpecialElements));
        //ResetPlayButton();
    }

    private void OnSuccessUI()
    {
        SuccessPanel.gameObject.SetActive(true);
        SetActivity(SceneUIs,false);
        StartCoroutine(SetElementsDotween(SuccessElements));
    }
  

    private IEnumerator Blink(GameObject gameObject,Image image)
    {
        
        gameObject.SetActive(true);
        image.color=new Color(0,0,0,1);
        image.DOFade(0,2f);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        SetSceneUIPosition(sceneX,sceneY);

    }

    private IEnumerator SetElementsDotween(List<GameObject> elements)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].transform.localScale=Vector3.zero;
        }

        for (int i = 0; i < elements.Count; i++)
        {
            yield return waitForSeconds;
            elements[i].transform.DOScale(Vector3.one,duration);
        }
    }

    private void SetActivity(List<GameObject> list,bool val)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetActive(val);
        }
    }

    private void OnSuccess()
    {
        //SetActivity(SceneUIs,false);
        SetSceneUIPosition(oldSceneX,oldSceneY);
    }

    private void OnPlayerDead()
    {
        SetSceneUIPosition(oldSceneX,oldSceneY);
        
    }

    private void OnFalseDrag()
    {
        StartCoroutine(SetActivity());
    }

    private IEnumerator SetActivity()
    {
        falseDragPanel.SetActive(true);
        falseDragPanel.transform.DOScale(Vector3.one,0.25f).SetEase(Ease.InBounce);
        yield return waitforSecondsSpecial;
        falseDragPanel.transform.DOScale(Vector3.zero,0.25f).SetEase(Ease.InBounce).OnComplete(()=>falseDragPanel.SetActive(false));

    }
    private void OnFailUI()
    {
        FailPanel.gameObject.SetActive(true);
        SetActivity(SceneUIs,false);
        StartCoroutine(SetElementsDotween(FailElements));
        CheckLifeCounter(restartLevelButton,failLifePanel);
    }

    private void OnLifeIncrease()
    {
        restartLevelButton.interactable=true;
    }

    private void SetSceneUIPosition(float valX,float valY)
    {
        ScenePanel.DOAnchorPos(new Vector2(valX,valY),duration);
    }

    private void OnPlayerSelection()
    {
        ColorSquare.color=playerData.selectedColor;
    }

    private void OnPlayerNull()
    {
        ColorSquare.color=Color.white;
    }

    private void OnOpenPlayButton()
    {
        playButton.transform.DOScale(Vector3.one*1.5f,0.1f);
        playButton.interactable=true;
        resetButton.interactable=false;
    }

    private void ResetPlayButton()
    {
        playButton.interactable=false;
        playButton.transform.DOScale(Vector3.one,0.1f);
        resetButton.interactable=true;
        falseDragPanel.SetActive(false);
    }

}
