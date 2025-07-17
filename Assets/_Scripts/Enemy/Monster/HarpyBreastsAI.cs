using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HarpyBreastsAI : MonsterAI
{
    [Header("-----Addon Components------")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform catchPoint;
    private bool isCatch;
    public bool isFlying;
    private bool isLanding;
    private float desiredOffset = 0f;
    private float flyingBlendSpeed = 1.5f;

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
        if (isFlying) {SetAnimatorParameter(MonsterAnimatorHash.isFlyingHash, true);}
        else {SetAnimatorParameter(MonsterAnimatorHash.isFlyingHash, false); }
        base.Update();
        AdjustFlyingHeight();
        Landing();
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

    #region Catch&Release

    public void CatchPrey()
    {
        if (!isCatch)
        {
            isCatch = true;
            player.transform.position = catchPoint.position;
            SetAnimatorParameter(MonsterAnimatorHash.CatchHash,null);
            FixedJoint joint = this.AddComponent<FixedJoint>();
            joint.connectedBody = player.GetComponent<Rigidbody>();
        }
    }
    public void ReleasePrey()
    {
        FixedJoint joint = GetComponent<FixedJoint>();
        if (joint != null)
        {
            SetAnimatorParameter(MonsterAnimatorHash.ReleaseHash, null);
            Destroy(joint);
            isCatch = false;
        }    
    }
    #endregion

    #region Fly&Land

    private void AdjustFlyingHeight()
    {
        // Chỉ điều chỉnh khi đang bay và không giữ player
        if (isFlying && !isCatch && player != null && monsterAgent.enabled)
        {

            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            float minOffset = 0.25f;
            float maxOffset = 10f;

            // Clamp giá trị chuẩn hóa để không vượt quá 1
            float t = Mathf.Clamp01(distanceToPlayer / 25f);
            // Tính độ cao mong muốn theo khoảng cách
            desiredOffset = Mathf.Lerp(minOffset, maxOffset, t);
            // Áp dụng độ cao mượt mà bằng Lerp
            monsterAgent.baseOffset = Mathf.Lerp(monsterAgent.baseOffset, desiredOffset, Time.deltaTime * flyingBlendSpeed);
        }        
    }

    private void Landing()
    {
        if (isLanding)
        {
            if (monsterAgent.enabled && monsterAgent.baseOffset > 0.1f)
            {
                monsterAgent.baseOffset = Mathf.Lerp(monsterAgent.baseOffset, 0f, Time.deltaTime*1.5f);              
            }
            else
            {
                SetAnimatorParameter(MonsterAnimatorHash.landHash,null);
                monsterAgent.baseOffset = 0f;
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



