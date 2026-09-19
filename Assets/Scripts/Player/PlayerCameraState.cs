using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraState : PlayerState
{
    [Header("Camera Info")]//摄像机控制缩放属性
    protected float elapsed = 0f;
    protected float duration = 0.2f;
    protected float camRadiusVelocity = 0f;
    protected float camFollowVelocity = 0f;
    protected float camFOVVelocity = 0f;
    protected float currentCamRadius;
    protected float currentCamFollowX;
    protected float currentCamFOV;


    public PlayerCameraState(Player _player, PlayerStateMachine _stateMachine, PlayerControls _controls) : base(_player, _stateMachine, _controls)
    {
    }

    public override void Enter()
    {
        base.Enter();

        currentCamRadius = player.freeLook.m_Orbits[1].m_Radius;
        currentCamFollowX = player.camFollow.localPosition.x;
        currentCamFOV = player.freeLook.m_Lens.FieldOfView;

        controls.Player.Aim.started += GetPlayerAimInput;
    }

    public override void Exit()
    {
        base.Exit();

        controls.Player.Aim.started -= GetPlayerAimInput;
    }

    public override void Update()
    {
        base.Update();

        ChangeCameraRadius();
    }

    
    

    //武器瞄准 --右键
    public void GetPlayerAimInput(InputAction.CallbackContext ctx)
    {
        if (player.weaponManager.currentHaveGun == null) return;


        if (isAim)
            stateMachine.ChangeCameraState(player.normalState);
        else
            stateMachine.ChangeCameraState(player.aimState);
    }

    //缩放摄像机
    public void ChangeCameraRadius()
    {
        float targetRadius = isAim ? 1.75f : 5f;
        //float targetPositionX = isAim ? 0.4f : 0.0422f;
        float targetPositionX = isAim ? 0.4f : 0.4f;
        float targetFOV = isAim ? 20 : 40;

        //currentCamRadius = Mathf.SmoothDamp(currentCamRadius, targetRadius, ref camRadiusVelocity, duration);
        currentCamFollowX = Mathf.SmoothDamp(currentCamFollowX, targetPositionX, ref camFollowVelocity, duration);
        currentCamFOV = Mathf.SmoothDamp(currentCamFOV, targetFOV, ref camFOVVelocity, duration);

        player.freeLook.m_Orbits[1].m_Radius = currentCamRadius;
        player.camFollow.localPosition = new Vector3(currentCamFollowX, 2f, 0);
        player.freeLook.m_Lens.FieldOfView = currentCamFOV;
    }
}
