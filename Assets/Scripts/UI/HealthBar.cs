using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private TMP_Text healthText;

    private void SetHealtText(int x, int total)
    {
        //Debug.Log(x);
        healthText.text = x + "/" + total;
    }

    private int GetHealthText()
    {
        string[] temp = healthText.text.Split('/');
        return int.Parse(temp[0]);
    }

    public void HealthBarAnimation(int newInt, float newFloat, int totalValue, float time, Action onComplete)
    {
        DOTween.To(() => GetHealthText(), x => SetHealtText(x, totalValue), newInt, time);

        DOTween.To(() => healthSlider.value, x => healthSlider.value = x, newFloat, time).SetEase(Ease.InQuad).OnComplete(() => 
        {
            if (onComplete != null)
                onComplete();
        });
    }
}
