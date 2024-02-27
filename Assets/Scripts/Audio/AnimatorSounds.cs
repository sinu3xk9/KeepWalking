using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AnimatorSounds : MonoBehaviour
{
    // plays a footstep sound coming from the p
    public void playerFootstep() {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerFootstep, this.transform.position);
    }
}
