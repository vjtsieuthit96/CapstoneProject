using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTimeLine : MonoBehaviour
{
    public Material SkyboxDay;
    public Material SkyboxNight;

    public AudioSource AudioSource_1;
    public AudioSource AudioSource_2;

    public AudioClip MorningClip;
    public AudioClip DayClip;
    public AudioClip EveningClip;
    public AudioClip NightClip;

    public Light directionalLight;

    public float cycleDuration = 60f;
    private float timer = 0f;
    private bool isDay = true;

    void Start()
    {
        SetPhase(true);
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Tính phần trăm chu kỳ (0 → 1)
        float cycleProgress = timer / cycleDuration;
        if (cycleProgress > 1f) cycleProgress = 0f;

        // Quay mặt trời đủ 360° trong 1 ngày
        float sunAngle = Mathf.Lerp(0f, 360f, cycleProgress);
        directionalLight.transform.rotation = Quaternion.Euler(sunAngle - 90f, 170f, 0f);

        // Chuyển pha ngày/đêm mỗi nửa chu kỳ
        if (timer >= cycleDuration)
        {
            timer = 0f;
        }

        if (cycleProgress < 0.5f && !isDay)
        {
            isDay = true;
            SetPhase(true);
        }
        else if (cycleProgress >= 0.5f && isDay)
        {
            isDay = false;
            SetPhase(false);
        }
    }

    void SetPhase(bool dayPhase)
    {
        AudioSource_1.Stop();
        AudioSource_2.Stop();

        if (dayPhase)
        {
            RenderSettings.skybox = SkyboxDay;

            AudioSource_1.clip = MorningClip;
            AudioSource_2.clip = DayClip;
            AudioSource_1.Play();
            AudioSource_2.Play();
        }
        else
        {
            RenderSettings.skybox = SkyboxNight;

            AudioSource_1.clip = EveningClip;
            AudioSource_2.clip = NightClip;
            AudioSource_1.Play();
            AudioSource_2.Play();
        }
    }
}