using UnityEngine;

public class SceneIndexManager : MonoBehaviour
{
    public static SceneIndexManager Instance { get; private set; }

    [Header("Giá trị index (0 - 1 - 2) sẽ set trong Inspector ở scene Intro")]
    [SerializeField] private int selectedIndex;
    public int SelectedIndex => selectedIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SetIndex(int index)
    {
        selectedIndex = index;
        Debug.Log("Index được set thành: " + selectedIndex);
    }
}
