using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//player动画触发
public class PlayerAnimationTrigger : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Player player;

    //行走触发脚步声事件
    public void PlayFootStep_Walk()
    {
        if (anim.GetFloat("Speed") < 0.1 || anim.GetFloat("Speed") > 1.5)
            return;

        AudioManager.instance.PlayEvent("Play_Footstep_Walk", gameObject);
    }

    //奔跑触发脚步声事件
    public void PlayFootStep_Run()
    {
        if (anim.GetFloat("Speed") <= 1.5)
            return;

        AudioManager.instance.PlayEvent("Play_Footstep_Run", gameObject);
    }

    public void EndReload()
    {
        player.weaponManager.canLeftIK = true;
        player.weaponManager.DoneReload();
    }

    public void EndLeftIK()
    {
        player.weaponManager.canLeftIK = false;
    }
}
