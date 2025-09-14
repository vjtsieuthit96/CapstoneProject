using Unity.VisualScripting;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    private SceneIndexManager Index;
    public int PlayerIndex;
    public GameObject[] Players;

    private void Awake()
    {
        Index = FindAnyObjectByType<SceneIndexManager>();
        PlayerIndex = Index.SelectedIndex;
        SpawnIndexPlayer();
    }

    private void SpawnIndexPlayer()
    {
        switch(PlayerIndex)
        {
            case 0:
                Players[0].SetActive(true);
                Destroy(Players[1]);
                Destroy(Players[2]);
                break;
            case 1:
                Players[1].SetActive(true);
                Destroy(Players[0]);
                Destroy(Players[2]);
                break;
            case 2:
                Players[2].SetActive(true);
                Destroy(Players[1]);
                Destroy(Players[0]);
                break;

        }    
    }    
}
