using UnityEngine;

public class CheckPlayerInRange : Node
{
    private MonsterAI monsterAI;

    public CheckPlayerInRange(MonsterAI monster) { this.monsterAI = monster; }  

    public override NodeState Evaluate()
    {
        Transform player = monsterAI.GetTarget();
        if (player == null) return NodeState.FAILURE;
        float distanceToPlayer = Vector3.Distance(monsterAI.transform.position, player.position);
        if (distanceToPlayer <= monsterAI.GetAttackRange())
        {
            //Debug.Log("Player is within attack range!");
            return NodeState.SUCCESS;
        }
        else
        {
            //Debug.Log("Player is out of attack range.");
            return NodeState.FAILURE;
        }
    }
}
