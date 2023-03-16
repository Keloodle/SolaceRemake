using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public Slider barSlider;
    public Gradient gradient;
    public Image fill;

    public float maxHP = 10;
    public float cHP = 10;

    void Awake()
    {
        barSlider = this.GetComponent<Slider>();
        barSlider.maxValue = maxHP;
        barSlider.value = cHP;
    }

    void Update()
    {
        fill.color = gradient.Evaluate(barSlider.normalizedValue);
    }

    public void adjustHealth(float value)
    {
        barSlider.value += value;
    }

}
