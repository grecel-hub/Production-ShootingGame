using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.U2D;
using UnityEngine.UI;

//武器管理
public class WeaponManager : MonoBehaviour
{
    [Header("IK Info")] //IK
    [SerializeField] private MultiAimConstraint armConstraint;
    [SerializeField] private MultiAimConstraint handConstraint;
    [SerializeField] private TwoBoneIKConstraint leftHandIk;
    [SerializeField] private Transform lHandle;
    public bool canLeftIK;
    private float weaponAimElapsed;
    private bool isAim;
    private bool isHoldGun;

    [Header("Recoil Info")] //后坐力
    [SerializeField] private Transform chest;
    [SerializeField] private Transform rightShoulder;
    [SerializeField] private CinemachineFreeLook freeLook;
    private float currentRecoil = 0;
    public bool isRecoiling;

    [Header("Weapon Info")] //武器
    [SerializeField] private LayerMask gunMask;
    [SerializeField] private Transform handle;
    [SerializeField] private AmmunitionUI ammunitionUI;
    public Gun currentHaveGun;
    public bool isReloading;
    private Transform startLHandle;


    private Camera cam;

    

    private Player player;


    private void Start()
    {
        player = GetComponent<Player>();

        cam = Camera.main;
    }

    private void Update()
    {
        GetPickGun();

        if (canLeftIK)
            LeftHandIkWait(isHoldGun);

        RightHandIkWait(isAim);

        if (currentHaveGun != null)
        {
            isHoldGun = true;

            ammunitionUI.SetAmmunitionText(currentHaveGun.GetMagazineSize());
        }

    }

    private void FixedUpdate()
    {
        if (currentHaveGun != null)
        {
            lHandle.position = currentHaveGun.GetLHandle().position;
            lHandle.rotation = currentHaveGun.GetLHandle().rotation;

        }
    }

    private void LateUpdate()
    {

    }

    //准星获取要拾取的枪支
    public Gun GetPickGun()
    {
        float pickUpRadius = 2.5f; // 角色的“手臂长度”范围
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickUpRadius, gunMask);

        Gun bestGun = null;
        float bestScore = float.MaxValue; // 用于选择“最优”目标

        foreach (Collider col in hitColliders)
        {
            // 获取武器组件（假设挂在根节点或自身）
            Gun gun = col.transform.root.GetComponent<Gun>();
            if (gun == null) continue;

            // 2. 核心筛选：限制在角色前方 120度 锥形范围内（防止拾取背后的物体）
            Vector3 dirToGun = (col.transform.position - transform.position).normalized;
            float angleToGun = Vector3.Angle(transform.forward, dirToGun);
            if (angleToGun > 60f) continue; // 大于60度（即120度锥角）忽略

            // 3. （进阶手感）计算该物体在屏幕上的投影位置，越靠近屏幕中心优先权越高
            Vector3 screenPos = cam.WorldToScreenPoint(col.transform.position);
            // 剔除在相机背后的物体（Z轴为负）
            if (screenPos.z < 0) continue;

            float distToScreenCenter = Vector3.Distance(
                screenPos,
                new Vector3(Screen.width * 0.5f, Screen.height * 0.5f)
            );

            // 选取距离屏幕中心最近的物品作为最终拾取目标
            if (distToScreenCenter < bestScore)
            {
                bestScore = distToScreenCenter;
                bestGun = gun;
            }
        }

        return bestGun;
    }

    public void EquipGun(Gun gun)
    {
        gun.transform.parent = handle;
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;

        gun.GetComponent<Rigidbody>().isKinematic = true;

        currentHaveGun = gun;
    }

    //换枪
    public void SwitchGun(Gun newGun)
    {
        if (currentHaveGun == null)
        {
            EquipGun(newGun);
        }
        else
        {
            currentHaveGun.GetComponent<ParentConstraint>().enabled = false;
            currentHaveGun.GetComponent<Rigidbody>().isKinematic = false;
            EquipGun(newGun);
        }

        ammunitionUI.Show();
    }


    //模型上抬后坐力
    public IEnumerator SpineBoneRecoil(Transform spineBone, Vector3 vector)
    {
        Quaternion spineBaseRot = spineBone.localRotation;
        Quaternion spineOffset;

        float elapsed = 0f;

        //获取Lua计算后坐力
        while (elapsed < currentHaveGun.duration)
        {
            elapsed += Time.deltaTime;

            currentRecoil = currentHaveGun.GetGunRecoil(elapsed);

            //人物模型后坐力表现
            spineOffset = Quaternion.AngleAxis(currentRecoil, vector);
            spineBone.localRotation = spineBaseRot * spineOffset;

            yield return null;
        }

        spineBone.localRotation = spineBaseRot;

        currentRecoil = 0f;
    }

    // 摄像机上抬后坐力
    public IEnumerator CamRecoil()
    {
        isRecoiling = true;

        float freeLookYAxisBase = freeLook.m_YAxis.Value;
        float freeLookYAxisOffset;

        float elapsed = 0f;
        float lastRecoil = 0f;

        //获取Lua计算后坐力
        while (elapsed < currentHaveGun.duration)
        {
            elapsed += Time.deltaTime;

            currentRecoil = currentHaveGun.GetGunRecoil(elapsed);

            float freeLookRecoil = currentRecoil / 500;

            freeLookYAxisOffset = (freeLookRecoil - lastRecoil);

            if (freeLook.m_YAxis.Value > freeLookYAxisBase && elapsed / currentHaveGun.duration > 0.3) //当玩家手动压枪至原位且处于回弹状态：停止回弹
                break;

            freeLook.m_YAxis.Value += freeLookYAxisOffset;

            lastRecoil = freeLookRecoil;

            yield return null;
        }

        currentRecoil = 0f;

        isRecoiling = false;
    }

    //武器攻击
    public void WeaponFire(float moveSpeed, int fireCount = 0)
    {
        if (currentHaveGun.Fire(player, moveSpeed, fireCount))
        {
            StartCoroutine(CamRecoil()); //视角后坐力
            StartCoroutine(SpineBoneRecoil(rightShoulder, Vector3.right)); //手臂后坐力
            StartCoroutine(SpineBoneRecoil(chest, Vector3.up)); //躯干后坐力
        }
        else
        {

        }

        ammunitionUI.Show();

    }

    //武器换弹
    public void Reload()
    {
        isReloading = true;

        startLHandle = lHandle;

        currentHaveGun.Reload(isReloading);
        ammunitionUI.Show();

        player.anim.SetBool("Reload", isReloading);
    }

    //武器换弹结束
    public void DoneReload()
    {
        isReloading = false;

        currentHaveGun.Reload(isReloading);
        player.anim.SetBool("Reload", isReloading);

        lHandle = startLHandle;
    }

    //武器瞄准状态
    public void WeaponAim(bool _isAim)
    {
        isAim = _isAim;

        if (isAim)
        {
            handConstraint.data.constrainedXAxis = false;
            handConstraint.data.constrainedZAxis = false;
        }
        else
        {
            handConstraint.data.constrainedXAxis = true;
            handConstraint.data.constrainedZAxis = true;
        }
    }

    //左手IK吸附枪托
    private void LeftHandIkWait(bool isHoldGun)
    {
        if (isReloading)
            return;

        if (isHoldGun)
        {
            weaponAimElapsed += Time.deltaTime;

            if (weaponAimElapsed >= 0.2f)
            {
                leftHandIk.weight = 1;
            }
        }
        else
        {
            leftHandIk.weight = 0;
            weaponAimElapsed = 0;
        }
    }

    //右手IK吸附
    private void RightHandIkWait(bool isAim)
    {
        if (isReloading)
            return;

        if (isAim)
        {
            weaponAimElapsed += Time.deltaTime;

            if (weaponAimElapsed >= 0.2f)
            {
                armConstraint.weight = 1;
            }
        }
        else
        {
            armConstraint.weight = 0;
        }
    }


}
