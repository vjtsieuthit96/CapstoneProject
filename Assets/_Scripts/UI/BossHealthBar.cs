using Invector.Utils;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [Header("First Time Active Sound")]
    [SerializeField] private AudioSource iconSource;
    [SerializeField] private AudioSource sliderSource;
    [SerializeField] private AudioClip iconSound;
    [SerializeField] private AudioClip sliderSound;
    [SerializeField] private Image iconImage;
    private Material iconMaterial;
    [SerializeField] private vFadeCanvas panel;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private MonsterStats monsterStats;
    [Header("Speed")]
    [SerializeField] private float healthSliderMaxValueSmooth = 0.5f; // Smoothness of the health bar max value change
    [SerializeField] private float healthSliderValueSmooth = 0.3f; // Smoothness of the health bar value change

    [SerializeField] private float healthSliderDuration = 2f; // Speed of the health bar filling up

    [SerializeField] private float noiseDuration = 3f; // Speed of the noise effect fading out

    [SerializeField] private float noiseBegin = 1f;

    private bool isActive = false;
    private bool isFirstTime = true;


    private void OnDisable()
    {
        if (panel)
        {
            panel.AlphaZero();
        }
        isActive = false;
        if (iconMaterial)
        {
            iconMaterial.SetFloat("_NoiseValue", 0f);
        }
    }
    private void OnEnable()
    {
       
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        iconMaterial = iconImage.material;
        if (iconMaterial == null)
        {
            Debug.LogError("Icon material is not assigned or missing.");
        }
        if (healthSlider && monsterStats)
        {

            if (monsterStats.GetMaxHealth() != healthSlider.maxValue)
            {
                healthSlider.maxValue = monsterStats.GetMaxHealth();
                healthSlider.onValueChanged.Invoke(healthSlider.value);

            }
        }
        else
        {
            Debug.LogError("Health Slider or MonsterStats is not assigned.");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            UpdateSlider();
        }    
        if(Input.GetKeyDown(KeyCode.M) && !isActive)
        {
            HealthBarOn();
        }
    }

    public void HealthBarOn()
    {
        panel.FadeIn();
        if(!isFirstTime)
        {
            iconMaterial.SetFloat("_NoiseValue", 0f);
            isActive = true;
            return;
        }

        if (healthSlider)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = 1;
            healthSlider.value = 0;
        }    
       StartCoroutine(HealthBarSliderStart());
       StartCoroutine(HealthBarIconStart());
        isFirstTime = false;
    }

    IEnumerator HealthBarSliderStart()
    {
        yield return null;
        sliderSource.clip = iconSound;
        sliderSource.Play();
        if (sliderSound)
        {
            SoundMixerManager.Instance.PlaySFXAudio(sliderSound);
        }
        // Phase 1: Fill healthSlider over 2 seconds
        float duration = healthSliderDuration;
        float healthElapsed = 0f;
        float startValue = 0;
        float targetValue = healthSlider.maxValue;

        while (healthElapsed < duration)
        {
            healthElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(healthElapsed / duration);
            healthSlider.value = Mathf.Lerp(startValue, targetValue, t);
            yield return null;
        }
        healthSlider.value = targetValue;
       sliderSource.Stop();
    }

    IEnumerator HealthBarIconStart()
    {
        iconSource.clip = iconSound;
        iconSource.Play();

        iconImage.gameObject.SetActive(false);
        iconMaterial.SetFloat("_NoiseValue", 0);
        //delay, slider will show first
        yield return new WaitForSeconds(0.2f);   
        
        float noiseValue = noiseBegin;

        iconImage.gameObject.SetActive(true);
        iconMaterial.SetFloat("_NoiseValue", noiseValue);

        // Phase 2: Reduce noiseValue to 0 over 2 seconds
        float duration = noiseDuration;
        float noiseElapsed = 0f;
        float startNoise = noiseValue;
        float targetNoise = 0f;

        while (noiseElapsed < duration)
        {
            noiseElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(noiseElapsed / duration);
            noiseValue = Mathf.Lerp(startNoise, targetNoise, t);
            iconMaterial.SetFloat("_NoiseValue", noiseValue);
            yield return null;
        }
        iconMaterial.SetFloat("_NoiseValue", 0f);

        isActive = true;
        Debug.Log("Boss Health Bar is active");
        iconSource.Stop();
    }    
    private void UpdateSlider()
    {
        if (healthSlider && monsterStats)
        {
            if (monsterStats.GetMaxHealth() != healthSlider.maxValue)
            {
                healthSlider.maxValue = Mathf.Lerp(healthSlider.maxValue, monsterStats.GetMaxHealth(), healthSliderMaxValueSmooth * Time.fixedDeltaTime);
                healthSlider.onValueChanged.Invoke(healthSlider.value);
            }
            healthSlider.value = Mathf.Lerp(healthSlider.value, monsterStats.GetCurrentHealth(), healthSliderValueSmooth * Time.fixedDeltaTime);
        }
    }    
}
