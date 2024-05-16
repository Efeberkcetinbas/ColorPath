using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridManager : MonoBehaviour
{
    public int numRows = 10;
    public int numColumns = 10;
    public GameObject cellPrefab;
    public Material defaultMaterial;
    public Material highlightMaterial;

    [SerializeField] private GameObject[,] gridCells;
    [SerializeField] private List<GameObject> cells=new List<GameObject>();

    void Start()
    {
        InitializeGridCells();
        GenerateGrid();
    }
    void InitializeGridCells()
    {
        // Initialize the 2D array to the correct size
        gridCells = new GameObject[numRows, numColumns];
    }

    void GenerateGrid()
    {
        if (cells.Count != numRows * numColumns)
        {
            Debug.Log(cells.Count + " / " + numRows*numColumns + "");
            Debug.LogError("The number of cells provided does not match the grid dimensions.");
            return;
        }

        int cellIndex = 0; // Index to iterate over the cells list

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                GameObject cell = cells[cellIndex];
                cellIndex++;

                // Set cell position and parent (optional)
                Vector3 cellPosition = new Vector3(col, 0, row);
                cell.transform.position = cellPosition;
                cell.transform.parent = transform;

                // Assign the cell to the grid array
                gridCells[row, col] = cell;

                // Set row and column in GridCell component
                GridCell gridCellComponent = cell.GetComponent<GridCell>();
                if (gridCellComponent != null)
                {
                    gridCellComponent.row = row;
                    gridCellComponent.column = col;
                }
                else
                {
                    Debug.LogWarning("GridCell component not found on cell at index " + cellIndex);
                }
            }
        }
    }

    // Highlight a specific cell in the grid
    public void HighlightCell(int row, int col,Color color)
    {
        if (row >= 0 && row < numRows && col >= 0 && col < numColumns)
        {
            GameObject cell = gridCells[row, col];
            Renderer renderer = cell.transform.GetChild(0).GetComponent<Renderer>();
            //renderer.material = highlightMaterial;
            if(renderer!=null)
            {
                cell.GetComponent<GridCell>().cellColors.Add(color);
                renderer.material.DOColor(cell.GetComponent<GridCell>().cellColors[cell.GetComponent<GridCell>().cellColors.Count-1],.5f);
                
            }
                
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
            Renderer renderer = cell.transform.GetChild(0).GetComponent<Renderer>();
            
            cell.GetComponent<GridCell>().cellColors.RemoveAt(cell.GetComponent<GridCell>().cellColors.Count-1);

            if(cell.GetComponent<GridCell>().cellColors.Count!=0)
                renderer.material.color=cell.GetComponent<GridCell>().cellColors[cell.GetComponent<GridCell>().cellColors.Count-1];
            else
                renderer.material = defaultMaterial;

        }
    }

    
    

   
}
