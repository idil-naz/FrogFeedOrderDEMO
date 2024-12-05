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

        yield return new WaitForSeconds(2f);

        startTime = Time.time;
        startingPos = points[points.Count - 1];
        endingPos = points[points.Count - 2];

        float retractionTime = duration;
        float elapsedTime = 0f;
        bool pointRemoved = false;

        while (elapsedTime < retractionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime/retractionTime);

            Vector3 pos = Vector3.Lerp(startingPos, endingPos, t);

            frogTounge.SetPosition(points.Count - 1, pos);


            CollectBerries(pos);

            if (!pointRemoved && t >= 1f)
            {
                points.RemoveAt(points.Count - 1);
                frogTounge.positionCount = points.Count;

                pointRemoved = true;
            }

            yield return null;

        }

        frogTounge.SetPosition(points.Count - 1, endingPos);

        yield return new WaitForSeconds(1.5f);
        parentFrog.pathNodes.Clear();
        parentFrog.berriesOnPath.Clear();

    }

    public void CollectBerries(Vector3 tounguePos)
    {
        float collectionRadar = 0.5f;
        float spacing = 0.5f;
        float smooth = 0.25f;

        Collider[] berriesOnRadar = Physics.OverlapSphere(tounguePos, collectionRadar);

        List<Collider> sortedBerries = new List<Collider>(berriesOnRadar);
        sortedBerries.Sort((a, b) => Vector3.Distance(tounguePos, a.transform.position).CompareTo(Vector3.Distance(tounguePos, b.transform.position)));

        Vector3 prevBerryPos = tounguePos;

        foreach (Collider col in sortedBerries)
        {
            if (col.CompareTag("Grape"))
            {
                Vector3 berryPosition = col.transform.position;
                Vector3 direction = (prevBerryPos - berryPosition).normalized;
                //Vector3 targetPosition = Vector3.Lerp(berryPosition, tounguePos, 1.75f);
                Vector3 targetPosition = prevBerryPos- direction * spacing;
               

                col.transform.position = Vector3.Lerp(berryPosition, targetPosition, smooth);
                //prevBerryPos = targetPosition;

                float t = Mathf.Clamp01(Vector3.Distance(col.transform.position, tounguePos) / collectionRadar);
                //float t = Mathf.Clamp01((tounguePos - col.transform.position).magnitude / collectionRadar);
                col.transform.position = Vector3.Lerp(col.transform.position, tounguePos, t * smooth);
            }
        }


    }



    //public void CollectBerries(Vector3 tounguePos)
    //{

    //    float collectionRadar = 0.75f;
    //    float spacing = 0.8f;
    //    float smooth = 0.25f;

    //    Collider[] berriesOnRadar = Physics.OverlapSphere(tounguePos, collectionRadar);

    //    List<Collider> sortedBerries = new List<Collider>(berriesOnRadar);
    //    sortedBerries.Sort((a, b) => Vector3.Distance(tounguePos, a.transform.position).CompareTo(Vector3.Distance(tounguePos, b.transform.position)));

    //    Vector3 prevBerryPos = tounguePos;

    //    foreach (Collider col in sortedBerries)
    //    {
    //        if (col.CompareTag("Grape"))
    //        {
    //            Vector3 berryPosition = col.transform.position;
    //            Vector3 targetPosition = Vector3.Lerp(berryPosition, tounguePos, 1.75f);

    //            float distance = Vector3.Distance(prevBerryPos, targetPosition);

    //            //Vector3 direction = (targetPosition - prevBerryPos).normalized;
    //            Vector3 direction = (prevBerryPos - berryPosition).normalized;
    //            targetPosition = prevBerryPos - direction * spacing;

    //            col.transform.position = Vector3.Lerp(berryPosition, targetPosition, smooth);
    //            prevBerryPos = targetPosition;

    //            float t = Mathf.Clamp01((tounguePos - col.transform.position).magnitude / collectionRadar);
    //            col.transform.position = Vector3.Lerp(col.transform.position, tounguePos, t);
    //        }
    //    }


    //}LAST WORKING POINT



}
