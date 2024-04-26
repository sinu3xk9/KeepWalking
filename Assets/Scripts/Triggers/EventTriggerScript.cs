using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EventTriggerScript : MonoBehaviour
{
    public List<Animator> eventAnimators;
    public String comment;
    public EventReference eventAudio;
    private bool eventPlayed = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !eventPlayed)
        {
            eventPlayed = true;
            AudioManager.instance.PlayOneShot(eventAudio, this.transform.position);
            foreach(Animator animator in eventAnimators)
            {
                animator.SetTrigger("EventTrigger");
            }
        }
    }
}
