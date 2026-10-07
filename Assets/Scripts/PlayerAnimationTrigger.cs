using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//player动画触发
public class PlayerAnimationTrigger : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Entity entity;

    //行走触发脚步声事件
    public void OnPlayFootStepWalk()
    {
        if ((anim.GetFloat("Speed") < 0.1 && anim.GetFloat("Speed") > -0.1) || anim.GetFloat("Speed") > 1.5)
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


    #region 换弹生命周期
    //换弹过程获取新弹匣
    public void OnReloadGetNewMagazine()
    {
        entity.weaponManager.GetNewMagazine();
    }

    //完成弹匣安装
    public void OnReloadInstallMagazine()
    {
        entity.weaponManager.DoneInstallMagazine();
    }

    //结束换弹
    public void OnReloadDone()
    {
        entity.weaponManager.canLeftIK = true;
        entity.weaponManager.DoneReload();
    }

    public void OnReloadPlayReloadUnload()
    {
        AudioManager.instance.PlayEvent("Play_Reload_Unload", gameObject);
    }

    public void OnReloadPlayReloadInstall()
    {
        AudioManager.instance.PlayEvent("Play_Reload_Install", gameObject);
    }

    public void OnReloadPlayReloadChamber()
    {
        AudioManager.instance.PlayEvent("Play_Reload_Chamber", gameObject);
    }
    #endregion

    public void EndLeftIK()
    {
        entity.weaponManager.canLeftIK = false;
    }
}
