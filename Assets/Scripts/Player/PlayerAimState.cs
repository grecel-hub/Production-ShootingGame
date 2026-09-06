using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimState : PlayerCameraState
{
    public PlayerAimState(Player _player, PlayerStateMachine _stateMachine, PlayerControls _controls) : base(_player, _stateMachine, _controls)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isAim = true;

        anim.SetBool("Aim", true);
        anim.applyRootMotion = false;

        player.crosshair.SetActive(true);

        player.weaponManager.WeaponAim(true);
    }

    public override void Exit()
    {
        base.Exit();

        isAim = false;
    }

    public override void Update()
    {
        base.Update();
    }
}
