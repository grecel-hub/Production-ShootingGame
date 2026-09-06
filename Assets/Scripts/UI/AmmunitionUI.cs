using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmunitionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI maxAmmunition;
    [SerializeField] private TextMeshProUGUI currentAmmunition;
    [SerializeField] private TextMeshProUGUI slash;


    [Header("Alpha Info")] //透明度控制参数
    [SerializeField] private float fadeDuration;
    private float alpha = 0f;
    private float fadeElapsed = 0f;

    private void Start()
    {
        
    }

    private void Update()
    {
        maxAmmunition.alpha = alpha;
        currentAmmunition.alpha = alpha;
        slash.alpha = alpha;

        AutoFade();
    }

    public void AutoFade()
    {
        fadeElapsed += Time.deltaTime;

        alpha = Mathf.Lerp(1, 0f, fadeElapsed / fadeDuration);

        if (fadeElapsed >= fadeDuration)
            alpha = 0f;
    }

    public void Show()
    {
        alpha = 1f;
        fadeElapsed = 0f;
    }

    public void SetAmmunitionText((int current, int max) size)
    {
        maxAmmunition.text = size.max.ToString();
        currentAmmunition.text = size.current.ToString();
    }
}
