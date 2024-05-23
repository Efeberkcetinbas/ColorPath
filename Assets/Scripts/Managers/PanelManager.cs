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
    [SerializeField] private Image Fade;
    [SerializeField] private float sceneX,sceneY,oldSceneX,oldSceneY,duration;


    public GameData gameData;

    private WaitForSeconds waitForSeconds;

    private void Start()
    {
        waitForSeconds=new WaitForSeconds(.25f);
    }





    private void OnEnable() 
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnSuccessUI,OnSuccessUI);
    }


    private void OnDisable() 
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnSuccessUI,OnSuccessUI);
    }

    
    public void StartGame() 
    {
        gameData.isGameEnd=false;
        StartPanel.gameObject.SetActive(false);
        ScenePanel.gameObject.SetActive(true);
        SetSceneUIPosition(sceneX,sceneY);
        //EventManager.Broadcast(GameEvent.OnGameStart);
        
    }

    

    private void OnRestartLevel()
    {
        OnNextLevel();
    }

    

    private void OnNextLevel()
    {
        /*StartPanel.gameObject.SetActive(true);
        StartPanel.DOAnchorPos(Vector2.zero,0.1f);*/
        SuccessPanel.gameObject.SetActive(false);
        StartCoroutine(Blink(Fade.gameObject,Fade));
    }

    private void OnSuccessUI()
    {
        SuccessPanel.gameObject.SetActive(true);
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
            SceneUIs[i].SetActive(val);
        }
    }

    private void OnSuccess()
    {
        SetSceneUIPosition(oldSceneX,oldSceneY);
    }

    private void SetSceneUIPosition(float valX,float valY)
    {
        ScenePanel.DOAnchorPos(new Vector2(valX,valY),duration);
    }

}
