using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogToungeScript : MonoBehaviour
{
    [SerializeField] public float duration = 10f;

    private LineRenderer frogTounge;
    private Vector3[] points;
    private int pointsCount;

    public FrogController parentFrog;
    // Start is called before the first frame update
    void Start()
    {
        parentFrog = gameObject.transform.parent.GetComponent<FrogController>();

        frogTounge = GetComponent<LineRenderer>();
        pointsCount = frogTounge.positionCount;
        points = new Vector3[pointsCount];

        Vector3 initPos = new Vector3(

                parentFrog.transform.position.x,
                parentFrog.transform.position.y + 0.15f*parentFrog.frogParentNode.GetComponent<NodesController>().cells.IndexOf(parentFrog.transform.parent.gameObject),
                parentFrog.transform.position.z

            );

        for (int i = 0; i < pointsCount; i++)
        {
            points[i] = initPos;
            frogTounge.SetPosition(i, initPos);

        }

        //StartCoroutine(ToungeAnim());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ToungeAnim()
    {
        float lineSegmentDuration = duration / pointsCount;

        for (int i = 0; i < pointsCount-1; i++)
        {
            float startTime = Time.time;

            Vector3 startingPos = points[i];


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
            points[i + 1] = endingPos;


            while (Time.time - startTime != duration)
            {
                float t = (Time.time - startTime) / lineSegmentDuration;
                Vector3 pos = Vector3.Lerp(startingPos, endingPos, t);

                    frogTounge.SetPosition(i, pos);

                yield return null;
            }
        }
    }
}
