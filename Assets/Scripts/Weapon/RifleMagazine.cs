using System.Collections;
using UnityEngine;

public class RifleMagazine : Magazine
{
    protected override void Update()
    {
        base.Update();
    }

    protected override IEnumerator Return()
    {
        yield return new WaitForSeconds(10f);

        transform.SetParent(RifleMagazinePool.instance.transform);

        canReturn = false;

        RifleMagazinePool.instance.Return(this);
    }
}
