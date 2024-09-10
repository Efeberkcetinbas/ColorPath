using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;




public class BreakeableWall : MonoBehaviour
{

    private bool check=false;

    
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private PlayerMovement targetBall;
    [SerializeField] private Transform wall;
    [SerializeField] private PathData pathData;
    [SerializeField] private GameData gameData;
    [SerializeField] private List<ParticleSystem> particles=new List<ParticleSystem>();

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
                    wall.transform.DOLocalMoveY(-2,0.1f);
                    EventManager.Broadcast(GameEvent.OnWallBreak);
                    PlayParticles(targetBall.playerColor);
                    check=true;
                }

                else
                {
                    if(!gameData.isPlayerDead)
                        EventManager.Broadcast(GameEvent.OnPlayerDead);
                }

            }
    }

    private void OnRestartLevel()
    {
        check=false;
        wall.transform.DOLocalMoveY(0,0.1f);
    }

    
   
    

    internal void PlayParticles(Color color)
    {
        for (int i = 0; i < particles.Count; i++)
        {
            
            SetParticleSystemColor(particles[i],color);
            ParticleSystem[] childParticles = particles[i].GetComponentsInChildren<ParticleSystem>();
            for (int j = 0; j < childParticles.Length; j++)
            {
                SetParticleSystemColor(childParticles[j], color);
            }
        }

        particles[0].Play();
    }

    private void SetParticleSystemColor(ParticleSystem particleSystem, Color color)
    {
        var main = particleSystem.main;
        main.startColor = color;
    }
    

    
}
