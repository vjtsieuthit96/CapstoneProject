using Invector.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class OptionBoardController : MonoBehaviour
{ 
    private vFadeCanvas fadeCanvas; // Reference to the vFadeCanvas component
    [Header("Panel")]
    [SerializeField] UIPanel[] showUIPanel;
    private Dictionary<PanelType, UIPanel> panelDictionary; // Dictionary to map PanelType to UIPanel
    public PanelType currentPanelType;
    public bool isPanelChildActing = false;
    [Header("Button")]
    [SerializeField] Button settingBtn; // Array of buttons to control the panels
    [SerializeField] Button[] backToOptionBtns; // Button to go back to the previous panel
    [SerializeField] Button skillTreeBtn; // Button to close the current panel

    private void Awake()
    {
        fadeCanvas = GetComponent<vFadeCanvas>(); // Try to get the vFadeCanvas component from the GameObject this script is attached to
        panelDictionary = new Dictionary<PanelType, UIPanel>(); // Initialize the dictionary to hold UIPanels
        for (int i = 0; i < showUIPanel.Length; i++)
        {
            if (panelDictionary.ContainsKey(showUIPanel[i].panelType))
            {
                Debug.LogError("Duplicate UIPanel found in showUIPanel array: " + showUIPanel[i].name);
            }
            else
            {
                panelDictionary.Add(showUIPanel[i].panelType, showUIPanel[i]);              
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Add listener to the button 
        if(settingBtn)
            settingBtn.onClick.AddListener(() => FadeIn(PanelType.Setting));
        foreach (var btn in backToOptionBtns)
        {
            btn.onClick.AddListener(() => FadeIn(PanelType.Option)); // Add listener to each button to go back to the Option panel
        }
        skillTreeBtn.onClick.AddListener(() => FadeIn(PanelType.SkillTree));
    }

    public void FadeIn(PanelType panelType)
    {
        if(panelDictionary.ContainsKey(panelType))
        {
            foreach (var panel in showUIPanel)
            {
                panel.FadeOut(); // Fade out all panels before showing the selected one
                panel.gameObject.SetActive(false); // Hide all panels

            }
           
            panelDictionary[panelType].gameObject.SetActive(true); // Show the selected panel
            StartCoroutine(FadeInNextFrame(panelDictionary[panelType], 1)); // Start fading in the selected panel

            if (panelDictionary[panelType].clickSound)
                SoundMixerManager.Instance.PlaySFXAudio(panelDictionary[panelType].clickSound); // Play the click audio when fading in
        }
        else
        {
            Debug.LogError("PanelType not found in dictionary: " + panelType);
        }
        currentPanelType = panelType; // Update the current panel type
        isPanelChildActing = false;
    }

    public void OptionFadeIn()
    { 

    }

    public void FadeOut(PanelType panelType)
    {
        if(panelDictionary.ContainsKey(panelType))
        {
            panelDictionary[panelType].FadeOut(); // Fade in the selected panel
        }
        else
        {
            Debug.LogError("PanelType not found in dictionary: " + panelType);
        }
        isPanelChildActing = false; // Reset the acting state
    }

    public void FadeOut()
    {
        foreach (var panel in showUIPanel)
        {
          panel.FadeOut(); // Fade out all other panels
        }
    }

    private IEnumerator FadeInNextFrame(UIPanel panel,int index)
    {
        yield return null; // Wait for the next frame

        if (index == 1)
            panel.FadeIn();
        else
            panel.FadeOut(); 
    }
}
    