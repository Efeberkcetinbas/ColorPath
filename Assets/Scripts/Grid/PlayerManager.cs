using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    public List<PlayerMovement> players=new List<PlayerMovement>(); // List of all players with PlayerMovement script attached

    public PlayerMovement selectedPlayer; // Currently selected player

    internal int counter;

    private int randomIndex;
    private int lastRandomIndex = -1; // Initialize with a value that is not in the range


    private bool canCount;
    private bool openPlayButton;

    [SerializeField] private PathData pathData;
    [SerializeField] private GameData gameData;
    [SerializeField] private PlayerData playerData;

    private WaitForSeconds waitForSeconds;
    void Start()
    {
        waitForSeconds=new WaitForSeconds(1);
        OnGameStart();

    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnTeleportRandomPlayer,OnTeleportRandomPlayer);
        EventManager.AddHandler(GameEvent.OnGhost,OnGhost);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnTeleportRandomPlayer,OnTeleportRandomPlayer);
        EventManager.RemoveHandler(GameEvent.OnGhost,OnGhost);
    }

    private void OnGameStart()
    {
        //Debug.Log(players.Count);
        players.Clear();
        FindObjectOfType<LevelProperty>().SetTempList();
        players=FindObjectOfType<LevelProperty>().tempList;
        //Debug.Log(players.Count);
        openPlayButton=false;
        playerData.numberOfPlayers=players.Count;
        counter=0;
        gameData.isPlayerDead=false;

    }

    private void OnGhost()
    {
        gameData.isPlayerGhost=true;
    }

    private void OnTeleportRandomPlayer()
    {
        int randomIndex;

        if (players.Count <= 1)
        {
            randomIndex = 0; // If only one player exists or the count is 1, use index 0.
        }
        else
        {
            randomIndex = Random.Range(0, players.Count - 1); // Generate random number
            if (randomIndex >= lastRandomIndex)
            {
                randomIndex++; // Skip the last random index
            }
        }

        // Teleport the player at the new random index
        players[randomIndex].TeleportToTarget();

        // Update the lastRandomIndex to the new one
        lastRandomIndex = randomIndex;
    }

    private void OnNextLevel()
    {
        OnGameStart();
    }

    private void OnRestartLevel()
    {
        counter=0;
        openPlayButton=false;
        gameData.isPlayerDead=false;
    }
    void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0 && !gameData.isGameEnd)
        {
            Touch touch = Input.GetTouch(0);

            // Handle touch phase
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    HandleTouchBegin(touch.position);
                    break;

                case TouchPhase.Moved:
                    HandleTouchMove(touch.position);
                    break;

                case TouchPhase.Ended:
                    HandleTouchEnd();
                    break;
            }
        }
    }

    void HandleTouchBegin(Vector2 touchPosition)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Check if the hit object is a player
            PlayerMovement player = hitObject.GetComponent<PlayerMovement>();
            if (player != null && players.Contains(player))
            {
                //Debug.Log("PLAYER CHOOSEN");
                // Select the touched player
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].isMe=false;
                    players[i].handHere=false;
                }
                
                selectedPlayer = player;
                selectedPlayer.isMe=true;
                selectedPlayer.handHere=true;
                //Debug.Log(selectedPlayer.name);
                canCount=true;
                playerData.selectedColor=selectedPlayer.playerColor;
                //selectedPlayer.transform.DOScale(transform.localScale*1.2f,0.1f).OnComplete(()=>selectedPlayer.transform.DOScale(transform.localScale,0.1f));
                EventManager.Broadcast(GameEvent.OnPlayerSelection);
                EventManager.Broadcast(GameEvent.OnStopFalseDrag);
                //Burada liste kontrolunu yapabilirsin

                
            }

            else
            {
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].isMe=false;
                }

                canCount=false;
                //Debug.Log("BOS YERE DOKUNUYORSUN");
                EventManager.Broadcast(GameEvent.OnFalseDrag);

                //


            
                





                //

            }
        }
    }

    void HandleTouchMove(Vector2 touchPosition)
    {
        if (selectedPlayer != null)
        {
            // Continue drawing path for the selected player
            selectedPlayer.ContinueDragging(touchPosition);
        }
    }

    void HandleTouchEnd()
    {
        //selectedPlayer = null; // Deselect the player when touch ends
        if(selectedPlayer!=null)
        {
            /*if(canCount && selectedPlayer.canCountOnMe)
                counter++;*/

            selectedPlayer.canCountOnMe=false;

        }

        selectedPlayer=null;
        EventManager.Broadcast(GameEvent.OnPlayerNull);
        


        //StartCoroutine(CheckCounter());
        


        
    }

    internal IEnumerator CheckCounter()
    {
        yield return waitForSeconds;

        if(playerData.numberOfPlayers==counter && !openPlayButton)
        {
            OnPlayersMove();
            EventManager.Broadcast(GameEvent.OnOpenPlayButton);
            openPlayButton=true;
        }
            

        
    }

    public void OnPlayersMove()
    {
        pathData.playersCanMove=true;
        gameData.isGameEnd=true;
        //Debug.Log("START TO MOVE");
        EventManager.Broadcast(GameEvent.OnPlayersStartMove);    
    }
    
}
