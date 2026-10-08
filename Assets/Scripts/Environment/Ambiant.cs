using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambiant : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.PlayEvent("Play_Environment_Ambiant", gameObject);
    }
}
