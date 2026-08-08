using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private Transform cam;

    private PlayerControls controls;
    private Animator anim;
    private Transform playerTransform;

    public Vector2 moveVector {  get; private set; }

    [Header("MoveInfo")]
    private bool isRuning;
    private Vector2 playerInputVec;
    private Vector3 playerMovement;
    private float currentSpeed;
    private float targetSpeed;

    public float rotaSpeed = 1000;
    public float walkSpeed = 1.5f;
    public float runSpeed = 3f;

    [Header("ShootingInfo")]
    private bool isAim = false;


    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerTransform = transform;
        cam = Camera.main.transform;

        controls.Player.Enable();

        controls.Player.Move.started += GetPlayerMoveInput;
        controls.Player.Move.performed += GetPlayerMoveInput;
        controls.Player.Move.canceled += GetPlayerMoveInput;

        controls.Player.Run.started += GetPlayerRunInput;
        controls.Player.Run.performed += GetPlayerRunInput;
        controls.Player.Run.canceled += EndPlayerRun => isRuning = false;

        controls.Player.Aim.started += GetPlayerIsAim;

        controls.Player.Fire.started += GetPlayerFireInput;
        

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        RotatePlayer();
        MovePlayer();
    }

    private void OnEnable()
    {
        controls?.Enable();
    }

    private void OnDisable()
    {
        controls?.Disable();
    }

    public void GetPlayerMoveInput(InputAction.CallbackContext ctx)
    {
        playerInputVec = ctx.ReadValue<Vector2>();
    }

    public void GetPlayerRunInput(InputAction.CallbackContext ctx)
    {
        if (playerInputVec != null)
            isRuning = true;

    }

    public void GetPlayerIsAim(InputAction.CallbackContext ctx)
    {
        if (isAim)
            isAim = false;
        else
            isAim = true;

        anim.SetBool("IsAim", isAim);
    }

    public void GetPlayerFireInput(InputAction.CallbackContext ctx)
    {
        WeaponManager.instance.WeaponFire();
    }

    //角色旋转
    public void RotatePlayer()
    {
        if (playerInputVec.Equals(Vector2.zero))
            return;

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0f;
        camRight.y = 0f;

        playerMovement = (camForward * playerInputVec.y + camRight * playerInputVec.x).normalized;
        //playerMovement = playerTransform.InverseTransformVector(playerMovement);
        if (playerMovement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerMovement, Vector3.up);
            playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, targetRotation, rotaSpeed * Time.deltaTime);

        }
    }

    //角色移动
    public void MovePlayer()
    {
        targetSpeed = isRuning ? runSpeed : walkSpeed;
        targetSpeed *= playerInputVec.magnitude;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 0.03f);
        anim.SetFloat("PlayerVertical", currentSpeed);
    }
}
