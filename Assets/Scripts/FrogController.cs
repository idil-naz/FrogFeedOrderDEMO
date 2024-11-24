using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ArrowController;
using static CellController;

public class FrogController : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject frogParentNode;
    public enum FrogDirection
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    }
    public FrogDirection frogDirection;

    
    //FOR PATHING
    public NodesController currentNode;
    public enum Direction
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    }
    public Direction currentDirection;

    public bool isMoving = false;


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
        frogParentNode = gameObject.transform.parent.transform.parent.gameObject;
        currentNode = frogParentNode.GetComponent<NodesController>();

       

    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void OnMouseDown()
    {
        Debug.Log("FrogClicked");
        if (!isMoving)
        {
            //StartCoroutine(frogTounge.GetComponent<FrogToungeScript>().ToungeAnim());
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
                Debug.Log("current node: " + currentNode.name);
                Debug.Log("next node: " + nextNode.name);
                Debug.Log("current direction: " + currentDirection);

                if (nextNode.getCellOnTop().GetComponent<CellController>().cellColor == currentNode.GetComponent<NodesController>().cellOnTop.GetComponent<CellController>().cellColor)
                {
                    Debug.Log("top cell colors match..." + "next node in path: " + nextNode.name);
                    currentNode = nextNode;

                    GameObject objectOnNextCell = currentNode.getCellOnTop().GetComponent<CellController>().housedGameObject;

                    Debug.Log("new current node: " + currentNode.name);

                    //COROUTINE CALL
                    StartCoroutine(frogTounge.GetComponent<FrogToungeScript>().ToungeAnim());

                    if (objectOnNextCell.CompareTag("Grape"))
                    {
                        Debug.Log("object type on the next cell is a grape..collecting grape");
                        
                        //COROUTINE CALL
                        //StartCoroutine(frogTounge.GetComponent<FrogToungeScript>().ToungeAnim());


                    }
                    if (objectOnNextCell.CompareTag("Arrow"))
                    {
                        Debug.Log("object type on the next cell is an arrow..changing direction");
                        currentDirection = (Direction)objectOnNextCell.GetComponent<ArrowController>().direction;
                        Debug.Log("current direction: " + currentDirection);

                        //COROUTINE CALL
                        //StartCoroutine(frogTounge.GetComponent<FrogToungeScript>().ToungeAnim());

                    }

                }
                else
                {
                    Debug.Log("color of the next cell does not match the color of the current cell. cannot form path. retracting...");
                    currentDirection = (Direction)frogDirection;
                    currentNode = frogParentNode.GetComponent<NodesController>();
                    isMoving = false;
                    break;
                    //current node goes back to being the frog's node.
                }

                isMoving = false;
                yield return new WaitForSeconds(1f);
                yield return StartCoroutine(StartPathing());
            }
            else
            {
                Debug.Log("No node found at target direction. You have hit a wall. Collecting berries....");
                //Start collecting berries
                gameObject.GetComponent<Collider>().enabled = true;
                isMoving = false;
                break;
            }
            
            //gameObject.GetComponent<Collider>().enabled = true;
            
        }
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
