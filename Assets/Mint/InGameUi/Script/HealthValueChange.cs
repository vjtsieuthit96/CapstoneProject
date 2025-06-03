using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class HealthValueChange : MonoBehaviour
{
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private Image healthBarImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       healthBarImage.color = healthGradient.Evaluate(healthBarImage.fillAmount);
    }
}
