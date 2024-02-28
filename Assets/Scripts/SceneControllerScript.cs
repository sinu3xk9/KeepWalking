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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneAt(0))
            {
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene(0);
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
}
