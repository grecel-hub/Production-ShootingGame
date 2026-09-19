using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerHoldShortGunState : PlayerStandState
    {
        public PlayerHoldShortGunState(global::Player _player, PlayerStateMachine _stateMachine, PlayerControls _controls) : base(_player, _stateMachine, _controls)
        {
        }

        public override void Enter()
        {
            base.Enter();

            anim.SetBool("HoldShortGun", true);

            controls.Player.Fire.started += GetPlayerFireInput;
            controls.Player.Reload.started += GetPlayerReloadInput;

        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();

            anim.SetBool("HoldShortGun", false);

            controls.Player.Fire.started -= GetPlayerFireInput;
            controls.Player.Reload.started -= GetPlayerReloadInput;
        }

        //武器开火 --左键
        public void GetPlayerFireInput(InputAction.CallbackContext ctx)
        {
            if (isRuning || player.weaponManager.currentHaveGun == null)
                return;

            player.weaponManager.WeaponFire();
        }

        //武器换弹 --R
        public void GetPlayerReloadInput(InputAction.CallbackContext ctx)
        {
            player.weaponManager.Reload();
        }
    }
}