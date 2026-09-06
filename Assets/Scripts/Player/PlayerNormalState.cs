using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNormalState : PlayerCameraState
{
    public PlayerNormalState(Player _player, PlayerStateMachine _stateMachine, PlayerControls _controls) : base(_player, _stateMachine, _controls)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isAim = false;

        anim.SetBool("Aim", false);
        anim.applyRootMotion = true;

        player.crosshair.SetActive(true);

        player.weaponManager.WeaponAim(false);

        controls.Player.Pick.canceled += GetPlayerPickInput;
    }

    public override void Exit()
    {
        base.Exit();

        isAim = true;

        controls.Player.Pick.canceled -= GetPlayerPickInput;
    }

    public override void Update()
    {
        base.Update();
    }

    public void GetPlayerPickInput(InputAction.CallbackContext ctx)
    {
        if (player.weaponManager.GetGun() == null) return;
        player.weaponManager.SwitchGun(player.weaponManager.GetGun());
    }
}
