using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadUIManager : MonoBehaviour
{
    [Header("UI Settings")]
    public Transform contentParent;
    public GameObject slotPrefab;

    private List<GameObject> currentSlots = new List<GameObject>();

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach (var go in currentSlots)
            Destroy(go);
        currentSlots.Clear();
        List<string> slots = SaveManager.Instance.GetAllSaveFiles();
        foreach (var s in slots)
        {
            GameObject slotGO = Instantiate(slotPrefab, contentParent);
            slotGO.GetComponent<SaveSlotUI>().Setup(s);
            currentSlots.Add(slotGO);
        }
    }

    private void OnSaveButtonClicked()
    {
        SaveManager.Instance.SaveGame();

        RefreshUI();
    }
}
