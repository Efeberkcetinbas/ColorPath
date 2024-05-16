using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;




public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] private List<GridCell> path = new List<GridCell>(); // List to store the movement path
    [SerializeField] private List<Vector3> tempPath=new List<Vector3>();
    
    private bool isDragging = false; // Flag to track if the user is currently dragging

    [SerializeField] private GameData gameData;
    [SerializeField] private PathData pathData;
    [SerializeField] private PlayerData playerData;
    public Color playerColor; // Color assigned to this player
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Transform target;


    
    [SerializeField] private SkinnedMeshRenderer playerRenderer; // Renderer component to apply color to the player

    public bool isMe=false;
    public bool canCountOnMe=true;
    public Animator animator;

    public CellType playerType;

    private bool orderCell=true;

    void Start()
    {
        OnNextLevel();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlayerDead,OnPlayerDead);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlayerDead,OnPlayerDead);
    }

    private void OnNextLevel()
    {
        playerRenderer.material.color = playerColor; // Set the player's color
        EventManager.Broadcast(GameEvent.OnPlayerColorUpdate);
    }

    private void OnPlayerDead()
    {
        Debug.Log("PARTICLE");
    }



    void Update()
    {
         // Check for touch input
        if (isMe && !gameData.isGameEnd)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
    
                // Handle touch phase
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        StartDragging(touch.position);
                        break;
    
                    case TouchPhase.Moved:
                        if (isDragging)
                        {
                            ContinueDragging(touch.position);
                        }
                        break;
    
                    case TouchPhase.Ended:
                        EndDragging();
                        break;
                }
            }
        }

        // Move the player along the path if it's not empty
        if (pathData.playersCanMove)
        {
            if (path.Count > 0)
            {
                MovePlayerAlongPath();
            }
        }
    }

    void StartDragging(Vector2 touchPosition)
    {
        // Perform a raycast to detect the grid cell the user started dragging from
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out hit))
        {
            GridCell hitCell = hit.collider.GetComponent<GridCell>();
            if (hitCell != null)
            {
                // Clear existing path and add the starting cell to the path
                path.Clear();
                path.Add(hitCell);
                isDragging = true;
            }
        }
    }

    internal void ContinueDragging(Vector2 touchPosition)
    {
        // Perform a raycast to detect the grid cell the user is currently dragging over
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out hit))
        {
            GridCell hitCell = hit.collider.GetComponent<GridCell>();
            if (hitCell != null && IsAdjacentToPreviousCell(hitCell) && !path.Contains(hitCell))
            {
                // Add the current cell to the path if it's adjacent to the previous cell
                path.Add(hitCell);
                Debug.Log("EVENTSSSS WILLLL BEEE ADDED");
                EventManager.Broadcast(GameEvent.OnPathAdded);
                hitCell.players.Add(this);
                gridManager.HighlightCell(hitCell.row,hitCell.column,playerColor);
                hitCell.cellTypes.Add(playerType);
            }
        }
    }

    void EndDragging()
    {
        isDragging = false;
        tempPath.Clear();

        foreach ( GridCell cell in path)
        {
            tempPath.Add(cell.transform.position);
        }


        

       
    }

    bool IsAdjacentToPreviousCell(GridCell cell)
    {
        if (path.Count > 0)
        {
            GridCell lastCell = path[path.Count - 1];
            
            // Check if the cells are adjacent horizontally or vertically
            bool isHorizontalAdjacent = Mathf.Abs(lastCell.row - cell.row) == 1 && lastCell.column == cell.column;
            bool isVerticalAdjacent = lastCell.row == cell.row && Mathf.Abs(lastCell.column - cell.column) == 1;
            
            // Return true if the cells are adjacent horizontally or vertically
            return isHorizontalAdjacent || isVerticalAdjacent;
        }
        
        return true; // Return true if there's no previous cell (first cell in the path)
    }

    void MovePlayerAlongPath()
    {
        if (path.Count > 0 && orderCell)
        {
        // Move the player towards the first cell in the path
            GameObject targetCell = path[0].gameObject;
            orderCell=false;
            //transform.position = Vector3.MoveTowards(transform.position, targetCell.transform.position, moveSpeed * Time.deltaTime);
            transform.DOJump(targetCell.transform.position,1,1,0.5f).OnComplete(()=>{
                EventManager.Broadcast(GameEvent.OnPlayerMove);
                path.RemoveAt(0);

                // If the path is now empty, the player has finished the path
                if (path.Count == 0)
                {
                    Debug.Log(name + " FINISHED THE PATH CHECK IT");
                    playerData.pathCompletedCounter++;

                        // Check if the player has reached the final target position
                    if (Vector3.Distance(transform.position, target.position) < 0.01f)
                    {
                        Debug.Log("SUCCESS PATH");
                        playerData.successPathCompletedCounter++;
                        target.DOLocalMoveY(-1,0.2f);
                    }

                    //animator.SetBool("walk",false);
                }
                orderCell=true;
                
                
            });
            //Animation
            //animator.SetBool("walk",true);

            //
            // Check if the player has reached the target position
            /*if (Vector3.Distance(transform.position, targetCell.transform.position) < 0.01f)
            {
                // Remove the reached cell from the path
                path.RemoveAt(0);

                // If the path is now empty, the player has finished the path
                if (path.Count == 0)
                {
                    Debug.Log(name + " FINISHED THE PATH CHECK IT");
                    playerData.pathCompletedCounter++;

                    // Check if the player has reached the final target position
                    if (Vector3.Distance(transform.position, target.position) < 0.01f)
                    {
                        Debug.Log("SUCCESS PATH");
                        playerData.successPathCompletedCounter++;
                        target.DOLocalMoveY(-1,0.2f);
                    }

                    //animator.SetBool("walk",false);
                }
            }*/
        }
    }

    
}
