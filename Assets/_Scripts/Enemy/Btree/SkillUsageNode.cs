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

        float distancetoPlayer = Vector3.Distance(monster.transform.position, player.transform.position);
        if (distancetoPlayer > monster.GetStoppingDistance()*1.25f)
        {
            Debug.Log("Chưa đủ khoảng cách dùng skill");
            return NodeState.FAILURE;
        }
        monster.transform.LookAt(new Vector3(player.position.x, monster.transform.position.y, player.position.z));
        //  Danh sách skill ưu tiên (có thể sắp xếp để chọn skill mạnh nhất trước)
        int[] skillPriority = 
        {
            MonsterAnimatorHash.skill_3Hash, // Skill mạnh nhất
            MonsterAnimatorHash.skill_2Hash,
            MonsterAnimatorHash.skill_1Hash  // Skill yếu nhất
        };
    
        foreach (int skill in skillPriority)
        {
            if (skillManager.CanUseSkill(skill))
            {
                skillManager.UseSkill(skill);
                Debug.Log($"Dùng Skill {skill}");
                return NodeState.RUNNING; // Ngừng tìm kiếm ngay sau khi dùng skill
            }
        }

        Debug.Log("Tất cả Skill đang CD, chọn hành vi khác.");
        return NodeState.FAILURE;
    }
}