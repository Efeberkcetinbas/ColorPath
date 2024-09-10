using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPoint : MonoBehaviour
{
    [SerializeField] private PathData pathData;
    [SerializeField] private GameData gameData;
    private void OnTriggerEnter(Collider other)
    {
        if(pathData.playersCanMove)
        {
            if(other.CompareTag("Player"))
            {
                if(!gameData.isPlayerDead)
                    EventManager.Broadcast(GameEvent.OnPlayerDead);
            }
        }
        
    }
}
