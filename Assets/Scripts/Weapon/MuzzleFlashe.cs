using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashe : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        MuzzleFlashePool.instance.Return(this);
    }
}
