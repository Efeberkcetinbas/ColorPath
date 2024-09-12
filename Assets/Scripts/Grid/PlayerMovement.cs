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
    private bool isTeleport=false;
    private bool hasReachedTarget = false;

    [SerializeField] private GameData gameData;
    [SerializeField] private PathData pathData;
    [SerializeField] private PlayerData playerData;
    public Color playerColor; // Color assigned to this player
    [SerializeField] private GridManager gridManager;
    private PlayerManager playerManager;
    [SerializeField] private Transform target;
    private GridCell startCell;

    
    [SerializeField] private SkinnedMeshRenderer playerRenderer; // Renderer component to apply color to the player

    public bool isMe=false;
    public bool canCountOnMe=true;
    public Animator animator;

    public CellType playerType;

    private bool orderCell=true;

    [SerializeField] private Vector3 startPos;

    public bool handHere;

    [SerializeField] private GameObject selectMeBall;

    void Start()
    {
        OnNextLevel();
        StartCoroutine(StarterAddCellToPath(startCell));

    }

    

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlayerDead,OnPlayerDead);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnFalseDrag,OnFalseDrag);
        EventManager.AddHandler(GameEvent.OnStopFalseDrag,OnStopFalseDrag);

    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlayerDead,OnPlayerDead);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnFalseDrag,OnFalseDrag);
        EventManager.RemoveHandler(GameEvent.OnStopFalseDrag,OnStopFalseDrag);
    }

    private void OnNextLevel()
    {
        playerManager=FindObjectOfType<PlayerManager>();
        playerRenderer.material.color = playerColor; // Set the player's color
        startPos=transform.position;
        EventManager.Broadcast(GameEvent.OnPlayerColorUpdate);
        isTeleport=false;
    }

    private void OnPlayerDead()
    {
        //Debug.Log("PARTICLE");
        animator.SetTrigger("dead");
        //Debug.Log("COLLISION HAS HAPPENED");
        
    }

    private void OnRestartLevel()
    {
        canCountOnMe=true;
        path.Clear();
        tempPath.Clear();
        DOTween.Kill(transform);
        transform.DOMove(startPos,0.1f);
        StartCoroutine(StarterAddCellToPath(startCell));
        target.DOMoveY(0,0.1f);
        animator.SetTrigger("idle");
        isTeleport=false;
        hasReachedTarget=false;
        
    }

    void Update()
    {
         // Check for touch input
        if (isMe && !gameData.isGameEnd && !isTeleport)
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
            
            /*else
            {
                EventManager.Broadcast(GameEvent.OnPlayerStopsMove);
            }*/
        }
    }

    void StartDragging(Vector2 touchPosition)
    {
        // Perform a raycast to detect the grid cell the user started dragging from
        
        
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        int wallLayer = LayerMask.NameToLayer("Wall");
        int layerMask = ~(1 << wallLayer); // Exclude the wall layer

        if (Physics.Raycast(ray, out hit,Mathf.Infinity, layerMask))
        {
            GridCell hitCell = hit.collider.GetComponent<GridCell>();
            if (hitCell != null)
            {
                // Clear existing path and add the starting cell to the path
                /*path.Clear();
                path.Add(hitCell);*/
                isDragging = true;
                //Debug.Log("INSIDE THE NULL CHECK");
            }

            //Debug.Log("OUTSIDE THE NULL CHECK");
        }
    }

    private void OnFalseDrag()
    {
        if(handHere)
        {
            selectMeBall.SetActive(true);
        }
    }

    private void OnStopFalseDrag()
    {
        selectMeBall.SetActive(false);
    }

    internal void ContinueDragging(Vector2 touchPosition)
    {
        // Perform a raycast to detect the grid cell the user is currently dragging over
        int wallLayer = LayerMask.NameToLayer("Wall");
        int layerMask = ~(1 << wallLayer); // Exclude the wall layer
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out RaycastHit hit,Mathf.Infinity, layerMask))
        {
            GridCell hitCell = hit.collider.GetComponent<GridCell>();
            if (hitCell != null && !hasReachedTarget)
            {
                // Ensure that the target cell is only added if it's adjacent to the last cell in the path
                // and if the path already contains at least one cell
                if (hitCell.isTarget && path.Count > 0 && IsAdjacentToPreviousCell(hitCell))
                {
                    //Debug.Log("KAFA DURDU");
                    EventManager.Broadcast(GameEvent.OnHitTarget);
                    AddCellToPath(hitCell);
                    hasReachedTarget = true; // Prevent further path drawing
                    playerManager.counter++;
                    StartCoroutine(playerManager.CheckCounter());
                }
                else if (IsAdjacentToPreviousCell(hitCell) && !path.Contains(hitCell) && !hitCell.isTarget)
                {
                    //Debug.Log("KAFA STOp");
                    // Add the current cell to the path if it's adjacent to the previous cell
                    AddCellToPath(hitCell);
                }

                
            }
        }
    }

    private void AddCellToPath(GridCell cell)
    {
        path.Add(cell);
        EventManager.Broadcast(GameEvent.OnPathAdded);
        cell.players.Add(this);
        gridManager.HighlightCell(cell.row, cell.column, playerColor);
        playerData.selectedColor = playerColor;
        cell.cellTypes.Add(playerType);
    }

    private IEnumerator StarterAddCellToPath(GridCell cell)
    {
        yield return new WaitForSeconds(.5f);
        GetGridCellAtExactPosition(startPos,1);
    }

    private GridCell GetGridCellAtExactPosition(Vector3 position, float radius)
    {
        // Check for colliders within a small sphere around the position
        Collider[] colliders = Physics.OverlapSphere(position, radius);
        
        foreach (var collider in colliders)
        {
            GridCell cell = collider.GetComponent<GridCell>();
            if (cell != null)
            {
                // Verify the position is exactly the same
                if (Vector3.Distance(position, cell.transform.position) < 0.01f) // Adjust tolerance as needed
                {
                    path.Add(cell);
                    cell.players.Add(this);
                    gridManager.HighlightCell(cell.row, cell.column, playerColor);
                    playerData.selectedColor = playerColor;
                    cell.cellTypes.Add(playerType);
                }
            }
        }
        return null; // Return null if no GridCell was found
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

    internal void TeleportToTarget()
    {
        EventManager.Broadcast(GameEvent.OnHitTarget);
        hasReachedTarget = true; // Prevent further path drawing
        playerManager.counter++;
        StartCoroutine(playerManager.CheckCounter());
        isTeleport=true;
        path.Clear();
        tempPath.Clear();
        playerData.pathCompletedCounter++;
        EventManager.Broadcast(GameEvent.OnIncreaseScore);
        playerData.successPathCompletedCounter++;
        target.DOLocalMoveY(-1,0.2f);
        EventManager.Broadcast(GameEvent.OnPlayerPathComplete);
        transform.position=target.position;
        DOTween.Kill(transform);
        //Particle
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

    //1 ler aslinda 0di !!!!!!!
    void MovePlayerAlongPath()
    {
        if (path.Count > 1 && orderCell && !isTeleport)
        {
        // Move the player towards the first cell in the path
            GameObject targetCell = path[1].gameObject;
            orderCell=false;
            //transform.position = Vector3.MoveTowards(transform.position, targetCell.transform.position, moveSpeed * Time.deltaTime);
            
            //TREN OLSUN PLAYER
            transform.DOLookAt(targetCell.transform.position,0.1f);
            transform.DOJump(targetCell.transform.position,1,1,0.5f).OnComplete(()=>{
                EventManager.Broadcast(GameEvent.OnPlayerMove);
                path.RemoveAt(0);
                // If the path is now empty, the player has finished the path
                if (path.Count == 1)
                {
                    //Debug.Log(name + " FINISHED THE PATH CHECK IT");
                    playerData.pathCompletedCounter++;

                        // Check if the player has reached the final target position
                    if (Vector3.Distance(transform.position, target.position) < 0.01f)
                    {
                        //Debug.Log("SUCCESS PATH");
                        EventManager.Broadcast(GameEvent.OnIncreaseScore);
                        playerData.successPathCompletedCounter++;
                        target.DOLocalMoveY(-1,0.2f);
                    }
                    else
                    {
                        if(!gameData.isGameEnd && !gameData.isPlayerDead)
                        {
                            //Debug.Log("FAILLLLLL PATH");
                            EventManager.Broadcast(GameEvent.OnPlayerDead);    
                        }
                        
                    }
                        

                    EventManager.Broadcast(GameEvent.OnPlayerPathComplete);
                    //animator.SetBool("walk",false);
                }
                orderCell=true;
                
            });
            
        }

        
    }

    
}
