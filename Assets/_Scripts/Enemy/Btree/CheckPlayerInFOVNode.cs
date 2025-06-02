using UnityEngine;

public class CheckPlayerInFOVNode : Node
{
    private MonsterAI monsterAI;

    public CheckPlayerInFOVNode(MonsterAI monster) { this.monsterAI = monster; }

    public override NodeState Evaluate()
    {
        Vector3 directionToPlayer = (monsterAI.GetTarget().position - monsterAI.transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(monsterAI.transform.position, monsterAI.GetTarget().position);

        // Kiểm tra nếu người chơi nằm trong tầm nhìn

        if (distanceToPlayer > monsterAI.GetViewRadius())
        {
            monsterAI.SetAnimatorParameter(MonsterAnimatorHash.isBattleHash, false);
            return NodeState.FAILURE;
        }
        if (Vector3.Angle(monsterAI.transform.forward, directionToPlayer) > monsterAI.GetViewAngle() / 2) return NodeState.FAILURE;       

        Debug.Log("Monster nhìn thấy người chơi!");
        monsterAI.SetAnimatorParameter(MonsterAnimatorHash.isBattleHash, true);
        return NodeState.SUCCESS;
    }
}