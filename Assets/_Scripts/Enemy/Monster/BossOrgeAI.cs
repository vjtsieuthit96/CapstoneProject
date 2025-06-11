using System.Collections.Generic;
using UnityEngine;

public class BossOrgeAI : MonsterAI
{
    private float smoothTime = 0.1f;
    private float turnVelocity = 0f;
    protected override void Start()
    {
        base.Start();      

    }   
    protected override void Update()
    {
        base.Update();
        TurnAmount();
    }
    protected override Node CreateBehaviorTree()
    {
        return new Selector(new List<Node>
    {
        new Sequence(new List<Node> //  Nếu máu thấp, AI retreat rồi tiếp tục hành vi khác
        {
            new CheckRetreatNode(this, monsterStats),
            new RetreatNode(this, monsterAgent)
        }),
        new Sequence(new List<Node> //  Nếu thấy Player, AI chiến đấu
        {
            new CheckPlayerInFOVNode(this),
            new RoarNode(this, monsterAudio),
            new ChaseNode(this, monsterAgent),
            new Selector(new List<Node>
            {
                new SkillUsageNode(this, skillManager),
                new MeleeAttackNode(this)
            })
        }),
        new PatrolNode(this, monsterAgent) //  Nếu mất dấu Player hoàn toàn, AI tuần tra lại
    });
    }

    private void TurnAmount()
    {
        // 1. Lấy tốc độ quay hiện tại từ NavMeshAgent
        float angularSpeed = monsterAgent.angularSpeed;

        // 2. Xác định hướng quay (trái/phải) bằng Dot Product
        float turnDirection = Vector3.Dot(monsterAgent.velocity.normalized, transform.right);

        // 3. Chuẩn hóa giá trị về khoảng -1 → 1
        float turnValue = Mathf.Clamp(turnDirection * (angularSpeed / monsterAgent.angularSpeed), -1f, 1f);

        // 4. Làm mượt giá trị để tránh giật animation
        float smoothTurn = Mathf.SmoothDamp(GetFloatAnimatorParameter(MonsterAnimatorHash.turnHash), turnValue, ref turnVelocity, smoothTime);

        // 5. Cập nhật vào Animator
        SetAnimatorParameter(MonsterAnimatorHash.turnHash, smoothTurn);
    }
}
