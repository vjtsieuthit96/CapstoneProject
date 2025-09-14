using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("Music 2D")]
    public Button music2DIncreaseButton;
    public Button music2DDecreaseButton;
    public RawImage[] musicImages2D;

    [Header("Music 3D")]
    public Button music3DIncreaseButton;
    public Button music3DDecreaseButton;
    public RawImage[] musicImages3D;

    [Header("SFX 2D")]
    public Button sfx2DIncreaseButton;
    public Button sfx2DDecreaseButton;
    public RawImage[] sfxImages2D;

    [Header("SFX 3D")]
    public Button sfx3DIncreaseButton;
    public Button sfx3DDecreaseButton;
    public RawImage[] sfxImages3D;

    public int musicIndex = 5;
    public int sfxIndex = 5;

    private Color activeColor;
    private Color inactiveColor = Color.white;

    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#00A6FF", out activeColor);
    }

    private void Start()
    {
        music2DIncreaseButton.onClick.AddListener(() => SetMusic(musicIndex + 1));
        music2DDecreaseButton.onClick.AddListener(() => SetMusic(musicIndex - 1));

        music3DIncreaseButton.onClick.AddListener(() => SetMusic(musicIndex + 1));
        music3DDecreaseButton.onClick.AddListener(() => SetMusic(musicIndex - 1));

        sfx2DIncreaseButton.onClick.AddListener(() => SetSfx(sfxIndex + 1));
        sfx2DDecreaseButton.onClick.AddListener(() => SetSfx(sfxIndex - 1));

        sfx3DIncreaseButton.onClick.AddListener(() => SetSfx(sfxIndex + 1));
        sfx3DDecreaseButton.onClick.AddListener(() => SetSfx(sfxIndex - 1));

        UpdateMusicUI();
        UpdateSfxUI();
    }

    private void SetMusic(int newIndex)
    {
        musicIndex = Mathf.Clamp(newIndex, 0, 10);
        UpdateMusicUI();
    }

    private void SetSfx(int newIndex)
    {
        sfxIndex = Mathf.Clamp(newIndex, 0, 10);
        UpdateSfxUI();
    }

    private void UpdateMusicUI()
    {
        UpdateVolumeUI(musicImages2D, musicIndex);
        UpdateVolumeUI(musicImages3D, musicIndex);
    }

    private void UpdateSfxUI()
    {
        UpdateVolumeUI(sfxImages2D, sfxIndex);
        UpdateVolumeUI(sfxImages3D, sfxIndex);
    }

    private void UpdateVolumeUI(RawImage[] images, int index)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = (i < index) ? activeColor : inactiveColor;
        }
    }
}
