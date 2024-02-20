using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InteractionBar : MonoBehaviour
{
    public FPSController player;
    public GameObject progressBarUI;
    public RectTransform progressBarTransform;
    public GameObject phoneText;
    public GameObject pepperText;

    // Update is called once per frame
    void Update()
    {
        if (player.useTick > 0 && (!player.phoneActive && !player.pepperActive))
        {
            progressBarUI.SetActive(true);
            progressBarTransform.localScale = new Vector3(Mathf.Clamp(player.useTick / player.useDuration, 0, 1), 1, 1);
        }
        else
        {
            progressBarUI.SetActive(false);
        }
        phoneText.SetActive(player.phoneActive);
        pepperText.SetActive(player.pepperActive);
    }
}
