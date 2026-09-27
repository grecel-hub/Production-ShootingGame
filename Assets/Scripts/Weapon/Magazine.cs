using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Magazine : MonoBehaviour
{
    protected Rigidbody rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
    }

    protected virtual void Update()
    {
    }

    //令弹匣跟随左手
    public void UpdateTransform(Transform _transform)
    {
        if (_transform == null)
            return;

        transform.parent = _transform;
        transform.localPosition = Vector3.zero; 
        transform.localRotation = Quaternion.identity;
    }

    //卸载弹匣
    public void UnloadMagazine()
    {
        rb.isKinematic = false;

        transform.SetParent(null);
    }

    //安装弹匣
    public void InstallMagazine(Transform magazineTransform)
    {
        rb.isKinematic = true;

        transform.SetParent(magazineTransform.parent);

        transform.position = magazineTransform.position;
        transform.rotation = magazineTransform.rotation;

    }
}
