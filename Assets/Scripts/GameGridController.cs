using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script Description:
    //This script is responsible for basic initilization operations in the 6x6 grid.
    //It passes information to all the nodes about their neighboring nodes (nodes on the left, right, front, and back of themselves)
    //It also checks whether all the cells in each node have been cleared which is a condition for level completion.

public class GameGridController : MonoBehaviour
{
    public List<GameObject> allNodes;
    public GameObject nodesParentHolder;

    private void Awake()
    {
        for (int i = 0; i< nodesParentHolder.transform.childCount; i++)
        {
            allNodes.Add(nodesParentHolder.transform.GetChild(i).gameObject);
            allNodes[i].GetComponent<NodesController>().gameGrid = this.gameObject.GetComponent<GameGridController>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        assignNeighbors();
}

    void assignNeighbors()
    {
        foreach (GameObject node in allNodes)
        {
            NodesController currentNode = node.GetComponent<NodesController>();

            currentNode.leftNode = findNode(currentNode.xCoordinate, currentNode.yCoordinate - 1);
            currentNode.rightNode = findNode(currentNode.xCoordinate, currentNode.yCoordinate + 1);
            currentNode.frontNode = findNode(currentNode.xCoordinate + 1, currentNode.yCoordinate);
            currentNode.backNode = findNode(currentNode.xCoordinate - 1, currentNode.yCoordinate);
        }
    }

    public NodesController findNode(int x, int y)
    {
        foreach(GameObject node in allNodes)
        {
            NodesController nodeController = node.GetComponent<NodesController>();

            if(nodeController.xCoordinate == x && nodeController.yCoordinate == y)
            {
                return nodeController;
            }
        }
        return null;
    }

    public bool allCellsCleared()
    {
        foreach (GameObject node in allNodes)
        {
            var nodeController = node.GetComponent<NodesController>();
            if (nodeController != null && nodeController.cells.Count != 1)
            {
                return false;
            }
        }
        return true;
    }

}
