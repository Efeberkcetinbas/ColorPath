using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    None,
    Yellow,
    Red,
    Blue,
    Green,
    Purple,
    Aqua,
    Orange,
    Pink
}
public class GridCell : MonoBehaviour
{
    public int row;
    public int column;

    private GridManager gridManager;
    private bool isTouching = false;
    [SerializeField] private bool isOne=false;
    //Bunu daha sonra array olarak tutki ust uste gectiklerinde renkler degisime ugrasin
    internal Color cellColor;

    public List<PlayerMovement> players=new List<PlayerMovement>();
    public List<CellType> cellTypes=new List<CellType>();

    void Start()
    {
        gridManager = GetComponentInParent<GridManager>();
    }

    /*void Update()
    {
        if (isTouching)
        {
            // Handle touch input
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out hit))
                    {
                        GridCell otherCell = hit.collider.GetComponent<GridCell>();
                        if (otherCell != null && (otherCell.row != row || otherCell.column != column))
                        {
                            //gridManager.HighlightCell(otherCell.row, otherCell.column,cellColor);
                            
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    //gridManager.ResetCellMaterial(row, column);
                }
            }
        }
    }*/

    void OnTriggerEnter(Collider other)
    {
        isTouching = true;
        if(other.CompareTag("Player"))
        {
            Debug.Log("ENTER PLAYER");
        }
    }

    void OnTriggerExit(Collider other)
    {
        isTouching = false;

        if(other.CompareTag("Player"))
        {
            //Eger altinda baska bir renk yoksa
            gridManager.ResetCellMaterial(row, column);
            /*if(players.Count!=0)
            {
                players.RemoveAt(players.Count-1);

            }*/
            
        }
    }
}
