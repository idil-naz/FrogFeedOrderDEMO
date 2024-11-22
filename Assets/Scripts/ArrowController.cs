using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        Renderer renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material = gameManager.cellMaterials[((int)gameObject.transform.parent.GetComponent<CellController>().cellColor)];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
