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
    public FrogDirection direction;

   

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        Renderer renderer = gameObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        renderer.material = gameManager.frogMaterials[((int)gameObject.transform.parent.GetComponent<CellController>().cellColor)];

        switch (direction)
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

        //Path.AddFirst(frogParentNode);
        //foreach(var x in Path)
        //Debug.Log(x.ToString());

    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void OnMouseDown()
    {
        Debug.Log("FrogClicked");

        

    }

    IEnumerator StartPathing()
    {
        yield return null;
    }

}
