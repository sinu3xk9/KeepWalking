using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EventTriggerScript : MonoBehaviour
{
    public List<Animator> eventAnimators;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach(Animator animator in eventAnimators)
            {
                animator.SetTrigger("EventTrigger");
                if(this.name.Equals("TrashcanTriggerBox")) {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.trashCanFall, this.transform.position);
                }
            }
        }
    }
}
