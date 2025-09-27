using Invector.vCamera;
using System.Collections;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform player;

    private void OnEnable()
    {
        StartCoroutine(FindPlayerCoroutine());
    }
    private IEnumerator FindPlayerCoroutine()
    {
        while (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                yield break;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
    void LateUpdate()
    {
        if (player == null) return;
        transform.LookAt(player.position, Vector3.up);
    }
}
