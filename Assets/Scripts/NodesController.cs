//using Palmmedia.ReportGenerator.Core.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodesController : MonoBehaviour
{
    public GameManager gameManager;

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
            updateCellOnTop(cellOnTop);
        }

    }

    void Start()
    {
        setCellPositions();
    }

    // Update is called once per frame
    void Update()
    {


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


}
