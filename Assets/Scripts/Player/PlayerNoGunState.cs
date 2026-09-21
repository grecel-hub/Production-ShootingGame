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

            if (player.weaponManager.currentHaveGun != null)
            {
                if (player.weaponManager.currentHaveGun.gunType == GunType.Short) //进入持短枪状态
                    stateMachine.ChangeState(player.holdShortGunState);

                else if (player.weaponManager.currentHaveGun.gunType == GunType.Long) //进入持长枪状态
                    stateMachine.ChangeState(player.holdLongGunState);

            }
        }
    }
}