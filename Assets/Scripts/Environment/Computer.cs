using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.PlayEvent("Play_Environment_Computer", gameObject);
    }
}
