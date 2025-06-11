using UnityEngine;
using System.Collections.Generic;

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

        float distanceToPlayer = Vector3.Distance(monster.transform.position, player.position);
        monster.transform.LookAt(new Vector3(player.position.x, monster.transform.position.y, player.position.z));

        //Lấy danh sách skill từ `SkillManager`, sắp xếp theo khoảng cách giảm dần
        List<int> skillPriority = skillManager.GetSkillListSortedByPriority();

        foreach (int skill in skillPriority)
        {
            float skillRange = skillManager.GetSkillRange(skill);

            if (distanceToPlayer <= skillRange && skillManager.CanUseSkill(skill))
            {
                skillManager.UseSkill(skill);
                Debug.Log($"Dùng Skill {skill}");
                return NodeState.RUNNING;
            }
        }

        Debug.Log("Tất cả Skill đang hồi hoặc ngoài phạm vi.");
        return NodeState.FAILURE;
    }
}