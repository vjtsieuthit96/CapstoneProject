using UnityEngine;

public class MeleeAttackNode : Node
{
    private MonsterAI monster;
    private float attackCooldown = 1f; //  Delay giữa các đòn tấn công
    private float lastAttackTime = float.MinValue; // Lưu thời gian AI tấn công lần cuối

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

        //  Nếu AI vừa tấn công, không tấn công tiếp tục ngay
        if (Time.time - lastAttackTime < attackCooldown)
        {
            return NodeState.RUNNING; // Chờ đến khi đủ thời gian cooldown mới tiếp tục đánh
        }

        // Xoay mặt về hướng người chơi trước khi tấn công
        monster.transform.LookAt(new Vector3(player.position.x, monster.transform.position.y, player.position.z));

        Debug.Log("Normal Attack!");
        monster.SetAnimatorParameter(MonsterAnimatorHash.nAttackHash, null); // Kích hoạt animation tấn công    

        lastAttackTime = Time.time; // Cập nhật thời gian tấn công cuối cùng
        return NodeState.RUNNING; //  Đảm bảo AI không spam liên tục
    }
}