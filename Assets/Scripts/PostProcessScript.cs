using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessScript : MonoBehaviour
{
    public List<GameObject> touchingSafeZones = new List<GameObject>();
    public PostProcessVolume processVolume;
    private Vignette vign;
    // Start is called before the first frame update
    void Start()
    {
        processVolume.profile.TryGetSettings(out vign);
    }

    // Update is called once per frame
    void Update()
    {
        if (touchingSafeZones.Count < 1)
        {
            if (vign.intensity.value < .7f)
            {
                vign.intensity.value += .7f * Time.deltaTime;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Safe Zone"))
        {
            if (vign.intensity.value > 0f)
            {
                vign.intensity.value -= .7f * Time.deltaTime;
            }
            else
            {
                vign.intensity.value = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Safe Zone"))
        {
            touchingSafeZones.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Safe Zone"))
        {
            touchingSafeZones.Remove(other.gameObject);
        }
    }
}
