using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private string saveFolder;
    private string currentSlotId;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            saveFolder = Path.Combine(Application.persistentDataPath, "Saves");
            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);

            SceneManager.activeSceneChanged += OnSceneChanged;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        TryAutoSave();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    public List<string> GetAllSaveFiles()
    {
        if (!Directory.Exists(saveFolder))
            return new List<string>();

        string[] files = Directory.GetFiles(saveFolder, "*.json");
        List<string> fileNames = new List<string>();
        foreach (var f in files)
            fileNames.Add(Path.GetFileNameWithoutExtension(f));

        fileNames.Sort((a, b) =>
        {
            int aNum = ParseLoadNumber(a);
            int bNum = ParseLoadNumber(b);
            return aNum.CompareTo(bNum);
        });

        return fileNames;
    }

    private int ParseLoadNumber(string name)
    {
        if (name.StartsWith("load"))
        {
            string num = name.Substring(4);
            if (int.TryParse(num, out int n))
                return n;
        }
        return 0;
    }

    private string GetSavePath(string slotName)
    {
        return Path.Combine(saveFolder, slotName + ".json");
    }

    public void SaveGame()
    {
        if (string.IsNullOrEmpty(currentSlotId))
        {
            int next = GetAllSaveFiles().Count + 1;
            currentSlotId = "load" + next;
        }

        PlayerRealTimeData.Instance.SaveToJson();

        string src = Path.Combine(Application.persistentDataPath, "PlayerRealTimeData.json");
        string dest = GetSavePath(currentSlotId);
        File.Copy(src, dest, true);

        Debug.Log("Game saved: " + currentSlotId);
    }

    private void OnSceneChanged(Scene prev, Scene next)
    {
        int buildIndex = next.buildIndex;

        if (prev.buildIndex == 7 || prev.buildIndex == 8 || prev.buildIndex == 9 || prev.buildIndex == 10)
        {
            SaveGame();
        }
    }

    private void TryAutoSave()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (buildIndex == 7 || buildIndex == 8 || buildIndex == 9 || buildIndex == 10)
        {
            SaveGame();
        }
    }

    public void LoadFromSlot(string slotId)
    {
        string path = GetSavePath(slotId);
        if (!File.Exists(path))
        {
            Debug.LogWarning("Save file not found: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "PlayerRealTimeData.json"), json);
        PlayerRealTimeData.Instance.LoadFromJson();

        currentSlotId = slotId;
        Debug.Log("Game loaded: " + slotId);
        SceneManager.LoadScene(11);
    }

    public void DeleteSlot(string slotId)
    {
        string path = Path.Combine(saveFolder, slotId + ".json");
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Deleted save slot: " + slotId);
            if (currentSlotId == slotId)
                currentSlotId = null;
        }
        else
        {
            Debug.LogWarning("Delete failed. Save file not found: " + slotId);
        }
    }

}
