using Invector.vCamera;
using System.Collections;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    public GameObject player;
    public GameObject m1;
    public GameObject m2;

    public Transform targetPosition;

    public KeyCode teleportKey = KeyCode.T;

    void Update()
    {
        if (Input.GetKeyDown(teleportKey))
        {
            TeleportPlayer();
            m1.SetActive(false);
            m2.SetActive(true);
        }
    }
    private void Start()
    {
        StartCoroutine(AfterStart());
    }

    public void TeleportPlayer()
    {
        if (player != null && targetPosition != null)
        {
            player.transform.position = targetPosition.position;
            player.transform.rotation = targetPosition.rotation;
        }
    }
    IEnumerator AfterStart()
    {
        yield return new WaitForSeconds(0.2f);

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
            {
                player = foundPlayer;
            }
        }
    }
}
