using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] private List<GridCell> path = new List<GridCell>(); // List to store the movement path
    [SerializeField] private List<Vector3> tempPath=new List<Vector3>();
    
    private bool isDragging = false; // Flag to track if the user is currently dragging

    [SerializeField] private PathData pathData;
    [SerializeField] private Color playerColor; // Color assigned to this player
    [SerializeField] private GridManager gridManager;

    private Renderer playerRenderer; // Renderer component to apply color to the player

    public bool isMe=false;
    public bool canCountOnMe=true;
    public CellType playerType;

    void Start()
    {
        playerRenderer = GetComponent<Renderer>(); // Get the Renderer component
        playerRenderer.material.color = playerColor; // Set the player's color
    }

    void Update()
    {
         // Check for touch input
        if (isMe)
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
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                hitCell.players.Add(this);
                //hitCell.cellColor=playerColor;
                gridManager.HighlightCell(hitCell.row,hitCell.column,playerColor);
                hitCell.cellTypes.Add(playerType);
                //Debug.Log("CELL COLOR  : " + hitCell.cellColor);
                //Debug.Log("PLAYER COLOR  : " + playerColor);
                // Highlight the grid cell with the player's color
                //hitCell.Highlight(playerColor);
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


        

        // Reset highlight for all cells
        /*foreach (GridCell cell in path)
        {
            cell.ResetHighlight();
        }*/
        // Optionally, you can perform additional actions when the dragging ends
        // For example, you can trigger the player to move along the collected path
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
        // Move the player along the collected path
        if (path.Count > 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[1].transform.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, path[1].transform.position) < 0.01f)
            {
                // Remove the current cell from the path once reached
                path.RemoveAt(0);
            }
        }
        else if (path.Count == 1)
        {
            // Move directly to the last cell if it's the only cell in the path
            transform.position = Vector3.MoveTowards(transform.position, path[0].transform.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, path[0].transform.position) < 0.01f)
            {
                // Clear the path once reached the last cell
                path.Clear();
            }
        }
    }
}
