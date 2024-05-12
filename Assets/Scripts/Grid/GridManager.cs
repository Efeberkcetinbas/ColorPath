using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int numRows = 10;
    public int numColumns = 10;
    public GameObject cellPrefab;
    public Material defaultMaterial;
    public Material highlightMaterial;

    private GameObject[,] gridCells;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        gridCells = new GameObject[numRows, numColumns];

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                Vector3 cellPosition = new Vector3(col, 0, row);
                GameObject cell = Instantiate(cellPrefab, cellPosition, Quaternion.identity);
                cell.transform.localScale = Vector3.one;
                cell.transform.parent = transform; // Set the GridManager as the parent

                GridCell gridCellComponent = cell.GetComponent<GridCell>();
                gridCellComponent.row = row;
                gridCellComponent.column = col;

                gridCells[row, col] = cell;
            }
        }
    }

    // Highlight a specific cell in the grid
    public void HighlightCell(int row, int col,Color color)
    {
        if (row >= 0 && row < numRows && col >= 0 && col < numColumns)
        {
            GameObject cell = gridCells[row, col];
            Renderer renderer = cell.GetComponent<Renderer>();
            //renderer.material = highlightMaterial;
            if(renderer!=null)
                renderer.material.color=color;
            else
                Debug.LogWarning("WARNING");
            
            
        }
    }

    // Reset the material of a specific cell in the grid
    public void ResetCellMaterial(int row, int col)
    {
        if (row >= 0 && row < numRows && col >= 0 && col < numColumns)
        {
            GameObject cell = gridCells[row, col];
            Renderer renderer = cell.GetComponent<Renderer>();
            renderer.material = defaultMaterial;
        }
    }

    // Reset the material of all cells in the grid
    public void ResetHighlightedCells()
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                ResetCellMaterial(row, col);
            }
        }
    }

    

   
}
