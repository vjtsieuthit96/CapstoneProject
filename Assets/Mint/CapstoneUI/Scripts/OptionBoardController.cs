using Invector.Utils;
using System;
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
    [Header("Button")]
    [SerializeField] Button settingBtn; // Array of buttons to control the panels

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
                panelDictionary.Add(showUIPanel[i].panelType, showUIPanel[i]); // Add each UIPanel to the dictionary with its corresponding PanelType
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingBtn.onClick.AddListener(() => FadeIn(PanelType.Setting)); // Add listener to the button to call FadeIn with PanelType.Option
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
            panelDictionary[panelType].FadeIn(); // Fade in the selected panel\

            if(panelDictionary[panelType].clickSound)
                SoundMixerManager.Instance.PlaySFXAudio(panelDictionary[panelType].clickSound); // Play the click audio when fading in
        }
        else
        {
            Debug.LogError("PanelType not found in dictionary: " + panelType);
        }
       
    }
    public PanelType GetPanelFadein()
    {
        return PanelType.None; 
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
    }

    public void FadeOut()
    {
        foreach (var panel in showUIPanel)
        {
                panel.FadeOut(); // Fade out all other panels
        }
    }
}
