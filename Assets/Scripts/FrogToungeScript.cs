using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FrogToungeScript : MonoBehaviour
{
    private float duration;

    public FrogController parentFrog;
    private LineRenderer frogTounge;
    private List<Vector3> points;


    // Start is called before the first frame update
    void Start()
    {
        parentFrog = gameObject.transform.parent.GetComponent<FrogController>();
        duration = parentFrog.pathingExecutionTime;


        frogTounge = GetComponent<LineRenderer>();
        points = new List<Vector3>();

        Vector3 initPos = new Vector3(

                parentFrog.transform.position.x,
                parentFrog.transform.position.y + 0.15f * parentFrog.frogParentNode.GetComponent<NodesController>().cells.IndexOf(parentFrog.transform.parent.gameObject),
                parentFrog.transform.position.z
            );

        points.Add(initPos);
        frogTounge.positionCount = points.Count;
        frogTounge.SetPosition(0, initPos);

        //Collider toungeCollider = frogTounge.GetComponent<Collider>();
        //toungeCollider.transform.localPosition = points[0];

    }


    public IEnumerator CollectBerries()
    {
        //Debug.Log("CollectBerries Called");

        /*THE NEW LOGIC*/
        //-each nodes in the path have a certain distance to the first element in the list in terms of number of nodes(i.e the third node is 2 nodes away rom the first node)
        //- for each node in the path list, a loop will iterate for the distance between nodes, in each iteration, the housed object will move one node closer to the first node.
        //- when all the housed objects are on the first node, the function terminates

        for (int i = 1; i < parentFrog.pathNodes.Count; i++)
        {
            GameObject currentCollectNode = parentFrog.pathNodes[i];
            GameObject targetCollectNode = parentFrog.pathNodes[0];

            GameObject currentBerry = currentCollectNode.GetComponent<NodesController>().cellOnTop.GetComponent<CellController>().housedGameObject;

            //Debug.Log(parentFrog.pathNodes[i]);

            if (currentBerry.CompareTag("Grape"))
            {
                
                int distanceToFirstNode = i;
                //Debug.Log("Distance of " + currentBerry.name + " to the first node: " + distanceToFirstNode);

                //Debug.Log("NEIGHBORS INFO");
                //Debug.Log("Current collection node: " + currentCollectNode);
                //Debug.Log("Current collection node's target: " + targetCollectNode);
                //Debug.Log("Left of this node: " + currentCollectNode.GetComponent<NodesController>().leftNode);
                //Debug.Log("Right of this node: " + currentCollectNode.GetComponent<NodesController>().rightNode);
                //Debug.Log("Front of this node: " + currentCollectNode.GetComponent<NodesController>().frontNode);
                //Debug.Log("Back of this node: " + currentCollectNode.GetComponent<NodesController>().backNode);

                for (int j = 0; j < distanceToFirstNode; j++)
                {
                    bool isMoved = false;

                    var frontNode = currentCollectNode.GetComponent<NodesController>().frontNode;
                    if (frontNode != null && frontNode.gameObject == parentFrog.pathNodes[i-1])
                    {
                        Debug.Log("Move Up");
                        currentBerry.GetComponent<BerryController>().MoveUp();
                    }

                    var backNode = currentCollectNode.GetComponent<NodesController>().backNode;
                    if (backNode != null && backNode.gameObject == parentFrog.pathNodes[i - 1])
                    {
                        Debug.Log("Move Down");
                        currentBerry.GetComponent<BerryController>().MoveDown();
                    }

                    var leftNode = currentCollectNode.GetComponent<NodesController>().leftNode;
                    if (leftNode != null && leftNode.gameObject == parentFrog.pathNodes[i - 1])
                    {
                        Debug.Log("Move Down");
                        currentBerry.GetComponent<BerryController>().MoveLeft();
                    }

                    var rightNode = currentCollectNode.GetComponent<NodesController>().rightNode;
                    if (rightNode != null && rightNode.gameObject == parentFrog.pathNodes[i - 1])
                    {
                        Debug.Log("Move Down");
                        currentBerry.GetComponent<BerryController>().MoveRight();
                    }

                    if(isMoved)
                    {
                        yield return new WaitForSeconds(0.5f);
                    }
                    isMoved = false;

                }

            }
           

        }
        Debug.Log("COLLECTION COMPLETE");
        yield return null;
    }

    public IEnumerator ToungeAnim()
    {
        Vector3 startingPos = points[points.Count - 1];
        Vector3 endingPos;

        switch (parentFrog.currentDirection)
        {
            case FrogController.Direction.Up:
                endingPos = new Vector3(
                    startingPos.x,
                    startingPos.y,
                    startingPos.z + 1f
                    );
                break;

            case FrogController.Direction.Down:
                endingPos = new Vector3(
                    startingPos.x,
                    startingPos.y,
                    startingPos.z - 1f
                    );
                break;

            case FrogController.Direction.Left:
                endingPos = new Vector3(
                    startingPos.x - 1f,
                    startingPos.y,
                    startingPos.z
                    );
                break;

            case FrogController.Direction.Right:
                endingPos = new Vector3(
                    startingPos.x + 1f,
                    startingPos.y,
                    startingPos.z
                    );
                    break;
            default:
                endingPos = startingPos;
                break;

        }


        points.Add(endingPos);

        frogTounge.positionCount = points.Count;

        float lineSegmentDuration = duration;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / lineSegmentDuration;
            Vector3 pos = Vector3.Lerp(startingPos, endingPos, t);

            frogTounge.SetPosition(points.Count-1, pos);

            yield return null;
        }
    }
    


}
