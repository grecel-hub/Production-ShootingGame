using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerHoldLongGunState : PlayerStandState
    {
        private bool isFiring;
        private int fireCount = 0;

        public PlayerHoldLongGunState(global::Player _player, PlayerStateMachine _stateMachine, PlayerControls _controls) : base(_player, _stateMachine, _controls)
        {
        }

        public override void Enter()
        {
            base.Enter();

            anim.SetBool("HoldLongGun", true);

            controls.Player.Fire.started += GetPlayerFireInput;
            controls.Player.Fire.canceled += GetPlayerFireInput;

            controls.Player.Reload.started += GetPlayerReloadInput;
        }

        public override void Update()
        {
            base.Update();

            if (isFiring)
            {
                if (isRuning || player.weaponManager.currentHaveGun == null)
                    return;

                fireCount++;
                player.weaponManager.WeaponFire(anim.GetFloat("Speed"), fireCount);
            }

        }

        public override void Exit()
        {
            base.Exit();

            isFiring = false;

            anim.SetBool("HoldLongGun", false);

            controls.Player.Fire.started -= GetPlayerFireInput;
            controls.Player.Fire.canceled -= GetPlayerFireInput;

            controls.Player.Reload.started -= GetPlayerReloadInput;
        }

        //武器开火 --左键
        public void GetPlayerFireInput(InputAction.CallbackContext ctx)
        {
            if (isRuning || player.weaponManager.currentHaveGun == null)
                return;

            if (ctx.started)
                isFiring = true;
            else if (ctx.canceled)
            {
                isFiring = false;
                fireCount = 0;
            }
        }

        //武器换弹 --R
        public void GetPlayerReloadInput(InputAction.CallbackContext ctx)
        {
            player.weaponManager.Reload();
        }
    }
}