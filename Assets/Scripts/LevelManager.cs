using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // Update is called once per frame
    void Update()
    {
        
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
