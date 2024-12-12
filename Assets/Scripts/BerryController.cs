using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script description:
    //BerryController is responsible for inheriting its color from the cell it in on from the GameManager. 
    //It also keeps information on whether it is collected or not
    //The parent of this object is the cell it is housed by, and the parent of that is the parent node.

public class BerryController : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject berryParentCell;
    public GameObject berryParentNode;

    public bool isCollected = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        Renderer renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material = gameManager.berryMaterials[((int)gameObject.transform.parent.GetComponent<CellController>().cellColor)];

        berryParentCell = gameObject.transform.parent.gameObject;
        berryParentNode = berryParentCell.transform.parent.gameObject;
    }

}