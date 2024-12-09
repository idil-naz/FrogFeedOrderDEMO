//using JetBrains.Annotations;
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
    public GameObject cellParentNode;

    private void Awake()
    {
        cellParentNode = gameObject.transform.parent.gameObject;
        housedGameObject = gameObject.transform.GetChild(0).gameObject;

        

        if (housedGameObject != null)
        {


            if (housedElementType == HousedElementType.Frog)
            {
                housedGameObject.GetComponent<FrogController>().frogParentNode = cellParentNode;
            }
            else if (housedElementType == HousedElementType.Berry)
            {
                housedGameObject.GetComponent<BerryController>().berryParentNode = cellParentNode;
            }
            else if (housedElementType == HousedElementType.Arrow)
            {
                housedGameObject.GetComponent<ArrowController>().arrowParentNode = cellParentNode;
            }
        }
        checkSelf();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        setCellColor();
        
    }

    public void checkSelf()
    {
        if (cellParentNode.GetComponent<NodesController>().getCellOnTop() != this.gameObject)
        {
            housedGameObject.SetActive(false);
            if (housedGameObject.CompareTag("Grape"))
            {
                housedGameObject.GetComponent<Collider>().enabled = false;
            }
        }
        else
        {
            housedGameObject.SetActive(true);
            if (housedGameObject.CompareTag("Grape"))
            {
                housedGameObject.GetComponent<Collider>().enabled = true;
            }

            if (!LeanTween.isTweening(this.gameObject))
            {
                LeanTween.scale(this.gameObject, Vector3.one * 1.3f, 0.1f).setEase(LeanTweenType.easeOutBounce).setOnComplete(() =>
                {
                    LeanTween.scale(this.gameObject, Vector3.one, 0.1f).setEase(LeanTweenType.easeInBounce);
                });
            }

        }
    }

    void setCellColor()
    {
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
    }
}
