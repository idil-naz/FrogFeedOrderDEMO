using System.Collections;
using System.Collections.Generic;
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

   /*
    FOR PATHING
    
    */
    public enum Direction
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    }
    public Direction currentDirection;

    public bool isMoving = false;

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

        currentDirection = (Direction)frogDirection;

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
            StartCoroutine(StartPathing());
        }

    }

    IEnumerator StartPathing()
    {
        isMoving = true;

        while (isMoving)
        {
            NodesController currentNode = frogParentNode.GetComponent<NodesController>();
            NodesController nextNode = getNextNode(currentDirection);

            Debug.Log("current node: " + currentNode.name);

            if(nextNode.getCellOnTop().GetComponent<CellController>().cellColor == frogParentNode.GetComponent<NodesController>().cellOnTop.GetComponent<CellController>().cellColor)
            {
                Debug.Log("top cell colors match..." + "next node in path: " + nextNode.name);

                currentNode = nextNode;
                GameObject objectOnNextCell = currentNode.getCellOnTop().GetComponent<CellController>().housedGameObject;

                Debug.Log("new current node: " + currentNode.name);


                if (objectOnNextCell.CompareTag("Grape"))
                {
                    Debug.Log("object type on the next cell is a grape..collecting grape");
                    
                }
                if (objectOnNextCell.CompareTag("Arrow"))
                {
                    Debug.Log("object type on the next cell is an arrow..changing direction");

                }



            }
            else
            {
                Debug.Log("color of the next cell does not match the color of the current cell. cannot form path. retracting...");
            }





            isMoving = false;
            yield return new WaitForSeconds(0.05f);
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
        switch (direction)
        {
            case Direction.Up:
                return frogParentNode.GetComponent<NodesController>().frontNode;
            case Direction.Down:
                return frogParentNode.GetComponent<NodesController>().backNode;
            case Direction.Left:
                return frogParentNode.GetComponent<NodesController>().leftNode;
            case Direction.Right:
                return frogParentNode.GetComponent<NodesController>().rightNode;
            default:
                return null;
        }
        
    }

    NodesController getNextNode(FrogDirection direction)
    {
        switch (direction)
        {
            case FrogDirection.Up:
                return frogParentNode.GetComponent<NodesController>().frontNode;
            case FrogDirection.Down:
                return frogParentNode.GetComponent<NodesController>().backNode;
            case FrogDirection.Left:
                return frogParentNode.GetComponent<NodesController>().leftNode;
            case FrogDirection.Right:
                return frogParentNode.GetComponent<NodesController>().rightNode;
            default:
                return null;
        }

    }

}
