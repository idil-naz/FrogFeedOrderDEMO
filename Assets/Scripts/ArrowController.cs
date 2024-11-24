using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject arrowParentNode;
    public enum ArrowDirection
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    }public ArrowDirection direction;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        Renderer renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material = gameManager.cellMaterials[((int)gameObject.transform.parent.GetComponent<CellController>().cellColor)];

        switch (direction)
        {
            case ArrowDirection.Up:
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 0, gameObject.transform.eulerAngles.z);
                break;
            case ArrowDirection.Down:
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 180, gameObject.transform.eulerAngles.z);
                break;
            case ArrowDirection.Left:
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 270, gameObject.transform.eulerAngles.z);
                break;
            case ArrowDirection.Right:
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 90, gameObject.transform.eulerAngles.z);
                break;
        }
    }
}
