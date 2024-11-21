using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGridController : MonoBehaviour
{
    
    //private GameManager gameManager;
    public List<GameObject> nodesList;

    // Start is called before the first frame update
    void Start()
    {
        /*for (int i = 0; i < gridLevels.transform.childCount; i++)
        {
            gridLevelsList.Add(gridLevels.transform.GetChild(i).gameObject);
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void initializeGrid()
    {
        for (int i = 0; i < gridLevelsList.Count; i++)
        {
            for (int j = 0; j < gridLevelsList[i].GetComponent<GridLevelController>().gridLevelNodes.Count; j++)
            {
                Instantiate(gameManager.CellPrefab, gridLevelsList[i].GetComponent<GridLevelController>().gridLevelNodes[j].transform.position, Quaternion.identity/*, gridLevelsList[i].GetComponent<GridLevelController>().gridLevelNodes[j].transform);
                
            }
        }
    }*/
}
