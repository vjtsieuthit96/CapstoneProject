using UnityEngine;

public class SkillTreeSystem : MonoBehaviour
{
    public SkillTree skillTree;
    public int availableSkillPoints;

    public void TryUnlock(SkillNode node)
    {
        if (availableSkillPoints > 0 && node.CanUnlock())
        {
            node.Unlock();
            availableSkillPoints--;
        }
    }
}
