using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesController : MonoBehaviour
{
    public int numberOfCells;
    public GameManager gameManager;

    public List<GameObject> cells;
    void Start()
    {
        Vector3 targetPos = gameObject.transform.position;

        for (int i = 0; i < numberOfCells; i++)
        {
            cells.Add(Instantiate(gameManager.CellPrefab, targetPos, Quaternion.identity, this.gameObject.transform));
        }

        for (int i = 1; i < numberOfCells; i++)
        {
            cells[i].transform.localPosition += new Vector3(cells[i].transform.localPosition.x,i*0.13f, cells[i].transform.localPosition.z);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
