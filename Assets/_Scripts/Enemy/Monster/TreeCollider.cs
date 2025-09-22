using UnityEngine;

public class TreeCollider : MonoBehaviour
{
    public DarkMagicTree Tree;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Tree.isPlayerInDefenseZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Tree.isPlayerInDefenseZone = false;
        }
    }
}
