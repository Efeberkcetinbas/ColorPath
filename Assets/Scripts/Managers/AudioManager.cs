using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip GameLoop,BuffMusic;
    public AudioClip GameOverSound,PathAddSound,PlayerMoveSound,PlayerDeadSound;

    AudioSource musicSource,effectSource;


    private void Start() 
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = GameLoop;
        //musicSource.Play();
        effectSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable() 
    {
        EventManager.AddHandler(GameEvent.OnPathAdded,OnPathAdded);
        EventManager.AddHandler(GameEvent.OnPlayerMove,OnPlayerMove);
        EventManager.AddHandler(GameEvent.OnPlayerDead,OnPlayerDead);
    }
    private void OnDisable() 
    {
        EventManager.RemoveHandler(GameEvent.OnPathAdded,OnPathAdded);
        EventManager.RemoveHandler(GameEvent.OnPlayerMove,OnPlayerMove);
        EventManager.RemoveHandler(GameEvent.OnPlayerDead,OnPlayerDead);
    }

    

    private void OnGameOver()
    {
        effectSource.PlayOneShot(GameOverSound);
    }

    private void OnPathAdded()
    {
        effectSource.PlayOneShot(PathAddSound);
    }

    private void OnPlayerMove()
    {
        effectSource.PlayOneShot(PlayerMoveSound);
    }

    private void OnPlayerDead()
    {
        effectSource.PlayOneShot(PlayerDeadSound);
    }



}
