using UnityEngine;

public class CheckPlayerInFOVNode : Node
{
    private MonsterAI monster;

    public CheckPlayerInFOVNode(MonsterAI monster) { this.monster = monster; }

    public override NodeState Evaluate()
    {
        Vector3 directionToPlayer = (monster.GetTarget().position - monster.transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(monster.transform.position, monster.GetTarget().position);

        // Kiểm tra nếu người chơi nằm trong tầm nhìn
        if (distanceToPlayer > monster.GetViewRadius()) return NodeState.FAILURE;
        if (Vector3.Angle(monster.transform.forward, directionToPlayer) > monster.GetViewAngle() / 2) return NodeState.FAILURE;

        // Kiểm tra nếu có vật chắn giữa quái vật và người chơi (Raycast)
        if (Physics.Raycast(monster.transform.position, directionToPlayer, distanceToPlayer))
        {
            return NodeState.FAILURE;
        }

        Debug.Log("Monster nhìn thấy người chơi!");
        return NodeState.SUCCESS;
    }
}