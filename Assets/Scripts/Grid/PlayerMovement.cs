using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private List<GridCell> path = new List<GridCell>(); // List to store the movement path
    private bool isDragging = false; // Flag to track if the user is currently dragging

    [SerializeField] private PathData pathData;


    

    void Update()
    {
        // Check for touch input
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

        // Move the player along the path if it's not empty
        if(pathData.playersCanMove)
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

    void ContinueDragging(Vector2 touchPosition)
    {
        // Perform a raycast to detect the grid cell the user is currently dragging over
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        
        if (Physics.Raycast(ray, out hit))
        {
            GridCell hitCell = hit.collider.GetComponent<GridCell>();
            if (hitCell != null && IsAdjacentToPreviousCell(hitCell))
            {
                // Add the current cell to the path if it's adjacent to the previous cell
                path.Add(hitCell);
            }
        }
    }

    void EndDragging()
    {
        isDragging = false;
        // Optionally, you can perform additional actions when the dragging ends
        // For example, you can trigger the player to move along the collected path
    }

    bool IsAdjacentToPreviousCell(GridCell cell)
    {
        if (path.Count > 0)
        {
            GridCell lastCell = path[path.Count - 1];
            // Cells are adjacent if the difference in row or column is at most 1
            return Mathf.Abs(lastCell.row - cell.row) <= 1 && Mathf.Abs(lastCell.column - cell.column) <= 1;
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
