using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//player动画触发
public class PlayerAnimationTrigger : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Player player;

    //行走触发脚步声事件
    public void OnPlayFootStepWalk()
    {
        if (anim.GetFloat("Speed") < 0.1 || anim.GetFloat("Speed") > 1.5)
            return;

        AudioManager.instance.PlayEvent("Play_Footstep_Walk", gameObject);
    }

    //奔跑触发脚步声事件
    public void OnPlayFootStepRun()
    {
        if (anim.GetFloat("Speed") <= 1.5)
            return;

        AudioManager.instance.PlayEvent("Play_Footstep_Run", gameObject);
    }

    //换弹过程获取新弹匣
    public void OnReloadGetNewMagazine()
    {
        player.weaponManager.GetNewMagazine();
    }

    //完成弹匣安装
    public void OnReloadInstallMagazine()
    {
        player.weaponManager.DoneInstallMagazine();
    }
    
    //完成上膛
    public void OnReloadDone()
    {
        player.weaponManager.canLeftIK = true;
        player.weaponManager.DoneReload();
    }

    public void EndLeftIK()
    {
        player.weaponManager.canLeftIK = false;
    }
}
