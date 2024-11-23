using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CellController;

public class FrogController : MonoBehaviour
{
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        Renderer renderer = gameObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        renderer.material = gameManager.frogMaterials[((int)gameObject.transform.parent.GetComponent<CellController>().cellColor)];
  
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider != null && hit.collider.gameObject.CompareTag("Frog"))
                {

                    Debug.Log("FrogClicked");
                }

            }


        }*/

        
    }

    private void OnMouseDown()
    {
        Debug.Log("FrogClicked");
    }

}
