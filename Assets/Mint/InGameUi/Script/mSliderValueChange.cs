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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       sliderBarImage.color = healthGradient.Evaluate(sliderBarImage.fillAmount);
    }

}
