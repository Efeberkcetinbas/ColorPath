using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject swipePointer,swipeText;


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
    }

    private void OnPlayersStartMove()
    {
        swipePointer.SetActive(false);
        swipeText.SetActive(false);
    }
}
