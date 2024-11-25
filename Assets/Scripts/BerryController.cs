using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BerryController : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject berryParentNode;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        Renderer renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material = gameManager.berryMaterials[((int)gameObject.transform.parent.GetComponent<CellController>().cellColor)];

        berryParentNode = gameObject.transform.parent.gameObject.GetComponent<CellController>().cellParentNode.gameObject;

    }


    public void MoveLeft()
    {
        //-1 on the x axis

        float targetx = berryParentNode.GetComponent<NodesController>().leftNode.transform.position.x;
        //berryParentNode = null;
        gameObject.LeanMoveX(targetx, 0.5f);
        //berryParentNode = berryParentNode.GetComponent<NodesController>().leftNode.gameObject;
    }

    public void MoveRight()
    {
        //+1 on the x axis
        float targetx = berryParentNode.GetComponent<NodesController>().rightNode.transform.position.x;
        gameObject.LeanMoveX(targetx, 0.5f);
        //berryParentNode = berryParentNode.GetComponent<NodesController>().rightNode.gameObject;
    }

    public void MoveUp()
    {
        //+1 on the z axis
        float targetz = berryParentNode.GetComponent<NodesController>().frontNode.transform.position.z;
        gameObject.LeanMoveZ(targetz, 0.5f);
        //berryParentNode = berryParentNode.GetComponent<NodesController>().frontNode.gameObject;
    }

    public void MoveDown()
    {
        //-1 on the z axis
        float targetz = berryParentNode.GetComponent<NodesController>().backNode.transform.position.z;
        gameObject.LeanMoveZ(targetz, 0.5f);
        //berryParentNode = berryParentNode.GetComponent<NodesController>().backNode.gameObject;
    }

}
