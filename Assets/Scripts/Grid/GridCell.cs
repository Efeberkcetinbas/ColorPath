using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    None,
    Yellow,
    Red,
    Blue,
    Green,
    Purple,
    Aqua,
    Orange,
    Pink
}
public class GridCell : MonoBehaviour
{
    public int row;
    public int column;

    [SerializeField] private GridManager gridManager;
    private bool isTouching = false;
    [SerializeField] private ParticleSystem dust;
    //Bunu daha sonra array olarak tutki ust uste gectiklerinde renkler degisime ugrasin
    public List<Color> cellColors=new List<Color>();

    public List<ParticleSystem> particles=new List<ParticleSystem>();

    

    public List<PlayerMovement> players=new List<PlayerMovement>();
    public List<CellType> cellTypes=new List<CellType>();

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
    }


    void OnTriggerEnter(Collider other)
    {
        isTouching = true;
        if(other.CompareTag("Player"))
        {
            Debug.Log("ENTER PLAYER" + other.name);

            if(cellTypes.Count>0)
            {

                //Parenta ver
                if(other.transform.parent.GetComponent<PlayerMovement>().playerType== cellTypes[cellTypes.Count-1])
                {
                    Debug.Log("PARTICLE AND SUCCESS");
                    dust.Play();
                    
                }

                else
                {
                    EventManager.Broadcast(GameEvent.OnPlayerDead);
                }
                    
            }
        }
    }

    private void OnRestartLevel()
    {
        cellColors.Clear();
        players.Clear();
        cellTypes.Clear();
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
    void OnTriggerExit(Collider other)
    {
        isTouching = false;

        if(other.CompareTag("Player"))
        {
            if(cellTypes.Count>0)
            {
            //Eger altinda baska bir renk yoksa
                gridManager.ResetCellMaterial(row, column);
                if(players.Count!=0)
                {
                    players.RemoveAt(players.Count-1);
                    cellTypes.RemoveAt(cellTypes.Count-1);
                }
            }
            
        }
    }
}
