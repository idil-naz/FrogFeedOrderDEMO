using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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