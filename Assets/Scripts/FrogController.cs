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
    public GameObject frogParentCell;
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
    public List<GameObject> berriesOnPath;

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

    private bool frogDestroyed = false;

    private void Awake()
    {
        
    }
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
        frogParentCell = gameObject.transform.parent.gameObject;
        frogParentNode = frogParentCell.GetComponent<CellController>().cellParentNode.gameObject;

        currentNode = frogParentNode.GetComponent<NodesController>();
        pathNodes.Add(frogParentNode);

    }

    private void OnMouseDown()
    {
        //Debug.Log("FrogClicked");
        if (!isMoving)
        {
            StartCoroutine(StartPathing());
            //gameObject.GetComponent<Collider>().enabled = false;
        }

    }

    private void Update()
    {
        if (!frogDestroyed && frogTounge.GetComponent<FrogToungeScript>().berryCollectionCompleted)
        {
            frogDestroyed = true;
            DestroyFrog();
        }
    }

    IEnumerator StartPathing()
    {
        isMoving = true;
        gameObject.GetComponent<Collider>().enabled = false;
        //frogTounge.GetComponent<FrogToungeScript>().pathingCompleted = false;

        while (isMoving)
        {

            if (getNextNode(currentDirection) != null && getNextNode(currentDirection).GetComponent<NodesController>().cellOnTop != null)
            {
                NodesController nextNode = getNextNode(currentDirection);
                //Debug.Log("CURRENT NODE: " + currentNode.name + " " + "//" + " " + "NEXT NODE IN DIRECTION: " + nextNode.name + " " + "//" + " " + "CURRENT DIRECTION: " + currentDirection);

                if (nextNode.getCellOnTop().GetComponent<CellController>().cellColor == currentNode.GetComponent<NodesController>().cellOnTop.GetComponent<CellController>().cellColor && !pathNodes.Contains(nextNode.gameObject))
                {
                    //COROUTINE CALL
                    StartCoroutine(frogTounge.GetComponent<FrogToungeScript>().ToungeAnim());

                    //Debug.Log("TOP CELL COLORS MATCH. NEXT NODE IN PATH: " + nextNode.name);
                    currentNode = nextNode;

                    GameObject objectOnNextCell = currentNode.getCellOnTop().GetComponent<CellController>().housedGameObject;

                    //Debug.Log("NEW CURRENT NODE: " + currentNode.name);
                    pathNodes.Add(currentNode.gameObject);

                    if (objectOnNextCell.CompareTag("Grape"))
                    {
                        berriesOnPath.Add(objectOnNextCell);
                        //Debug.Log("[COLLECTING GRAPE]");
                        //LEAN TWEEN ANIMATIONS
                        LeanTween.scale(objectOnNextCell, Vector3.one * 1.3f, 0.1f).setEase(LeanTweenType.easeOutBounce)
                                .setOnComplete(() =>
                                {
                                    LeanTween.scale(objectOnNextCell, Vector3.one, 0.1f).setEase(LeanTweenType.easeInBounce);
                                });
                    }
                    if (objectOnNextCell.CompareTag("Arrow"))
                    {
                        //Debug.Log("CHANGING DIRECTION");
                        currentDirection = (Direction)objectOnNextCell.GetComponent<ArrowController>().direction;
                        //Debug.Log("NEW CURRENT DIRECTION: " + currentDirection);
                    }

                }
                else
                {
                    //Debug.Log("TOP CELL COLORS DO NOT MATCH. RETRACTING");
                    currentDirection = (Direction)frogDirection;
                    currentNode = frogParentNode.GetComponent<NodesController>();

                    this.gameObject.GetComponent<Collider>().enabled = true;
                    isMoving = false;
                    currentNode = gameObject.transform.parent.transform.parent.GetComponent<NodesController>();
                    //frogTounge.GetComponent<FrogToungeScript>().pathingCompleted = true;

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
                //Debug.Log("END OF PATH. COLLECTING BERRIES");

                gameObject.GetComponent<Collider>().enabled = true;
                isMoving = false;
                //frogTounge.GetComponent<FrogToungeScript>().pathingCompleted = true;

                currentNode = gameObject.transform.parent.transform.parent.GetComponent<NodesController>();
                currentDirection = (Direction)frogDirection;

                break;
            }

        }

        /*THE LOGIC*/
        //Move in the direction of the Frog's direction.
        //Check the NODE that is in the direction of the frog. Check the TOP CELL in that node. If it is the same color with the CURRENT CELL,
        //check the HOUSED OBJECT of the TOP CELL.

        //If the housed object is a berry, collect it and move to the next NODE in the direction of the frog. Check the TOP CELL of that NODE.
        //IF the housed object in an arrow, collect it and move to the next NODE in the direction of the arrow. Check the TOP CELL of that NODE.

        //Continue with the same logic.

        gameObject.GetComponent<Collider>().enabled = true;
        isMoving = false;
    }


    NodesController getNextNode(Direction direction)
    {
        if (direction == Direction.Up && currentNode.frontNode != null) return currentNode.GetComponent<NodesController>().frontNode;

        else if (direction == Direction.Down && currentNode.backNode != null) return currentNode.GetComponent<NodesController>().backNode;

        else if (direction == Direction.Left && currentNode.leftNode != null) return currentNode.GetComponent<NodesController>().leftNode;

        else if (direction == Direction.Right && currentNode.rightNode != null) return currentNode.GetComponent<NodesController>().rightNode;

        else return null;
    }


    public void DestroyFrog()
    {
        gameObject.transform.parent.transform.parent = null;

        if (!LeanTween.isTweening(gameObject))
        {
            LeanTween.cancel(gameObject);
        }

        LeanTween.scale(gameObject.transform.parent.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
            {
                if (!LeanTween.isTweening(gameObject.transform.parent.gameObject.gameObject))
                {
                    frogParentNode.GetComponent<NodesController>().updateCellOnTop();

                    if (frogParentNode.GetComponent<NodesController>().cellOnTop.GetComponent<CellController>())
                    {
                        frogParentNode.GetComponent<NodesController>().cellOnTop.GetComponent<CellController>().checkSelf();
                    }

                    Destroy(gameObject.transform.parent.transform.gameObject);
                }
            });
        }
}
