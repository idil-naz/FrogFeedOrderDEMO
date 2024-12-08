using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            //Debug.Log("node: " + node.name);

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
                //Debug.Log("found node: " + node.name + "at" + x + "," + "y");
                return nodeController;
            }
        }
        return null; //if no node was found at target location
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
