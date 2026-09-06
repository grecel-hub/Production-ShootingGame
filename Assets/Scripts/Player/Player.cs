using Cinemachine;
using UnityEngine;

//玩家
public class Player : MonoBehaviour
{
    [Header("Camera Info")]
    private Camera cam;
    public CinemachineFreeLook freeLook;
    public Transform camFollow;
    public Transform aimTarget;
    public GameObject crosshair;
    [SerializeField] private Transform aimStartTarget;
    [SerializeField] private LayerMask aimMask;
    private Vector3 aimPosition;


    public CharacterController character;
    public WeaponManager weaponManager { get; private set; }

    private Transform playerTransform;

    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerControls controls { get; private set; }

    public Animator anim { get; private set; }

    //状态
    public PlayerStandState standState { get; private set; }
    public PlayerAimState aimState { get; private set; }
    public PlayerNormalState normalState { get; private set; }


    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        controls = new PlayerControls();

        standState = new PlayerStandState(this, stateMachine, controls);
        aimState = new PlayerAimState(this, stateMachine, controls);
        normalState = new PlayerNormalState(this, stateMachine, controls);
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        //rb = GetComponent<Rigidbody>();
        cam = Camera.main;

        weaponManager = GetComponent<WeaponManager>();
        character = GetComponent<CharacterController>();

        playerTransform = transform;

        stateMachine.Initialize(standState, normalState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
        stateMachine.currentCameraState.Update();

    }

    private void FixedUpdate()
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

        return targetPoint;
    }

    //准星角度限制
    public void AimAngleLimit()
    {
        aimPosition = GetShootTargetPoint();
        aimPosition.y = 0f;

        Vector3 playerPosition = playerTransform.position;
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
