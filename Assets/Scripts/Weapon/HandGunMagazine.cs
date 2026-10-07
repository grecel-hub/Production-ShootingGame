using System.Collections;
using UnityEngine;

public class HandGunMagazine : Magazine
{
    protected override void Update()
    {
        base.Update();
    }

    protected override IEnumerator Return()
    {
        yield return new WaitForSeconds(10f);

        transform.SetParent(HandGunMagazinePool.instance.transform);

        canReturn = false;

        HandGunMagazinePool.instance.Return(this);
    }

}
