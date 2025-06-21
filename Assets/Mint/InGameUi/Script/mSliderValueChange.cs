using Invector.Utils;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class mSliderValueChange : MonoBehaviour
{
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private Image sliderBarImage;
    [SerializeField] private Slider sliderGUI;
    private float percentage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(sliderGUI == null)
        {
             TryGetComponent<Slider>(out sliderGUI);
            if(sliderGUI != null)
            {
                sliderBarImage = sliderGUI.fillRect.GetComponent<Image>();
            }
        }
        ChangeColorByPercentage();
    }

    private void ChangeColorByPercentage()
    {
        if(sliderGUI != null)
        {
            percentage = sliderGUI.value / sliderGUI.maxValue;
            sliderBarImage.color = healthGradient.Evaluate(percentage);
        }
    }
}