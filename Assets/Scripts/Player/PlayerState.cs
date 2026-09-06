using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerControls controls;
    protected Player player;

    protected Animator anim;
    protected Transform playerTransform;
    protected Transform cam;

    protected CharacterController character;

    protected static bool isAim = false;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, PlayerControls _controls)
    {
        player = _player;
        stateMachine = _stateMachine;
        controls = _controls;
    }

    public virtual void Enter()
    {
        anim = player.anim;

        playerTransform = player.transform;
        cam = Camera.main.transform;

        character = player.character;

        controls.Player.Enable();
    }

    public virtual void Exit()
    {
        
    }

    public virtual void Update()
    {
        
    }

}
