using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Vector3 closePosition;
    private Vector3 openPosition;

    private float duration = 1f;

    public bool isOpen;
    public Vector3 openDistance;

    private void Awake()
    {
        closePosition = transform.position;
        openPosition = closePosition + openDistance;
    }

    private void Update()
    {
    }

    public void SwitchDoor()
    {
        if (!isOpen)
            OpenDoor();
        else
            CloseDoor();

        AudioManager.instance.PlayEvent("Play_Environment_Door", gameObject);
    }

    private void OpenDoor()
    {
        transform.DOKill();

        transform.DOLocalMove(openPosition, duration).SetEase(Ease.InOutQuad);

        isOpen = true;
    }

    private void CloseDoor()
    {
        transform.DOKill();

        transform.DOLocalMove(closePosition, duration).SetEase(Ease.InOutQuad);

        isOpen = false;
    }
}
