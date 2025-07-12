using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HarpyBreastsAI : MonsterAI
{
    [Header("-----Addon Components------")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform catchPoint;
    private bool isCatch;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Không tìm thấy GameObject có tag 'Player'");
        }
    }
    protected override void Update()
    {
        base.Update();
        if (Input.GetKey(KeyCode.Keypad1))
        {
            CatchPrey();
        }
        if (Input.GetKey(KeyCode.Keypad2))
        {
            ReleasePrey();
        }
    }

    protected override Node CreateBehaviorTree()
    {
        return new Sequence(new List<Node> //  Nếu máu thấp, AI retreat rồi tiếp tục hành vi khác
        {
            new CheckRetreatNode(this, monsterStats),
            new RetreatNode(this, monsterAgent)
        });
    }

    public void CatchPrey()
    {
        if (!isCatch)
        {
            isCatch = true;
            player.transform.position = catchPoint.position;
            FixedJoint joint = this.AddComponent<FixedJoint>();
            joint.connectedBody = player.GetComponent<Rigidbody>();
        }
    }
    public void ReleasePrey()
    {
        FixedJoint joint = GetComponent<FixedJoint>();
        if (joint != null)
        {
            Destroy(joint);
            isCatch = false;
        }    
    }
}



