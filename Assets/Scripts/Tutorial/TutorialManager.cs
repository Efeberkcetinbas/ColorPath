using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject swipePointer,pointFinger,swipeText;


    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnOpenPlayButton,OnOpenPlayButton);
        EventManager.AddHandler(GameEvent.OnPlayersStartMove,OnPlayersStartMove);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnOpenPlayButton,OnOpenPlayButton);
        EventManager.RemoveHandler(GameEvent.OnPlayersStartMove,OnPlayersStartMove);
    }

    private void OnOpenPlayButton()
    {
        swipePointer.SetActive(false);
        swipeText.SetActive(false);
        pointFinger.SetActive(true);
    }

    private void OnPlayersStartMove()
    {
        swipePointer.SetActive(false);
        swipeText.SetActive(false);
        pointFinger.SetActive(false);
    }
}
