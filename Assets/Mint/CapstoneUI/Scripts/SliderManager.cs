using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Slider slider;
    public float sliderValue { get; private set; }

    private void Start()
    {
        UpdateValueText(slider.value);
    }
    private void OnEnable()
    {
        if(slider != null)
        {
            slider.onValueChanged.AddListener(UpdateValueText);
        }
    }

    private void UpdateValueText(float value)
    {
        float valueToDisplay = (value/slider.maxValue) * 100;
        if (valueText != null)
        {
            valueText.text = $"{Mathf.Round(valueToDisplay)}";
        }
    }
}
