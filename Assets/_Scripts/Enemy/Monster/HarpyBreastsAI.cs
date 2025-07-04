using Unity.VisualScripting;
using UnityEngine;

public class HarpyBreastsAI : MonsterAI
{
    [Header("-----Addon Components------")]
    [SerializeField] private GameObject Player;
    [SerializeField] private Transform catchPoint;        

    protected override void Start()
    {
        base.Start();
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    protected override void Update()
    {
        base.Update();
    }

    protected override Node CreateBehaviorTree()
    {
        throw new System.NotImplementedException();
    }

    public void CatchPrey()
    {
        FixedJoint joint = this.AddComponent<FixedJoint>();
        joint.connectedBody = Player.GetComponent<Rigidbody>();
    }

    //private void GetTargetToPlayer()
    //{
    //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    //    if (players.Length > 0)
    //    {
    //        GameObject targetPlayer = players[Random.Range(0, players.Length)];           
    //    }
    //}
}



