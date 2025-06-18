using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillTree", menuName = "Scriptable Objects/SkillTree")]
public class SkillTree : ScriptableObject
{
    public List<SkillNode> allNodes;
}
