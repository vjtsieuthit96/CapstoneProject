using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private Button AudioBtn;
    [SerializeField] private GameObject AudioPanel;

    [Header("Key Settings")]
    [SerializeField] private Button KeyBindingsBtn;
    [SerializeField] private GameObject KeyBindingsPanel;

    [SerializeField] private GameObject[] panels;

    private void OnEnable()
    {
        AudioBtn.onClick.AddListener(ToggleAudioPanel);
        KeyBindingsBtn.onClick.AddListener(ToggleKeyBindingsPanel);
    }

    private void Start()
    {

        AudioBtn.Select();
        // Add listeners to buttons
        AudioBtn.onClick.AddListener(ToggleAudioPanel);
        KeyBindingsBtn.onClick.AddListener(ToggleKeyBindingsPanel);
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
        Debug.Log("active");
    }
}
