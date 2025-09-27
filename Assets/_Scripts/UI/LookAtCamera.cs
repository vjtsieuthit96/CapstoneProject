using Invector.vCamera;
using System.Collections;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform player;

    private void OnEnable()
    {
       player = PlayerMock.Instance.PlayerTransform;
    }
   
    void LateUpdate()
    {
        if (player == null) return;
        transform.LookAt(player.position, Vector3.up);
    }
}
