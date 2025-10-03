using UnityEngine;
using UnityEngine.UI;

public class CheatGame : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite cheatSprite;
    [SerializeField] private bool isCheatMode = false;
    [SerializeField] private CharacterConfigurator characterConfigurator;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip cheatOnSound;
    [SerializeField] private AudioClip cheatOffSound;
    private AudioSource audioSource;

    private bool isSearching = true;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        isCheatMode = PlayerRealTimeData.Instance.isCheat;
        ApplyCheatUI(false);
    }

    private void Update()
    {
        PlayerRealTimeData.Instance.isCheat = isCheatMode;

        if (isSearching)
        {
            characterConfigurator = FindObjectOfType<CharacterConfigurator>();
            if (characterConfigurator != null)
            {
                isSearching = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            ToggleCheat();
        }
    }

    private void ToggleCheat()
    {
        isCheatMode = !isCheatMode;
        PlayerRealTimeData.Instance.isCheat = isCheatMode;
        ApplyCheatUI(true);
    }

    private void ApplyCheatUI(bool playSound)
    {
        if (targetImage != null)
        {
            targetImage.sprite = isCheatMode ? cheatSprite : normalSprite;
            targetImage.color = isCheatMode ? Color.red : Color.white;
        }

        if (!playSound) return;

        if (isCheatMode && cheatOnSound != null)
        {
            audioSource.PlayOneShot(cheatOnSound);
        }
        else if (!isCheatMode && cheatOffSound != null)
        {
            audioSource.PlayOneShot(cheatOffSound);
        }
    }
}
