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
        Transform player = monster.GetTarget();
        if (player == null) return NodeState.FAILURE;

        monster.transform.LookAt(new Vector3(player.position.x, monster.transform.position.y, player.position.z));

        if (!skillManager.CanUseSkill(MonsterAnimatorHash.skill_1Hash) &&
            !skillManager.CanUseSkill(MonsterAnimatorHash.skill_2Hash) &&
            !skillManager.CanUseSkill(MonsterAnimatorHash.skill_3Hash))
        {
            Debug.Log("Tất cả Skill đang CD");
            return NodeState.FAILURE;
        }

        if (skillManager.CanUseSkill(MonsterAnimatorHash.skill_1Hash))
        {
            skillManager.UseSkill(MonsterAnimatorHash.skill_1Hash);
            Debug.Log("Dùng Skill 1");
            return NodeState.RUNNING; //  Trả về RUNNING để tránh gọi lại liên tục
        }
        else if (skillManager.CanUseSkill(MonsterAnimatorHash.skill_2Hash))
        {
            skillManager.UseSkill(MonsterAnimatorHash.skill_2Hash);
            Debug.Log("Dùng Skill 2");
            return NodeState.RUNNING;
        }
        else if (skillManager.CanUseSkill(MonsterAnimatorHash.skill_3Hash))
        {
            skillManager.UseSkill(MonsterAnimatorHash.skill_3Hash);
            Debug.Log("Dùng Skill 3");
            return NodeState.RUNNING;
        }

        return NodeState.FAILURE;
    }
}