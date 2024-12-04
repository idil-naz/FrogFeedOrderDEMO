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
        if (berryParentNode.GetComponent<NodesController>().leftNode != null)
        {
            float targetx = berryParentNode.GetComponent<NodesController>().leftNode.transform.position.x;
            gameObject.LeanMoveX(targetx, 0.5f);
            //berryParentNode = berryParentNode.GetComponent<NodesController>().leftNode.gameObject;
            //gameObject.transform.parent = berryParentNode.GetComponent<NodesController>().cellOnTop.transform;
        }
    }

    public void MoveRight()
    {
        //+1 on the x axis
        if (berryParentNode.GetComponent<NodesController>().rightNode != null)
        {
            float targetx = berryParentNode.GetComponent<NodesController>().rightNode.transform.position.x;
            gameObject.LeanMoveX(targetx, 0.5f);
            //berryParentNode = berryParentNode.GetComponent<NodesController>().rightNode.gameObject;
            //gameObject.transform.parent = berryParentNode.GetComponent<NodesController>().cellOnTop.transform;
        }
    }

    public void MoveUp()
    {
        //+1 on the z axis
        if(berryParentNode.GetComponent<NodesController>().frontNode != null)
        {
            float targetz = berryParentNode.GetComponent<NodesController>().frontNode.transform.position.z;
            gameObject.LeanMoveZ(targetz, 0.5f);
            //berryParentNode = berryParentNode.GetComponent<NodesController>().frontNode.gameObject;
            //gameObject.transform.parent = berryParentNode.GetComponent<NodesController>().cellOnTop.transform;
        }
    }

    public void MoveDown()
    {
        //-1 on the z axis
        if(berryParentNode.GetComponent<NodesController>().backNode != null)
        {
            float targetz = berryParentNode.GetComponent<NodesController>().backNode.transform.position.z;
            gameObject.LeanMoveZ(targetz, 0.5f);
            //berryParentNode = berryParentNode.GetComponent<NodesController>().backNode.gameObject;
            //gameObject.transform.parent = berryParentNode.GetComponent<NodesController>().cellOnTop.transform;
        }
    }
}
