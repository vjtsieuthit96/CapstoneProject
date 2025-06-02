using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private MonsterAI monster;

    [SerializeField] private float skill1Cooldown = 5f;
    [SerializeField] private float skill2Cooldown = 10f;
    [SerializeField] private float skill3Cooldown = 15f;

    private Dictionary<int, float> skillCooldowns = new Dictionary<int, float>();

    private void Awake()
    {
        InitializeCooldowns();
    }

    private void InitializeCooldowns()
    {
        skillCooldowns[MonsterAnimatorHash.skill_1Hash] = skill1Cooldown;
        skillCooldowns[MonsterAnimatorHash.skill_2Hash] = skill2Cooldown;
        skillCooldowns[MonsterAnimatorHash.skill_3Hash] = skill3Cooldown;
    }

    public bool CanUseSkill(int skillHash)
    {
        return Time.time >= skillCooldowns[skillHash]; // Kiểm tra cooldown
    }

    public void UseSkill(int skillHash)
    {
        if (CanUseSkill(skillHash))
        {
            monster.SetAnimatorParameter(skillHash, null);
            skillCooldowns[skillHash] = Time.time + GetSkillCooldown(skillHash);
        }
    }

    private float GetSkillCooldown(int skillHash)
    {
        return skillCooldowns.TryGetValue(skillHash, out float cooldown) ? cooldown : 1f;
    }
}