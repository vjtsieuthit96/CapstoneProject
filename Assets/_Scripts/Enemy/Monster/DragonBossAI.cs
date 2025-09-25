using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBossAI : MonsterAI
{
    private bool isFlying = true;
    private bool isLanding;
    public bool IsLanding() => isLanding;
    public bool IsFlying() => isFlying;

    public void SetFlying(bool value)
    {
        isFlying = value;
    }
    public void SetLanding(bool value)
    {
        isLanding = value;
    }
    public float flyTimer;
    public float maxFlyTime = 360f;
    public float walkTimer;
    public float maxWalkTimer = 90f;
    public float flyHoverRadius = 10f;
    public float hoverTimer = 0f;
    public float maxHoverTime = 15f;
    public Vector3 currenHoverPos;
    public bool hasHoverTarget = false;
    private bool landing;
    private bool takeoff;
   

    protected override void Start()
    {
        base.Start();
        RepeatEvaluateBehaviorTree(0f, 1f);
    }
    protected override void Update()
    {
        base.Update();   
        
        if (hasHoverTarget)
        {
            hoverTimer += Time.deltaTime;
        }        
        if (isFlying)
        {
            walkTimer = 0f;
            flyTimer += Time.deltaTime;            
        }
        if(isLanding)
        {
            walkTimer += Time.deltaTime;
            flyTimer = 0f;
        }
        if (walkTimer >= maxWalkTimer && !isFlying)
        {
            isLanding = false;
            isFlying = true;
            monsterAgent.height = 10f;
            SetAnimatorParameter(MonsterAnimatorHash.isFlyingHash, true);
            SetAnimatorParameter(MonsterAnimatorHash.isLandingHash, false);
        }
        AnimatorStateInfo stateInfo = monsterAnimator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.shortNameHash == MonsterAnimatorHash.LandHash_3)
        {
            landing = true;
        }
        if (stateInfo.shortNameHash == MonsterAnimatorHash.TakeoffHash_1)
        {
            takeoff = true;
        }
        if (landing)
        {
            StartOffsetLerp(0f, 1.5f);
            landing = false;
        }
        if(takeoff)
        {
            StartOffsetLerp(8f, 1.5f);
            takeoff = false;
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
                new SkillUsageNode(this, skillManager),
                new DragonChaseNode(this, monsterAgent),
                new MeleeAttackNode(this)
            }),
         }),
        new PatrolNode(this, monsterAgent) //  Nếu mất dấu Player hoàn toàn, AI tuần tra lại
    });
    }
    public void StartOffsetLerp(float targetOffset, float duration)
    {
        StartCoroutine(LerpBaseOffset(targetOffset, duration));
    }
    public void DeadOffsetLerp()
    {
        StartCoroutine(LerpBaseOffset(0,1f));
    }

    private IEnumerator LerpBaseOffset(float targetOffset, float duration)
    {
        float startOffset = monsterAgent.baseOffset;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            monsterAgent.baseOffset = Mathf.Lerp(startOffset, targetOffset, t);
            yield return null;
        }
        monsterAgent.baseOffset = targetOffset; 
    }

}
