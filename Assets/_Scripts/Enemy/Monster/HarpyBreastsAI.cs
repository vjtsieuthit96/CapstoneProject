using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class HarpyBreastsAI : MonsterAI
{
    [Header("-----Addon Components------")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform catchPoint;
    private bool isCatch;
    private bool isFlying;   
    private bool isLanding;
    private bool isTakeOff;
    private float monsterHeight;
    private bool isFalling;
    private bool isFlyToPlayer;
    private float riseVelocity = 0f;
    private float fallVelocity = 0f;


    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Không tìm thấy GameObject có tag 'Player'");
        }
        monsterHeight = monsterAgent.height;
    }
    protected override void Update()
    {
        base.Update();
        if (isFlying) {SetAnimatorParameter(MonsterAnimatorHash.isFlyingHash, true);}
        else {SetAnimatorParameter(MonsterAnimatorHash.isFlyingHash, false); }      
        Landing();
        if (isCatch)
        {
            monsterAgent.baseOffset = Mathf.SmoothDamp(monsterAgent.baseOffset, 20f, ref riseVelocity,2f);
            monsterAgent.height = monsterHeight + monsterAgent.baseOffset;
        }
        if (isTakeOff)
        {
            monsterAgent.baseOffset = Mathf.SmoothDamp(monsterAgent.baseOffset, 15f, ref riseVelocity, 3f);
            monsterAgent.height = monsterHeight + monsterAgent.baseOffset;
        }
        if (isFlyToPlayer)
        {
            monsterAgent.baseOffset = Mathf.SmoothDamp(monsterAgent.baseOffset, 1.15f, ref fallVelocity, 1.5f);
            monsterAgent.height = monsterHeight + monsterAgent.baseOffset;
        }
    }

    protected override Node CreateBehaviorTree()
    {
        return new Selector(new List<Node>
        {
        new CatchPreyNode(this, monsterAgent), 

        new Sequence(new List<Node>
        {
            new CheckPlayerInFOVNode(this),            
        }),
        new PatrolNode(this, monsterAgent)
        });
    }
    public void SetIsFalling(bool value)
    {
        isFalling = value;
    }
    public bool IsFalling()
    {
        return isFalling;
    }

    #region Catch&Release


    public void CatchPrey()
    {
        if (!isCatch)
        {
            isCatch = true;        
            player.transform.position = catchPoint.position;                      
            FixedJoint joint = this.AddComponent<FixedJoint>();
            joint.connectedBody = player.GetComponent<Rigidbody>();
            joint.massScale = 0.01f; // Giảm trọng lượng của Joint để không làm Player bị kéo xuống quá nhanh
            joint.connectedMassScale = 0.01f; // Giảm trọng lượng của Player khi bị giữ       
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

    #endregion

    #region Fly&Land
    public bool IsFlying() => isFlying;
    public void SetIsFlying(bool value)
    {
        isFlying = value;
    }
    public void TakeOff()
    {
        isTakeOff = true;
    }
    public void DoneTakeOff()
    {
        isTakeOff = false;
    }
    public void FlyToPlayer(bool value)
    {
        isFlyToPlayer = value;       
    }

    private void Landing()
    {
        if (isLanding)
        {
            if (monsterAgent.enabled && monsterAgent.baseOffset > 0.15f)
            {
                monsterAgent.baseOffset = Mathf.Lerp(monsterAgent.baseOffset, 0f, Time.deltaTime*1.5f);
                monsterAgent.height = monsterHeight + monsterAgent.baseOffset;
            }
            else
            {
                SetAnimatorParameter(MonsterAnimatorHash.landHash,null);
                monsterAgent.baseOffset = 0f;
                monsterAgent.height = monsterHeight;
                isLanding = false;               
            }
        }
    }
    public void SetIsLanding()
    {
        isLanding = true;
    }
    
    #endregion
}



