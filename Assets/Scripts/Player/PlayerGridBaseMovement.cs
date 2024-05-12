using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGridBaseMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private List<GridCell> path = new List<GridCell>(); // List to store the movement path
    private int currentPathIndex = 0; // Index of the current cell in the path

    void Update()
    {
        if (path.Count > 0)
        {
            // Move towards the current cell in the path
            transform.position = Vector3.MoveTowards(transform.position, path[currentPathIndex].transform.position, moveSpeed * Time.deltaTime);

            // If reached the current cell, move to the next one
            if (Vector3.Distance(transform.position, path[currentPathIndex].transform.position) < 0.01f)
            {
                currentPathIndex++;

                // Check if reached the end of the path
                if (currentPathIndex >= path.Count)
                {
                    path.Clear();
                    currentPathIndex = 0;
                }
            }
        }
    }

    // Set the movement path for the player
    public void SetPath(List<GridCell> newPath)
    {
        path = newPath;
        currentPathIndex = 0;
    }
}
