using UnityEngine;

public class MeleeAttackNode : Node
{
    private MonsterAI monster;

    public MeleeAttackNode(MonsterAI monster)
    {
        this.monster = monster;
    }

    public override NodeState Evaluate()
    {
        Transform player = monster.GetTarget();
        if (player == null || Vector3.Distance(monster.transform.position, player.position) > monster.GetAttackRange())
        {
            return NodeState.FAILURE;
        }
        Debug.Log("Normal Attack!");
        monster.SetAnimatorParameter(MonsterAnimatorHash.nAttackHash, null); // Kích hoạt animation tấn công       

        return NodeState.RUNNING; // Giữ trạng thái RUNNING để tiếp tục hành vi
    }
}