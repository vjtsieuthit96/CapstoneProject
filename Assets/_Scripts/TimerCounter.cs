using UnityEngine;
using UnityEngine.UI;

public class TimerCounter : MonoBehaviour
{
    public Text timerText;

    [Header("Thời gian bắt đầu (giờ : phút : giây)")]
    public int startHours = 0;
    public int startMinutes = 0;
    public int startSeconds = 0;

    private int hours;
    private int minutes;
    private int seconds;

    [Header("Thiết lập tốc độ đếm")]
    public int ticksPerSecond = 10;
    private int tickCounter = 0;
    private float tickTimer = 0f;
    public float tickInterval = 0.1f;

    [Header("Hiệu ứng blink dấu :")]
    private float blinkTimer = 0f;
    public float blinkInterval = 0.5f;
    private bool showColon = true;

    void Start()
    {
        hours = startHours;
        minutes = startMinutes;
        seconds = startSeconds;
        UpdateTimerText();
    }

    void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= tickInterval)
        {
            tickTimer -= tickInterval;
            tickCounter++;
            if (tickCounter >= ticksPerSecond)
            {
                tickCounter = 0;
                seconds++;

                if (seconds >= 60)
                {
                    seconds = 0;
                    minutes++;
                }

                if (minutes >= 60)
                {
                    minutes = 0;
                    hours++;
                }
            }
        }
        blinkTimer += Time.deltaTime;
        if (blinkTimer >= blinkInterval)
        {
            blinkTimer = 0f;
            showColon = !showColon;
        }
        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        string colon = showColon ? ":" : " ";
        timerText.text = string.Format("{0:00}{3}{1:00}{3}{2:00}", hours, minutes, seconds, colon);
    }
}
