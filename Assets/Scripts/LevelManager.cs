using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script Description:
    //This script keeps info on the active level, and updates it when the level has been completed. It gets the levels from Unity's scene manager.


public class LevelManager : MonoBehaviour
{
    private int activelevelIndex;
    private static LevelManager instance;
    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelManager>();
            }
            return instance;
        }
    }
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        activelevelIndex = SceneManager.GetActiveScene().buildIndex;   
    }

    public void NextLevel()
    {
        activelevelIndex++;

        if(activelevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(activelevelIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
