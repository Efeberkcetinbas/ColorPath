using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTutorial : MonoBehaviour
{
    [SerializeField] private GameObject colorBarPointer,colorBarText;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnOpenPlayButton,OnOpenPlayButton);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnOpenPlayButton,OnOpenPlayButton);
    }

    private void OnOpenPlayButton()
    {
        colorBarPointer.SetActive(false);
        colorBarText.SetActive(false);
    }
}
