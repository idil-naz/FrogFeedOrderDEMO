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
}
