using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject CellPrefab;
    public GameObject FrogPrefab;
    public GameObject GrapePrefab;

    public List<Material> cellMaterials;
    public List<Material> frogMaterials;
    public List<Material> berryMaterials;


    public Button restartButton;

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
        ReInitResButton();

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
        ReInitResButton();
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
}
