using UnityEngine;

public class LookAtPlayer : LookAtCamera
{
    private void LateUpdate()
    {
        base.LateUpdate();
    }
    public override void Lookatplayer()
    {
        if (player == null) return;
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPosition, Vector3.up);
    }
}
