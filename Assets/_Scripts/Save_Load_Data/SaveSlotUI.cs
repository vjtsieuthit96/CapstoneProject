using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotUI : MonoBehaviour
{
    public TMP_Text slotNameText;
    public Button loadButton;
    public Button deleteButton;

    private string slotName;

    public void Setup(string name)
    {
        slotName = name;
        slotNameText.text = name;

        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(() => SaveManager.Instance.LoadFromSlot(slotName));

        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(() =>
        {
            SaveManager.Instance.DeleteSlot(slotName);
            Destroy(gameObject);
        });
    }
}
