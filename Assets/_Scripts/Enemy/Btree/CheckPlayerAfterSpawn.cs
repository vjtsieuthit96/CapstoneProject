using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPlayerAfterSpawn : MonoBehaviour
{
    public Transform target;
    public DragonBossAI bossAI;
    private void Awake()
    {
        bossAI.gameObject.SetActive(false);
    }
    void Start()
    {
        StartCoroutine(AfterStart());
    }
    
    IEnumerator AfterStart()
    {
        while (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
                bossAI.gameObject.SetActive (true);
                bossAI.target = target;
                yield break;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
