using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /*

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                Debug.Log("quitting whole game");
                Application.Quit();
            }
            else if(SceneManager.GetActiveScene().name == "GameScene")
            {
                if (Time.timeScale == 1)
                {
                    Time.timeScale = 0f; // Pause the game
                }
                else
                {
                    Time.timeScale = 1f; // Resume the game
                }
            }
            else
            {
                getSceneAtIndex(0);
            }
        }
    }

    public void getSceneAtIndex(int index)
    {
        SceneManager.LoadScene(index);
    } 

    public void quitGame()
    {
        Application.Quit();
    }
    */
}
