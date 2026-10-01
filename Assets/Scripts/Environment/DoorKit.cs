using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorKit : MonoBehaviour
{
    [SerializeField] private Door[] doors;

    Player player;

    private void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            player = other.GetComponent<Player>();

            player.SetDoorKit(this);
            player.canSwitchDoor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (player != null)
        {
            player.SetDoorKit(null); 
            player.canSwitchDoor = false;
            player = null;
        }
    }

    public void SwitchDoor()
    {
        foreach(Door door in doors)
        {
            door.SwitchDoor();
        }
    }
}
