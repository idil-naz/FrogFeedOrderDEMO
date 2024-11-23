using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesController : MonoBehaviour
{
    public GameManager gameManager;

    public List<GameObject> cells;
    public GameObject cellOnTop;

    void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            cells.Add(gameObject.transform.GetChild(i).gameObject);
            //cells[i].gameObject.AddComponent<CellController>();

        }

        for (int i = 1; i < gameObject.transform.childCount; i++)
        {
            cells[i].transform.localPosition += new Vector3(cells[i].transform.localPosition.x,i*0.13f, cells[i].transform.localPosition.z);


        }

        cellOnTop = gameObject.transform.GetChild(transform.childCount - 1).gameObject;
        updateCellOnTop(cellOnTop);
    }
    // Update is called once per frame
    void Update()
    {


    }

    public void updateCellOnTop(GameObject newCellonTop)
    {
        cellOnTop = newCellonTop;
    }
}
