using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetControl : MonoBehaviour
{
    
    [SerializeField] private MeshRenderer meshRenderer;
    
    [SerializeField] private PlayerMovement player;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlayerColorUpdate,OnPlayerColorUpdate);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlayerColorUpdate,OnPlayerColorUpdate);
    }
    private void OnPlayerColorUpdate()
    {
        meshRenderer.material.color=player.playerColor;
    }
}
