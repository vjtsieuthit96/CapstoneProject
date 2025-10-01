using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossOrgeAI : MonsterAI
{
    private float smoothTime = 0.1f;
    private float turnVelocity = 0f;
    public GameObject Brigde;
    public MonsterStats OrgeStats;
    public GameObject Wall;
    public GameObject bosscanvas;
    protected override void Start()
    {
        OrgeStats = GetComponent<MonsterStats>();
        Brigde.SetActive(false);
        base.Start();
        RepeatEvaluateBehaviorTree(0f, 1.5f);
    }
    protected override void Update()
    {
        base.Update();
        TurnAmount(); 
        CheckNextScene();
    }
    public override void Die()
    {
        base.Die();
    }
    public void CheckNextScene()
    {
        if(monsterStats.GetCurrentHealth() <= 0)
        {
            Wall.SetActive(false);
            Brigde.SetActive(true);
            bosscanvas.SetActive(false);
        }
    }    
    protected override Node CreateBehaviorTree()
    {
        return new Selector(new List<Node>
    {
        new Sequence(new List<Node> //  Nếu máu thấp, AI retreat rồi tiếp tục hành vi khác
        {
            new CheckLowHealthNode(this, monsterStats),
            new RageNode(this, monsterStats)
        }),
        new Sequence(new List<Node> //  Nếu thấy Player, AI chiến đấu
        {
            new CheckPlayerInFOVNode(this),
            new RoarNode(this, monsterAudio),
            new Selector(new List<Node> 
            {
                new SkillUsageNode(this, skillManager),
                new ChaseNode(this, monsterAgent)                
            }),                          
         }),
        new PatrolNode(this, monsterAgent) //  Nếu mất dấu Player hoàn toàn, AI tuần tra lại
    });
    }

    private void TurnAmount()
    {
        
        float angularSpeed = monsterAgent.angularSpeed;

        
        float turnDirection = Vector3.Dot(monsterAgent.velocity.normalized, transform.right);

        
        float turnValue = Mathf.Clamp(turnDirection * (angularSpeed / monsterAgent.angularSpeed), -1f, 1f);

        
        float smoothTurn = Mathf.SmoothDamp(GetFloatAnimatorParameter(MonsterAnimatorHash.turnHash), turnValue, ref turnVelocity, smoothTime);

       
        SetAnimatorParameter(MonsterAnimatorHash.turnHash, smoothTurn);
    }
}
