using System.Collections;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    protected Rigidbody rb;

    protected bool canReturn;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
    }

    protected virtual void Update()
    {
        if (canReturn)
            StartCoroutine(Return());
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

        canReturn = true;
    }

    //安装弹匣
    public void InstallMagazine(Transform magazineTransform)
    {
        rb.isKinematic = true;

        transform.SetParent(magazineTransform.parent);

        transform.position = magazineTransform.position;
        transform.rotation = magazineTransform.rotation;

    }

    //
    protected virtual IEnumerator Return()
    {
        yield return null;
    }
}
