using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    [Header("Quit")]
    [SerializeField] private Button quitBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button yesBtn;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private GameObject quitPanel;
    [SerializeField] private TextMeshProUGUI quitText;
    [SerializeField] private TextMeshProUGUI quitCommentText;
    [Header("Audio Settings")]
    [SerializeField] private Button audioBtn;
    [SerializeField] private GameObject AudioPanel;

    [Header("Key Settings")]
    [SerializeField] private Button keyBindingsBtn;
    [SerializeField] private GameObject KeyBindingsPanel;

    [SerializeField] private GameObject[] panels;

    private void OnEnable()
    {
        audioBtn.onClick.AddListener(ToggleAudioPanel);
        keyBindingsBtn.onClick.AddListener(ToggleKeyBindingsPanel);
        quitBtn.onClick.AddListener(ShowQuitGamePanel);
        exitBtn.onClick.AddListener(ShowExitGamepPanel);
        cancelBtn.onClick.AddListener(Cancel);
        quitPanel.SetActive(false);
    }

    
    private void Start()
    {

        audioBtn.Select();
        audioBtn.onClick.AddListener(ToggleAudioPanel);
        keyBindingsBtn.onClick.AddListener(ToggleKeyBindingsPanel);
        quitBtn.onClick.AddListener(ShowQuitGamePanel);
        exitBtn.onClick.AddListener(ShowExitGamepPanel);
        cancelBtn.onClick.AddListener(Cancel);
    }

    private void Cancel()
    {
        SoundMixerManager.Instance.PlaySFXAudio(clickSound);
        quitPanel.SetActive(false);
    }
    private void ShowExitGamepPanel()
    {
        quitPanel.SetActive(true);
        quitText.text = "Exit To Main Menu";
        quitCommentText.text = "Are you sure you want to exit to the main menu? Your progress will not be saved.";
        yesBtn.onClick.RemoveAllListeners();
        yesBtn.onClick.AddListener(() =>
        {
            SoundMixerManager.Instance.PlaySFXAudio(clickSound);
            quitPanel.SetActive(false);
            Debug.Log("Exit To Main menu");
            // TODO ADD MainMenu Scene
        });
        cancelBtn.onClick.AddListener(Cancel);
    }


    private void ShowQuitGamePanel()
    {
        quitPanel.SetActive(true);
        quitText.text = "Quit Game";
        quitCommentText.text = "Are you sure you want to quit the game? Your progress will not be saved.";
        yesBtn.onClick.RemoveAllListeners();
        yesBtn.onClick.AddListener(() =>
        {
            SoundMixerManager.Instance.PlaySFXAudio(clickSound);
            Application.Quit();
        });
     }

    private void ToggleKeyBindingsPanel()
    {
        if (KeyBindingsPanel.activeSelf)
        {
            return;
        }
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
                return;
            }
        }
        KeyBindingsPanel.SetActive(true);
        SoundMixerManager.Instance.PlaySFXAudio(clickSound);
    }

    private void ToggleAudioPanel()
    {
        if(AudioPanel.activeSelf)
        {
            return;
        }
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
                return;
            }
        }
        AudioPanel.SetActive(true);
        SoundMixerManager.Instance.PlaySFXAudio(clickSound);
    }
}
