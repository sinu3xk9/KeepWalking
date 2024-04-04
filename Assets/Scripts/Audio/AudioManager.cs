using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

//Source: https://www.youtube.com/watch?v=rcBHIOjZDpk&ab_channel=ShapedbyRainStudios
public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances;
    public static AudioManager instance {get; private set;}
    private EventInstance ambienceEventInstance;

    //ensures that the class is singleton
    private void Awake() 
    {
        if(instance != null) 
        {
            Debug.LogError("Found more than one AudioManager in the scene");
        }
        instance = this;

        eventInstances = new List<EventInstance>();
    }

    private void Start()
    {
        InitializeAmbience(FMODEvents.instance.ambienceTrack);
    }

    //Initializes Ambience
    private void InitializeAmbience(EventReference ambienceEventReference) 
    {
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }

    //plays a single instance of a single sound
    public void PlayOneShot(EventReference sound, Vector3 worldPosition)
    {
        RuntimeManager.PlayOneShot(sound, worldPosition);
    }

    //plays a sound for the duration of the scene
    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp() 
    {
        foreach(EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    //Cleans up when the 
    private void OnDestroy() 
    {
        CleanUp();
    }

}
