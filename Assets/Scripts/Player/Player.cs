using Assets.Scripts.Player;
using Cinemachine;
using UnityEngine;

//玩家
public class Player : Entity
{
    [Header("Camera Info")]
    public CinemachineFreeLook freeLook;
    public Transform camFollow;
    public Transform aimTarget;
    public GameObject crosshair;
    [SerializeField] private Transform aimStartTarget;
    [SerializeField] private LayerMask aimMask;
    private Camera cam;
    private Vector3 aimPosition;

    public WeaponManager weaponManager { get; private set; }

    public PlayerControls controls { get; private set; }

    //状态
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerNoGunState noGunState { get; private set; }
    public PlayerHoldShortGunState holdShortGunState { get; private set; }
    public PlayerAimState aimState { get; private set; }
    public PlayerNormalState normalState { get; private set; }


    protected void Awake()
    {
        stateMachine = new PlayerStateMachine();
        controls = new PlayerControls();

        noGunState = new PlayerNoGunState(this, stateMachine, controls);
        holdShortGunState = new PlayerHoldShortGunState(this, stateMachine, controls);
        aimState = new PlayerAimState(this, stateMachine, controls);
        normalState = new PlayerNormalState(this, stateMachine, controls);
    }

    protected override void Start()
    {
        base.Start();

        cam = Camera.main;

        weaponManager = GetComponent<WeaponManager>();

        stateMachine.Initialize(noGunState, normalState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
        stateMachine.currentCameraState.Update();

    }

    protected void FixedUpdate()
    {
        AimAngleLimit();
    }

    //获取准星位置
    public Vector3 GetShootTargetPoint()
    {
        Vector3 targetPoint;

        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
        Ray ray = cam.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, aimMask))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
        }

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

        return targetPoint;
    }

    //准星角度限制
    public void AimAngleLimit()
    {
        aimPosition = GetShootTargetPoint();
        aimPosition.y = 0f;

        Vector3 playerPosition = transform.position;
        playerPosition.y = 0f;

        Vector3 aimStartPosition = aimStartTarget.position;
        aimStartPosition.y = 0f;

        Vector3 aimVector = aimPosition - playerPosition;

        float angle = Vector3.Angle(aimVector, aimStartPosition - playerPosition);

        /*if (angle < 60 && !WeaponManager.instance.isRecoiling)
            aimTarget.position = Vector3.Lerp(aimTarget.position, GetShootTargetPoint(), 0.1f);
        else
            aimTarget.position = Vector3.Lerp(aimTarget.position, aimStartTarget.position, 0.1f);*/

        aimTarget.position = Vector3.Lerp(aimTarget.position, GetShootTargetPoint(), 1f);
    }
}
