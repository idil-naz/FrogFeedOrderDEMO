using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogToungeScript : MonoBehaviour
{
    [SerializeField] public float duration = 1f;

    private LineRenderer frogTounge;
    private List<Vector3> points;

    public FrogController parentFrog;
    // Start is called before the first frame update
    void Start()
    {
        parentFrog = gameObject.transform.parent.GetComponent<FrogController>();

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
