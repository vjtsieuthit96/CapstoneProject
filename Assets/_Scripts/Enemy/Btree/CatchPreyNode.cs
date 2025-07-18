using UnityEngine;
using UnityEngine.AI;

public class CatchPreyNode : Node
{
    private HarpyBreastsAI monster;
    private NavMeshAgent agent;
    private Transform player;  

    private enum CatchState { Idle, FlyingToPlayer, Catching, Retreating, Releasing, Hovering }
    private CatchState currentState = CatchState.Idle;

    private Vector3 retreatTarget;
    private float retreatRadius = 35f; // Khoảng cách tối đa để tìm vị trí retreat
    private float catchDistance = 3f;
    private float retreatTargetReachThreshold = 1.5f;

    private float hoverTime;
    private float hoverTimer;



    public CatchPreyNode(HarpyBreastsAI monster, NavMeshAgent agent)
    {
        this.monster = monster;
        this.player = monster.GetTarget();
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        if (!monster.IsInCombat())
        {           
            return NodeState.FAILURE;
        }

        if (monster == null || player == null) return NodeState.FAILURE;       

        if (monster.IsFalling())
        {           
            return NodeState.FAILURE;
        }
        Vector3 monsterPosXZ = new Vector3(monster.transform.position.x, 0f, monster.transform.position.z);      

        switch (currentState)
        {
            case CatchState.Idle:
                if (!monster.IsFlying()) monster.SetIsFlying(true);
                agent.speed = monster.GetBaseSpeed();
                currentState = CatchState.FlyingToPlayer;
                return NodeState.RUNNING;               

            case CatchState.FlyingToPlayer:
                agent.speed = monster.GetBaseSpeed() * monster.GetSpeedMultiplier();
                monster.FlyToPlayer(true);
                agent.SetDestination(player.position);
                Vector3 playerPosXZ = new Vector3(player.position.x, 0f, player.position.z);
                float distance = Vector3.Distance(monsterPosXZ, playerPosXZ);

                if (distance < catchDistance && !monster.IsCatch())
                {
                    Debug.Log("Bắt prey!");
                    monster.SetAnimatorParameter(MonsterAnimatorHash.CatchHash, null);
                    monster.FlyToPlayer(false);                    
                    currentState = CatchState.Catching;                    
                }
                return NodeState.RUNNING;

            case CatchState.Catching:
                if (monster.IsCatch())
                {
                   retreatTarget = GetRandomRetreatPosition();
                    currentState = CatchState.Retreating;
                }
                else
                {
                    hoverTimer -= Time.deltaTime;
                    if (hoverTimer <= 0f)
                    {                      
                        currentState = CatchState.Hovering;
                        hoverTime = Random.Range(0,0.2f);
                        hoverTimer = hoverTime;
                    }
                }
                return NodeState.RUNNING;

            case CatchState.Retreating:
                agent.SetDestination(retreatTarget);
                Vector3 retreatPosXZ = new Vector3(retreatTarget.x, 0f, retreatTarget.z);
                float distanceToRetreat = Vector3.Distance(monsterPosXZ, retreatPosXZ);

                if (distanceToRetreat < retreatTargetReachThreshold)
                {                    
                    monster.SetAnimatorParameter(MonsterAnimatorHash.ReleaseHash, null);
                    currentState = CatchState.Hovering;
                    hoverTime = Random.Range(0, 0.2f);
                    hoverTimer = hoverTime;
                }
                return NodeState.RUNNING;

            case CatchState.Hovering:
                monster.SetIsHovering(true);
                hoverTimer -= Time.deltaTime;             
                FlyAroundPoint(player.transform.position);                
                if (hoverTimer <= 0f)
                {
                    currentState = CatchState.FlyingToPlayer;
                    monster.SetIsHovering(false);
                }
                return NodeState.RUNNING;
                
        }

        return NodeState.RUNNING;
    }

    private Vector3 GetRandomRetreatPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * retreatRadius;
        Vector3 rawPos = player.position + new Vector3(randomCircle.x, 0, randomCircle.y);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(rawPos, out hit, retreatRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return monster.transform.position;
    }

    private void FlyAroundPoint(Vector3 center)
    {
        Vector3 offset = new Vector3(Mathf.Sin(Time.time), 0, Mathf.Cos(Time.time)) * 15f;
        Vector3 rawTarget = center + offset;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(rawTarget, out hit, 15f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            agent.SetDestination(center);
        }
    }   

    
}