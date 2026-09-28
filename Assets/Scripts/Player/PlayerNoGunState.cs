using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerNoGunState : PlayerStandState
    {
        public PlayerNoGunState(global::Player _player, PlayerStateMachine _stateMachine, PlayerControls _controls) : base(_player, _stateMachine, _controls)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}