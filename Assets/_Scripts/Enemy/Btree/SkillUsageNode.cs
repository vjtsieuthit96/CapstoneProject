using UnityEngine;
public class SkillUsageNode : Node
{
    private MonsterAI monster;
    private SkillManager skillManager;

    public SkillUsageNode(MonsterAI monster, SkillManager skillManager)
    {
        this.monster = monster;
        this.skillManager = skillManager;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Kiểm tra skill có thể sử dụng...");

        if (skillManager.CanUseSkill(MonsterAnimatorHash.skill_1Hash))
        {
            skillManager.UseSkill(MonsterAnimatorHash.skill_1Hash);
            return NodeState.SUCCESS;
        }
        else if (skillManager.CanUseSkill(MonsterAnimatorHash.skill_2Hash))
        {
            skillManager.UseSkill(MonsterAnimatorHash.skill_2Hash);
            return NodeState.SUCCESS;
        }
        else if (skillManager.CanUseSkill(MonsterAnimatorHash.skill_3Hash))
        {
            skillManager.UseSkill(MonsterAnimatorHash.skill_3Hash);
            return NodeState.SUCCESS;
        }

        Debug.Log("Không có skill sẵn sàng! Chuyển sang MeleeAttackNode.");
        return NodeState.FAILURE; // Đảm bảo AI chuyển sang MeleeAttackNode
    }
}