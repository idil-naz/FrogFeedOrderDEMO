using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Script Description:
    //This script helps the Nodes keep and update information about themselves and the cells that they are housing.
    //It also initializes the cells positions according to how many cells it has (all the cells are children of the respective node)
    //It also keeps and updates the information about its top cell.


public class NodesController : MonoBehaviour
{
    public List<GameObject> cells;
    public GameObject cellOnTop;

    public int xCoordinate, yCoordinate;
    public NodesController leftNode, rightNode, frontNode, backNode;

    public GameGridController gameGrid;

    private void Awake()
    {
        string[] coordinates = name.Split('x');
        xCoordinate = int.Parse(coordinates[0]);
        yCoordinate = int.Parse(coordinates[1]);

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            cells.Add(gameObject.transform.GetChild(i).gameObject);

            cellOnTop = gameObject.transform.GetChild(transform.childCount - 1).gameObject;
        }

        setCellPositions();
        updateCellOnTop(cellOnTop);
    }

    void Start()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].GetComponent<CellController>() != null)
            {
                cells[i].GetComponent<CellController>().checkSelf();
            }
        }
    }

    public void updateCellOnTop(GameObject newCellonTop)
    {
        cellOnTop = newCellonTop; 
    }

    public GameObject getCellOnTop() {

        return cellOnTop;
    }

    void setCellPositions()
    {
        for (int i = 1; i < gameObject.transform.childCount; i++)
        {
            cells[i].transform.localPosition += new Vector3(cells[i].transform.localPosition.x, i * 0.12f, cells[i].transform.localPosition.z);
        }
    }

    public void updateCellOnTop()
    {
        if(cells.Count > 1)
        {
            cells.Remove(cells[cells.Count - 1]);
            cellOnTop = cells[cells.Count - 1];
        }
        else
        {
            cellOnTop = null;
        }
    }
}
