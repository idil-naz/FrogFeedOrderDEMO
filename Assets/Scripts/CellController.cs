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

    public enum HousedElementType
    {
        Frog,
        Berry,
        Arrow
    } public HousedElementType housedElementType;

    public GameObject housedGameObject;
    public GameObject parentNode;

    private void Awake()
    {
        
    }

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

        if (gameObject.transform.parent != null) parentNode = gameObject.transform.parent.gameObject;
        if (gameObject.transform.childCount > 0) housedGameObject = gameObject.transform.GetChild(0).gameObject;


        //if this cell is the top cell in its own node, then the housed object of this cell should be setactive true.
        //if this cell is not the top cell in its own node, then the housed object of this cell should be setactive false.

        if (housedGameObject != null)
        {
            if (parentNode.GetComponent<NodesController>().getCellOnTop() != this.gameObject)
            {
                housedGameObject.SetActive(false);
            }
            else
            {
                housedGameObject.SetActive(true);
            }
        }

        Debug.Log($"cellOnTop: {parentNode.GetComponent<NodesController>().getCellOnTop()}, this GameObject: {this.gameObject.name}");

        //setObjectPosition();

        //all cells have one housed object child
        //all cells are parented by a node


    }

    // Update is called once per frame
    void Update()
    {
       //
    }

    void setObjectPosition()
    {
        int housedGameObjectIndex = parentNode.GetComponent<NodesController>().cells.IndexOf(gameObject);

        if (parentNode.GetComponent<NodesController>().cells.Count > 1)
        {
            housedGameObject.transform.localPosition += new Vector3(housedGameObject.transform.localPosition.x,
                                                                    housedGameObject.transform.localPosition.y + housedGameObjectIndex * 0.015f,
                                                                    housedGameObject.transform.localPosition.z);
        }


    }
}
