using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

// source: https://www.youtube.com/watch?v=rcBHIOjZDpk&ab_channel=ShapedbyRainStudios
public class FMODEvents : MonoBehaviour
{
    [field: Header("Player Sounds")]
    [field: SerializeField] public EventReference playerFootstep {get; private set;}

    [field: Header("Ambiences")]
    [field: SerializeField] public EventReference ambienceTrack {get; private set;}

    [field: Header("Environment Sounds")]
    [field: SerializeField] public EventReference followFootstep {get; private set;}



   public static FMODEvents instance {get; private set;}

   //ensures that the class is singleton
   private void Awake()
   {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMODEvents instance in the scene");
        }
        instance = this;
   }
}
