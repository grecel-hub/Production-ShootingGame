using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStandState : PlayerState
{
    [Header("MoveInfo")]
    protected bool isRuning;
    protected Vector2 playerInputVec;
    protected Vector3 playerMovement;
    protected float currentSpeed;
    protected float targetSpeed;
    protected float moveSpeed = 2;
    protected float rotaSpeed = 1000;
    protected float backSpeed = -1f;
    protected float walkSpeed = 1.5f;
    protected float runSpeed = 3f;


    public PlayerStandState(Player _player, PlayerStateMachine _stateMachine, PlayerControls _controls) : base(_player, _stateMachine, _controls)
    {
    }

    public override void Enter()
    {
        base.Enter();
        playerTransform = player.transform;

        controls.Player.Move.started += GetPlayerMoveInput;
        controls.Player.Move.performed += GetPlayerMoveInput;
        controls.Player.Move.canceled += GetPlayerMoveInput;

        controls.Player.Run.started += GetPlayerRunInput;
        controls.Player.Run.performed += GetPlayerRunInput;
        controls.Player.Run.canceled += EndPlayerRun => isRuning = false;

        
    }

    public override void Exit()
    {
        base.Exit();

        controls.Player.Move.started -= GetPlayerMoveInput;
        controls.Player.Move.performed -= GetPlayerMoveInput;
        controls.Player.Move.canceled -= GetPlayerMoveInput;

        controls.Player.Run.started -= GetPlayerRunInput;
        controls.Player.Run.performed -= GetPlayerRunInput;
        controls.Player.Run.canceled -= EndPlayerRun => isRuning = false;


    }

    public override void Update()
    {
        base.Update();

        RotatePlayer();

        if (!isAim)
            MovePlayer();
        else
            CharacterMovePlayer();

        
    }

    //角色移动 --W,A,S,D
    public void GetPlayerMoveInput(InputAction.CallbackContext ctx)
    {
        playerInputVec = ctx.ReadValue<Vector2>();
    }

    //角色奔跑 --LSHIFT
    public void GetPlayerRunInput(InputAction.CallbackContext ctx)
    {
        if (playerInputVec != null && !isAim)
            isRuning = true;

    }

    //角色旋转
    public void RotatePlayer()
    {
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0f;
        camRight.y = 0f;

        if (isAim)
            playerMovement = camForward.normalized;
        else
            playerMovement = (camForward * playerInputVec.y + camRight * playerInputVec.x).normalized;

        if (playerMovement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerMovement, Vector3.up);
            playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, targetRotation, rotaSpeed * Time.deltaTime);
        }
    }

    //角色正常状态移动
    public void MovePlayer()
    {
        targetSpeed = isRuning ? runSpeed : walkSpeed;
        targetSpeed *= playerInputVec.magnitude;

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 0.1f);
        anim.SetFloat("Speed", currentSpeed);
    }

    //角色瞄准状态移动
    public void CharacterMovePlayer()
    {
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0f;
        camRight.y = 0f;

        playerMovement = (camForward * playerInputVec.y + camRight * playerInputVec.x).normalized;

        character.Move(playerMovement * moveSpeed * Time.deltaTime);

        targetSpeed = (playerInputVec.y < 0f) ? backSpeed : walkSpeed;
        targetSpeed *= playerInputVec.magnitude;

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 0.03f);
        anim.SetFloat("Speed", currentSpeed);
    }
}
