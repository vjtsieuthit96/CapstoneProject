using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private MonsterAI monster;

    [Header("Cooldown Settings")]
    [SerializeField] private float skill1Cooldown;
    [SerializeField] private float skill2Cooldown;
    [SerializeField] private float skill3Cooldown;

    [Header("Skill Range Settings")] // Thêm khoảng cách skill vào Inspector
    [SerializeField] private float skill1Range;
    [SerializeField] private float skill2Range;
    [SerializeField] private float skill3Range;

    private Dictionary<int, float> skillCooldowns = new Dictionary<int, float>();
    private Dictionary<int, bool> isSkillOnCooldown = new Dictionary<int, bool>();
    private Dictionary<int, float> skillRanges = new Dictionary<int, float>(); // Lưu khoảng cách từng skill

    private void Awake()
    {
        skillCooldowns[MonsterAnimatorHash.skill_1Hash] = skill1Cooldown;
        skillCooldowns[MonsterAnimatorHash.skill_2Hash] = skill2Cooldown;
        skillCooldowns[MonsterAnimatorHash.skill_3Hash] = skill3Cooldown;

        isSkillOnCooldown[MonsterAnimatorHash.skill_1Hash] = false;
        isSkillOnCooldown[MonsterAnimatorHash.skill_2Hash] = false;
        isSkillOnCooldown[MonsterAnimatorHash.skill_3Hash] = false;

        skillRanges[MonsterAnimatorHash.skill_1Hash] = skill1Range; //  Lưu khoảng cách skill
        skillRanges[MonsterAnimatorHash.skill_2Hash] = skill2Range;
        skillRanges[MonsterAnimatorHash.skill_3Hash] = skill3Range;       

    }

    public bool CanUseSkill(int skillHash)
    {
        return skillCooldowns.ContainsKey(skillHash) && !isSkillOnCooldown[skillHash];
    }

    public void UseSkill(int skillHash)
    {
        if (!CanUseSkill(skillHash)) return;

        StartCoroutine(CooldownRoutine(skillHash));
        monster.SetAnimatorParameter(skillHash, null);
        isSkillOnCooldown[skillHash] = true;
    }

    public float GetSkillRange(int skillHash) // Hàm lấy khoảng cách sử dụng skill
    {
        return skillRanges.ContainsKey(skillHash) ? skillRanges[skillHash] : 0f;
    }
    public List<int> GetSkillListSortedByPriority()
    {
        return skillRanges.OrderByDescending(s => s.Value) //Sắp xếp skill theo khoảng cách giảm dần
                          .Select(s => s.Key)
                          .ToList();
    }

    private IEnumerator CooldownRoutine(int skillHash)
    {
        yield return new WaitForSecondsRealtime(skillCooldowns[skillHash]);
        isSkillOnCooldown[skillHash] = false;
    }
}