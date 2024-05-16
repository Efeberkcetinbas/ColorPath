using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<PlayerMovement> players=new List<PlayerMovement>(); // List of all players with PlayerMovement script attached

    [SerializeField] private PlayerMovement selectedPlayer; // Currently selected player

    private int numberOfPlayers;
    private int counter;


    private bool canCount;

    [SerializeField] private PathData pathData;
    [SerializeField] private GameData gameData;


    private WaitForSeconds waitForSeconds;
    void Start()
    {
        waitForSeconds=new WaitForSeconds(1);
        OnGameStart();

    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnGameStart,OnGameStart);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnGameStart,OnGameStart);
    }

    private void OnGameStart()
    {
        players.Clear();
        players=FindObjectOfType<LevelProperty>().levelPlayersList;
        

        numberOfPlayers=players.Count;
        counter=0;

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
                // Select the touched player
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].isMe=false;
                }
                
                selectedPlayer = player;
                selectedPlayer.isMe=true;
                Debug.Log(selectedPlayer.name);
                canCount=true;
                //Burada liste kontrolunu yapabilirsin
            }

            else
            {
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].isMe=false;
                }

                canCount=false;

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
            if(canCount && selectedPlayer.canCountOnMe)
                counter++;

            selectedPlayer.canCountOnMe=false;

        }

        selectedPlayer=null;
        

        StartCoroutine(CheckCounter());
        


        
    }

    private IEnumerator CheckCounter()
    {
        yield return waitForSeconds;

        if(numberOfPlayers==counter)
        {
            pathData.playersCanMove=true;
            gameData.isGameEnd=true;

        }
            

        
    }
}
