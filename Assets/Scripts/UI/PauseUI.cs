using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public GameObject QButton;
    public GameObject EButton;
    public GameObject pauseMenuUI;

    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the ESCAPE key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    void PauseGame()
    {
        QButton.SetActive(false);
        EButton.SetActive(false);
        pauseMenuUI.SetActive(true); // Show the pause menu
        Time.timeScale = 0f; // Pause the game
        isPaused = true; // Update the pause state

        // Free up the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ResumeGame()
    {
        QButton.SetActive(true);
        EButton.SetActive(true);
        pauseMenuUI.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Resume the game
        isPaused = false; // Update the pause state

        // Lock the cursor back for FPS control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
