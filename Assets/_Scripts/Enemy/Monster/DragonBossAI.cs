using System.Collections.Generic;
using UnityEngine;

public class DragonBossAI : MonsterAI
{
    private bool isFlying = true;
    public bool IsFlying() => isFlying;
    public float flyTimer;
    public float maxFlyTime = 360f;
    public float walkTimer;
    public float maxWalkTimer = 90f;
    public float flyHoverRadius = 10f;
    public float hoverTimer = 0f;
    public float maxHoverTime = 15f;
    public Vector3 currenHoverPos;
    public bool hasHoverTarget = false;
    protected override void Start()
    {
        base.Start();
        RepeatEvaluateBehaviorTree(0f, 1f);
    }
    protected override void Update()
    {
        base.Update();    
        if (isFlying)
        {
            flyTimer += Time.deltaTime;            
        }
        else
        {
            walkTimer += Time.deltaTime;
        }
        if (hasHoverTarget)
        {
            hoverTimer += Time.deltaTime;
        }
    }

    protected override Node CreateBehaviorTree()
    {
        return new Selector(new List<Node>
    {
        new Sequence(new List<Node> //  Nếu thấy Player, AI chiến đấu
        {
            new CheckPlayerInRange(this),
            new Selector(new List<Node>
            {
                //new SkillUsageNode(this, skillManager),
                new DragonChaseNode(this, monsterAgent)
                // new AttackMelee();
            }),
         }),
        new PatrolNode(this, monsterAgent) //  Nếu mất dấu Player hoàn toàn, AI tuần tra lại
    });
    }
}
