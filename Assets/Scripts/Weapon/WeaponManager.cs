using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

//武器管理
public class WeaponManager : MonoBehaviour
{
    [Header("IK Info")]
    [SerializeField] private MultiAimConstraint armConstraint;
    [SerializeField] private MultiAimConstraint handConstraint;
    [SerializeField] private TwoBoneIKConstraint leftHandIk;
    [SerializeField] private Transform lHandle;
    private float weaponAimElapsed;
    private bool isAim;
    private bool isHoldGun;

    [Header("Recoil Info")]
    [SerializeField] private Transform aimTarget;
    [SerializeField] private Transform chest;
    [SerializeField] private Transform rightShoulder;

    [Header("Weapon Info")]
    [SerializeField] private LayerMask gunMask;
    [SerializeField] private Transform handle;
    [SerializeField] private AmmunitionUI ammunitionUI;
    public Gun currentHaveGun;

    private Camera cam;
    private float currentRecoil = 0;
    public bool isRecoiling;

    

    private Player player;


    private void Start()
    {
        player = GetComponent<Player>();

        cam = Camera.main;
    }

    private void Update()
    {
        GetPickGun();

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

        Debug.Log(bestGun);

        return bestGun;
    }

    public void EquipGun(Gun gun)
    {
        gun.transform.parent = handle;
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;

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
            EquipGun(newGun);
        }

        ammunitionUI.Show();
    }


    //后坐力控制
    public IEnumerator SpineBoneRecoil(Transform spineBone, Vector3 vector)
    {
        Quaternion baseRot;
        Quaternion offset;

        float elapsed = 0f;

        //使用Lua计算手枪后坐力
        while (elapsed < LuaManager.instance.handGunTab.Get<float>("duration"))
        {
            elapsed += Time.deltaTime;

            currentRecoil = LuaManager.instance.getHandGunRecoil(elapsed); 

            baseRot = spineBone.localRotation;
            offset = Quaternion.AngleAxis(currentRecoil, vector);
            spineBone.localRotation = baseRot * offset;

            yield return null;
        }
    }

    //武器攻击
    public void WeaponFire()
    {
        if (currentHaveGun.Fire(player))
        {
            StartCoroutine(SpineBoneRecoil(rightShoulder, Vector3.right));
            StartCoroutine(SpineBoneRecoil(chest, Vector3.up));
        }
        else
        {

        }

        ammunitionUI.Show();

    }

    //武器换弹
    public void Reload()
    {
        currentHaveGun.Reload();
        ammunitionUI.Show();
    }

    //武器瞄准
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
