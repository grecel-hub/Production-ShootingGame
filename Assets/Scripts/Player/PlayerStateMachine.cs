using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState {  get; private set; }

    public PlayerState currentCameraState { get; private set; }

    public void Initialize(PlayerState _playerState, PlayerState _cameraState)
    {
        currentState = _playerState;
        currentState.Enter();

        currentCameraState = _cameraState;
        currentCameraState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }

    public void ChangeCameraState(PlayerState _newState)
    {
        currentCameraState.Exit();
        currentCameraState = _newState;
        currentCameraState.Enter();
    }
}
