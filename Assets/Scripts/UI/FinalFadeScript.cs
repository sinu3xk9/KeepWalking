using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFadeScript : MonoBehaviour
{
    public SceneControllerScript sceneController;

    public void endGame()
    {
        sceneController.getSceneFromName("MainMenu");
    }
}
