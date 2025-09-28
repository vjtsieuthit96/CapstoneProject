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
    private void Start()
    {
        player = PlayerMock.Instance.PlayerTransform;
    }
    public void setplayer(Transform Player)
    {
        this.player = Player;
    }
    public virtual void Lookatplayer()
    {
        if (player == null) return;
        transform.LookAt(player.position, Vector3.up);
    }

    public void LateUpdate()
    {
        Lookatplayer();
    }
}
