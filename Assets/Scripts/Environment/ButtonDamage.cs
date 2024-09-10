using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonDamage : MonoBehaviour
{
    private bool check=false;
    [SerializeField] private List<GameObject> damageableObject=new List<GameObject>();
    [SerializeField] private GameObject button;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private PlayerMovement targetBall;
    [SerializeField] private PathData pathData;

    private void Start() 
    {
        meshRenderer.material.color=targetBall.playerColor;
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
    }
    

    private void OnTriggerEnter(Collider other)
    {
            if(pathData.playersCanMove && !check)
            {
                
                if(other.transform.parent.GetComponent<PlayerMovement>() == targetBall)
                {
                    button.transform.DOLocalMoveY(-1,0.1f);
                    check=true;
                    EventManager.Broadcast(GameEvent.OnButtonPressed);
                    for (int i = 0; i < damageableObject.Count; i++)
                    {
                        damageableObject[i].transform.DOLocalMoveY(-2,0.5f).OnComplete(()=>{
                            damageableObject[i].SetActive(false);    
                        });
                        
                    }
                    
                }

               

            }
    }

    private void OnRestartLevel()
    {
        check=false;
        button.transform.DOLocalMoveY(0,0.1f);
        for (int i = 0; i < damageableObject.Count; i++)
        {
            damageableObject[i].SetActive(true);    
            damageableObject[i].transform.DOLocalMoveY(0,0.2f);
        }
    }
    
}
