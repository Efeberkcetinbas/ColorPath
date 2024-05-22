using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private RectTransform StartPanel;

    [SerializeField] private List<GameObject> SceneUIs=new List<GameObject>();
    [SerializeField] private Image Fade;


    public GameData gameData;






    private void OnEnable() 
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
    }


    private void OnDisable() 
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
    }

    
    public void StartGame() 
    {
        gameData.isGameEnd=false;
        StartPanel.gameObject.SetActive(false);

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
        StartCoroutine(Blink(Fade.gameObject,Fade));
    }


  

    private IEnumerator Blink(GameObject gameObject,Image image)
    {
        
        gameObject.SetActive(true);
        image.color=new Color(0,0,0,1);
        image.DOFade(0,2f);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);

    }


    private void SetActivity(List<GameObject> list,bool val)
    {
        for (int i = 0; i < list.Count; i++)
        {
            SceneUIs[i].SetActive(val);
        }
    }

}
