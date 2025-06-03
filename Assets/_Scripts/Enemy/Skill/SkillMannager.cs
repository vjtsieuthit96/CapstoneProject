using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private MonsterAI monster;

    [Header("Cooldown Settings")]
    [SerializeField] private float skill1Cooldown;
    [SerializeField] private float skill2Cooldown;
    [SerializeField] private float skill3Cooldown;

    private Dictionary<int, float> skillCooldowns = new Dictionary<int, float>();
    private Dictionary<int, bool> isSkillOnCooldown = new Dictionary<int, bool>();

    private void Awake()
    {
        skillCooldowns[MonsterAnimatorHash.skill_1Hash] = skill1Cooldown;
        skillCooldowns[MonsterAnimatorHash.skill_2Hash] = skill2Cooldown;
        skillCooldowns[MonsterAnimatorHash.skill_3Hash] = skill3Cooldown;

        isSkillOnCooldown[MonsterAnimatorHash.skill_1Hash] = false;
        isSkillOnCooldown[MonsterAnimatorHash.skill_2Hash] = false;
        isSkillOnCooldown[MonsterAnimatorHash.skill_3Hash] = false;
    }

    public bool CanUseSkill(int skillHash)
    {
        if (!skillCooldowns.ContainsKey(skillHash))
        {
            return false;
        }

        return !isSkillOnCooldown[skillHash];
    }

    public void UseSkill(int skillHash)
    {
        if (!skillCooldowns.ContainsKey(skillHash) || isSkillOnCooldown[skillHash])
        {
            Debug.Log($"Skill {skillHash} vẫn đang hồi.");
            return;
        }

        StartCoroutine(CooldownRoutine(skillHash));
        monster.SetAnimatorParameter(skillHash, null);
        isSkillOnCooldown[skillHash] = true;
    }

    private IEnumerator CooldownRoutine(int skillHash)
    {
        float cooldown = skillCooldowns[skillHash];
        yield return new WaitForSecondsRealtime(cooldown);
        isSkillOnCooldown[skillHash] = false;
    }
}