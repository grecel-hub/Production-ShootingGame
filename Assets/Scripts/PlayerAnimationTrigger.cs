using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    public void PlayFootStep()
    {
        AudioManager.instance.PlayEvent("Play_Footstep", gameObject);
    }
}
