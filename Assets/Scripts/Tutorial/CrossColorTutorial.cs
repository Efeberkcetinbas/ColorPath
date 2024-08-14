using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossColorTutorial : MonoBehaviour
{
    [SerializeField] private GameObject firstPointer,secondPointer,overlapText;
    [SerializeField] private BoxCollider firstCollider;

    private bool isFirst=true;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnHitTarget,OnHitTarget);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnOpenPlayButton,OnOpenPlayButton);
        
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnHitTarget,OnHitTarget);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnOpenPlayButton,OnOpenPlayButton);
    }

    private void Start()
    {
        OnStart();
    }

    private void OnRestartLevel()
    {
        OnStart();
    }

    

    private void OnHitTarget()
    {
        if(isFirst)
        {
            firstPointer.SetActive(false);
            secondPointer.SetActive(true);
            firstCollider.enabled=true;
            isFirst=false;
        }
    }

    private void OnStart()
    {
        isFirst=true;
        firstPointer.SetActive(true);
        secondPointer.SetActive(false);
        firstCollider.enabled=false;
        overlapText.SetActive(false);
    }

    private void OnOpenPlayButton()
    {
        firstPointer.SetActive(false);
        secondPointer.SetActive(false);
        overlapText.SetActive(true);
    }

    
}
