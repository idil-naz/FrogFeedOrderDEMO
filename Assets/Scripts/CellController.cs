using System.Collections;
using System.Collections.Generic;
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
    }

    public CellColor cellColor;

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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setObjectPosition()
    {



    }
}
