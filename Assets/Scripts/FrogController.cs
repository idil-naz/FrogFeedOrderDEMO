using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ArrowController;
using static CellController;

public class FrogController : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject frogParentNode;
    public enum FrogDirection
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    }
    public FrogDirection frogDirection;
    
    //PATHING
    public NodesController currentNode;
    public List<GameObject> pathNodes;

    public enum Direction
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    }
    public Direction currentDirection;

    public float pathingExecutionTime = 0.25f;
    private bool isMoving = false;

    //FROG TOUNGE
    public LineRenderer frogTounge;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        Renderer renderer = gameObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        renderer.material = gameManager.frogMaterials[((int)gameObject.transform.parent.GetComponent<CellController>().cellColor)];

        switch (frogDirection)
        {
            case FrogDirection.Up:
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 180, gameObject.transform.eulerAngles.z);
                break;
            case FrogDirection.Down:
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 0, gameObject.transform.eulerAngles.z);
                break;
            case FrogDirection.Left:
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 90, gameObject.transform.eulerAngles.z);
                break;
            case FrogDirection.Right:
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 270, gameObject.transform.eulerAngles.z);
                break;
        }

        //PATHING
        currentDirection = (Direction)frogDirection;
        frogParentNode = gameObject.transform.parent.gameObject.GetComponent<CellController>().cellParentNode;
        currentNode = frogParentNode.GetComponent<NodesController>();
        pathNodes.Add(frogParentNode);

    }

    private void OnMouseDown()
    {
        Debug.Log("FrogClicked");
        if (!isMoving)
        {
            StartCoroutine(StartPathing());
            gameObject.GetComponent<Collider>().enabled = false;
        }

    }

    IEnumerator StartPathing()
    {
        isMoving = true;

        while (isMoving)
        {
            if(getNextNode(currentDirection) != null)
            {
                NodesController nextNode = getNextNode(currentDirection);
                Debug.Log("CURRENT NODE: " + currentNode.name + " " + "//" + " " + "NEXT NODE IN DIRECTION: " + nextNode.name + " " + "//" + " " + "CURRENT DIRECTION: " + currentDirection);

                if (nextNode.getCellOnTop().GetComponent<CellController>().cellColor == currentNode.GetComponent<NodesController>().cellOnTop.GetComponent<CellController>().cellColor)
                {
                    Debug.Log("TOP CELL COLORS MATCH. NEXT NODE IN PATH: " + nextNode.name);
                    currentNode = nextNode;

                    GameObject objectOnNextCell = currentNode.getCellOnTop().GetComponent<CellController>().housedGameObject;

                    Debug.Log("NEW CURRENT NODE: " + currentNode.name);

                    //COROUTINE CALL
                    StartCoroutine(frogTounge.GetComponent<FrogToungeScript>().ToungeAnim());
                    pathNodes.Add(currentNode.gameObject);

                    if (objectOnNextCell.CompareTag("Grape"))
                    {
                        Debug.Log("[COLLECTING GRAPE]");
                    }
                    if (objectOnNextCell.CompareTag("Arrow"))
                    {
                        Debug.Log("CHANGING DIRECTION");
                        currentDirection = (Direction)objectOnNextCell.GetComponent<ArrowController>().direction;
                        Debug.Log("NEW CURRENT DIRECTION: " + currentDirection);
                    }

                }
                else
                {
                    Debug.Log("TOP CELL COLORS DO NOT MATCH. RETRACTING");
                    currentDirection = (Direction)frogDirection;
                    currentNode = frogParentNode.GetComponent<NodesController>();
                    isMoving = false;
                    break;
                    //current node goes back to being the frog's node.
                }

                isMoving = false;
                yield return new WaitForSeconds(pathingExecutionTime);
                yield return StartCoroutine(StartPathing());
            }
            else
            {
                
                //Start collecting berries

                yield return new WaitForSeconds(1f);
                Debug.Log("END OF PATH. COLLECTING BERRIES");
                StartCoroutine(frogTounge.GetComponent<FrogToungeScript>().CollectBerries());


                gameObject.GetComponent<Collider>().enabled = true;
                isMoving = false;
                break;
            }
            
            //gameObject.GetComponent<Collider>().enabled = true;
            
        }

        /*THE LOGIC*/
        //Move in the direction of the Frog's direction.
        //Check the NODE that is in the direction of the frog. Check the TOP CELL in that node. If it is the same color with the CURRENT CELL,
        //check the HOUSED OBJECT of the TOP CELL.

        //If the housed object is a berry, collect it and move to the next NODE in the direction of the frog. Check the TOP CELL of that NODE.
        //IF the housed object in an arrow, collect it and move to the next NODE in the direction of the arrow. Check the TOP CELL of that NODE.

        //Continue with the same logic.
    }


    NodesController getNextNode(Direction direction)
    {
        if (direction == Direction.Up && currentNode.frontNode != null) return currentNode.GetComponent<NodesController>().frontNode;

        else if (direction == Direction.Down && currentNode.backNode != null) return currentNode.GetComponent<NodesController>().backNode;

        else if (direction == Direction.Left && currentNode.leftNode != null) return currentNode.GetComponent<NodesController>().leftNode;

        else if (direction == Direction.Right && currentNode.rightNode != null) return currentNode.GetComponent<NodesController>().rightNode;

        else return null;
    }
}
