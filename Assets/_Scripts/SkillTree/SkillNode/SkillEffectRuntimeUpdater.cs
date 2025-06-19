using UnityEngine;

public class SkillEffectRuntimeUpdater : MonoBehaviour
{
    [SerializeField] private SkillTree skillTree;
    [SerializeField] private CharacterConfigurator configurator;

    void Update()
    {
        foreach (var node in skillTree.allNodes)
        {
            if (node.isUnlocked && node.effect != null)
            {
                node.effect.UpdateCondition(configurator);
            }
        }
    }
}
