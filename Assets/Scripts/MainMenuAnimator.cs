using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuAnimator : MonoBehaviour
{
    public int blinkDelay;
    public int counter;
    public TextMeshProUGUI mainMenuText;
    // Start is called before the first frame update
    void Start()
    {
        mainMenuText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Debug.Log(mainMenuText.text.Length);
        counter++;
        if (counter >= blinkDelay)
        {
            counter = 0;
            if (mainMenuText.text.Length == 12)
            {
                mainMenuText.text = "Keep Walking|";
                Debug.Log("found with no line");
            }
            else if (mainMenuText.text.Length == 13)
            {
                mainMenuText.text = "Keep Walking";
                Debug.Log("found with line");
            }
        }
    }
}
