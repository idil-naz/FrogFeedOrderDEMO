using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FrogToungeScript : MonoBehaviour
{
    private float duration;

    public FrogController parentFrog;
    private LineRenderer frogTounge;
    private List<Vector3> points;

    public bool berryCollectionCompleted = false;
    //public bool pathingCompleted = false;


    private void Awake()
    {
        parentFrog = gameObject.transform.parent.GetComponent<FrogController>();
        frogTounge = GetComponent<LineRenderer>();

       
    }
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.init(1000);

        if (parentFrog.frogParentCell == null)
        {
            parentFrog.frogParentCell = gameObject.transform.parent.parent.GetComponent<CellController>().gameObject;
        }


        duration = parentFrog.pathingExecutionTime;
        points = new List<Vector3>();

        Vector3 initPos = new Vector3(

                parentFrog.transform.position.x,
                parentFrog.transform.position.y,
                parentFrog.transform.position.z
            ) ;

        if (parentFrog.frogParentCell != null)
        {
            initPos = parentFrog.frogParentCell.transform.TransformPoint(Vector3.up * (0.1f * parentFrog.frogParentCell.transform.GetSiblingIndex()));
        }

        frogTounge.SetPosition(0, initPos);
        points.Add(initPos);
        frogTounge.positionCount = points.Count;
        
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

        float startTime = Time.time;
        float lineSegmentDuration = duration;

        while (Time.time - startTime < lineSegmentDuration)
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

        yield return new WaitForSeconds(2f);
    }

    public void CollectBerries(Vector3 tounguePos)
    {
        float collectionRadar = 0.5f;
        float spacing = 1f;
        float smooth = 0.06f;

        Collider[] berriesOnRadar = Physics.OverlapSphere(tounguePos, collectionRadar);

        //for (int i = 0; i < berriesOnRadar.Length; i++)
        //{
        //    Debug.Log(berriesOnRadar[i].name);
        //}
        
        List<Collider> sortedBerries = new List<Collider>(berriesOnRadar);
        sortedBerries.Sort((a, b) => Vector3.Distance(tounguePos, a.transform.position).CompareTo(Vector3.Distance(tounguePos, b.transform.position)));

        Vector3 prevBerryPos = tounguePos;

        foreach (Collider col in sortedBerries)
        {
            if (!LeanTween.isTweening(col.gameObject))
            {
                LeanTween.cancel(col.gameObject);
            }

            if (col.CompareTag("Grape") && parentFrog.berriesOnPath.Contains(col.gameObject))
            {
                Vector3 berryPosition = col.transform.position;

                Vector3 direction = (prevBerryPos - berryPosition).normalized;
                Vector3 targetPosition = berryPosition + direction * spacing;
                prevBerryPos = targetPosition;


                float t = Mathf.Clamp01(Vector3.Distance(col.transform.position, tounguePos) / collectionRadar);
                col.transform.position = Vector3.Lerp(col.transform.position, tounguePos, t * smooth);

                //LEAN TWEEN ANIMATIONS WHEN BERRIES REACH FROG
                if (Vector3.Distance(col.transform.position, parentFrog.transform.position) < 0.85f)
                {
                    LeanTween.scale(col.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutSine)
                    .setOnComplete(() =>
                    {
                        Destroy(col.gameObject);
                        parentFrog.berriesOnPath.Clear();
                        parentFrog.pathNodes.Clear();

                        berryCollectionCompleted = true;

                    });

                }

                //LEAN TWEEN ANIMATIONS WHEN TOUNGE PASSES BY THE CELL
                if (col.transform.parent != null)
                {
                    GameObject berryParentCell = col.transform.parent.gameObject;

                    if (berryParentCell != null && berryParentCell.GetComponent<CellController>() != null)
                    {
                        GameObject berryParentNode = berryParentCell.GetComponent<CellController>().cellParentNode;
                        col.transform.parent = null;

                        if (!LeanTween.isTweening(berryParentCell))
                        {
                            LeanTween.cancel(berryParentCell);
                        }

                        LeanTween.scale(berryParentCell, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
                        {
                            if (!LeanTween.isTweening(berryParentCell.gameObject))
                            {

                                if (berryParentNode != null && berryParentNode.GetComponent<NodesController>() != null)
                                {
                                    berryParentNode.GetComponent<NodesController>().updateCellOnTop();
                                }
                                if (berryParentNode.GetComponent<NodesController>().cellOnTop.GetComponent<CellController>())
                                {
                                    berryParentNode.GetComponent<NodesController>().cellOnTop.GetComponent<CellController>().checkSelf();
                                }


                                Destroy(berryParentCell.gameObject);

                            }
                        });
                    }
                    
                }

            }
            //LEAN TWEEN ANIMATIONS FOR ARROW CELLS
            if (col.CompareTag("Arrow"))
            {
                if (col.transform.parent != null)
                {
                    GameObject arrowParentCell = col.transform.parent.gameObject;
                    GameObject arrowParentNode = arrowParentCell.GetComponent<CellController>().cellParentNode;
                    arrowParentCell.transform.parent = null;

                    if (!LeanTween.isTweening(arrowParentCell))
                    {
                        LeanTween.cancel(arrowParentCell);
                    }

                    LeanTween.scale(arrowParentCell, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
                    {
                        if (!LeanTween.isTweening(arrowParentCell.gameObject)){

                                arrowParentNode.GetComponent<NodesController>().updateCellOnTop();

                            if (arrowParentNode.GetComponent<NodesController>().cellOnTop.GetComponent<CellController>())
                            {
                                arrowParentNode.GetComponent<NodesController>().cellOnTop.GetComponent<CellController>().checkSelf();
                            }

                            Destroy(arrowParentCell.gameObject);
                        }
                    });
                }
            }


        }
    }


}
