using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//Script description:
    //The GameManager keeps the materials for the objects to pull from.

    //It is responsible for checking if the level has been completed, and also is in communication with the level manager to load and pass through levels.
    //It is also responsible for keeping and updating the move count, and operating the restart button.


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private LevelManager levelManager;

    public GameObject CellPrefab;
    public GameObject FrogPrefab;
    public GameObject GrapePrefab;

    public List<Material> cellMaterials;
    public List<Material> frogMaterials;
    public List<Material> berryMaterials;

    public GameGridController gridController;
    public bool levelCompleteTriggered = false;

    public Button restartButton;
    public TextMeshProUGUI movesLeftTxt;
    public int moves;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if(levelManager == null)
        {
            levelManager = FindObjectOfType<LevelManager>();
        }
        ReinitLevel();
        InvokeRepeating("CheckLevelCompletion", 1f, 1f);
    }

    private void Update()
    {
        if (movesLeftTxt != null)
        {
            movesLeftTxt.SetText("Moves Left: " + moves);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ReinitLevel();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ReInitResButton()
    {
        if (restartButton == null)
        {
            GameObject buttonObj = GameObject.Find("RestartButton");
            if (buttonObj != null)
            {
                restartButton = buttonObj.GetComponent<Button>();
            }
        }

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartLevel);
        }
    }

    private void ReInitMoveLimit()
    {
        if (movesLeftTxt == null)
        {
            movesLeftTxt = GameObject.Find("MoveLimit").GetComponent<TextMeshProUGUI>();
        }
    }

    private void CheckLevelCompletion()
    {
        if (!levelCompleteTriggered && gridController != null && gridController.allCellsCleared())
        {
            //Debug.Log("level complete");
            LevelComplete();
            
        }
        if(!levelCompleteTriggered && moves == 0)
        {
            //Debug.Log("level failed");
            CancelInvoke();
        }
    }

    public void LevelComplete()
    {
        levelCompleteTriggered = true;

        if(levelManager != null)
        {
            LevelManager.Instance.NextLevel();
            levelCompleteTriggered = false;
        }
    }

    private void ReinitLevel()
    {
        ReInitResButton();
        ReInitMoveLimit();
        gridController = GameObject.Find("GameGrid").GetComponent<GameGridController>();
        moves = 25;
    }
}
