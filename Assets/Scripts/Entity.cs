using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Transform camFollow;
    public Transform aimTarget;

    public CharacterController character { get; protected set; }
    public Animator anim {  get; protected set; }
    public Rigidbody[] rbs { get; protected set; }
    public WeaponManager weaponManager { get; private set; }


    protected int health = 100;

    public bool isDead;
    public bool canDie = true;

    protected virtual void Start()
    {
        character = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        rbs = GetComponentsInChildren<Rigidbody>();
        weaponManager = GetComponent<WeaponManager>();
    }

    protected virtual void Update()
    {
        if (health <= 0)
        {
            isDead = true;
        }

        Die();
    }

    //失去生命值
    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    //角色死亡
    public void Die()
    {
        if (!isDead || !canDie)
            return;

        health = 0;

        SetLayerRecursively(gameObject, LayerMask.NameToLayer("Dead"));

        foreach (Rigidbody rb in rbs)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        character.enabled = false;
        anim.enabled = false;

        canDie = false;
    }

    // 递归修改自身及所有子物体的层
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
