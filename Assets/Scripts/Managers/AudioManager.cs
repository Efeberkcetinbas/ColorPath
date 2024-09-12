using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip GameLoop,BuffMusic;
    public AudioClip GameOverSound,PathAddSound,PlayerMoveSound,PlayerDeadSound,SuccessSound,SuccessUISound,RestartSound
    ,PlayersStartMoveSound,NextLevelSound,StartSound,FailUISound,FalseSound,WallBreakSound,ButtonPressSound,HitTargetSound,IncrementalSound;

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
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnSuccessUI,OnSuccessUI);
        EventManager.AddHandler(GameEvent.OnFailUI,OnFailUI);
        EventManager.AddHandler(GameEvent.OnOpenPlayButton,OnOpenPlayButton);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnGameStart,OnGameStart);
        EventManager.AddHandler(GameEvent.OnFalseDrag,OnFalseDrag);
        EventManager.AddHandler(GameEvent.OnWallBreak,OnWallBreak);
        EventManager.AddHandler(GameEvent.OnButtonPressed,OnButtonPressed);
        EventManager.AddHandler(GameEvent.OnHitTarget,OnHitTarget);
        EventManager.AddHandler(GameEvent.OnIncrementalPress,OnIncrementalPress);

    }
    private void OnDisable() 
    {
        EventManager.RemoveHandler(GameEvent.OnPathAdded,OnPathAdded);
        EventManager.RemoveHandler(GameEvent.OnPlayerMove,OnPlayerMove);
        EventManager.RemoveHandler(GameEvent.OnPlayerDead,OnPlayerDead);
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnSuccessUI,OnSuccessUI);
        EventManager.RemoveHandler(GameEvent.OnFailUI,OnFailUI);
        EventManager.RemoveHandler(GameEvent.OnOpenPlayButton,OnOpenPlayButton);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnGameStart,OnGameStart);
        EventManager.RemoveHandler(GameEvent.OnFalseDrag,OnFalseDrag);
        EventManager.RemoveHandler(GameEvent.OnWallBreak,OnWallBreak);
        EventManager.RemoveHandler(GameEvent.OnButtonPressed,OnButtonPressed);
        EventManager.RemoveHandler(GameEvent.OnHitTarget,OnHitTarget);
        EventManager.RemoveHandler(GameEvent.OnIncrementalPress,OnIncrementalPress);

    }

    private void OnHitTarget()
    {
        effectSource.PlayOneShot(HitTargetSound);
    }

    private void OnIncrementalPress()
    {
        effectSource.PlayOneShot(IncrementalSound);
    }

    
    private void OnFalseDrag()
    {
        effectSource.PlayOneShot(FalseSound);
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

    private void OnSuccess()
    {
        effectSource.PlayOneShot(SuccessSound);
    }

    private void OnSuccessUI()
    {
        effectSource.PlayOneShot(SuccessUISound);
    }

    private void OnOpenPlayButton()
    {
        effectSource.PlayOneShot(PlayersStartMoveSound);
    }

    private void OnRestartLevel()
    {
        effectSource.PlayOneShot(RestartSound);
    }

    private void OnNextLevel()
    {
        effectSource.PlayOneShot(NextLevelSound);
    }

    private void OnGameStart()
    {
        effectSource.PlayOneShot(StartSound);
    }

    private void OnFailUI()
    {
        effectSource.PlayOneShot(FailUISound);
    }

    private void OnWallBreak()
    {
        effectSource.PlayOneShot(WallBreakSound);
    }

    private void OnButtonPressed()
    {
        effectSource.PlayOneShot(ButtonPressSound);
    }

}
