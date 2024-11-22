using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellController : MonoBehaviour
{
    public GameManager gameManager;
    public enum CellColor
    {
        Blue = 0,
        Green = 1,
        Purple = 2,
        Red = 3,
        Yellow = 4
    } public CellColor cellColor;

    public enum HousedElement
    {
        Frog,
        Berry,
        Arrow
    } public HousedElement housedElement;

    public GameObject housedGameObject;
    public GameObject parentNode;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        Renderer renderer = GetComponent<Renderer>();

        switch (cellColor)
        {
            case CellColor.Blue:
                renderer.material = gameManager.cellMaterials[0];
                break;
            case CellColor.Green:
                renderer.material = gameManager.cellMaterials[1];
                break;
            case CellColor.Purple:
                renderer.material = gameManager.cellMaterials[2];
                break;
            case CellColor.Red:
                renderer.material = gameManager.cellMaterials[3];
                break;
            case CellColor.Yellow:
                renderer.material = gameManager.cellMaterials[4];
                break;
        }

        if(gameObject.transform.parent != null)
        {
            parentNode = gameObject.transform.parent.gameObject;
        }
        if (gameObject.transform.childCount > 0)
        {
            housedGameObject = gameObject.transform.GetChild(0).gameObject;
        }

        if (parentNode.GetComponent<NodesController>() != null)
        {
            if (parentNode.GetComponent<NodesController>().cellOnTop != this.gameObject)
            {
                if (housedGameObject != null)
                {
                    housedGameObject.SetActive(false);
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
       //
    }

    void setObjectPosition()
    {
        


    }
}
