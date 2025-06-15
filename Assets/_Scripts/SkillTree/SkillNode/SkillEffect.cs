using UnityEngine;

[CreateAssetMenu(fileName = "SkillEffect", menuName = "Scriptable Objects/SkillEffect")]
public abstract class SkillEffect : ScriptableObject
{
    public abstract void ApplyEffect();
}
