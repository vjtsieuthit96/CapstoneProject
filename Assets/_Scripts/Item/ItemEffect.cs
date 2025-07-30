using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemEffect", menuName = "ItemEffect/Effect")]
public class ItemEffect : ScriptableObject
{
    public ItemEffectType effectType;
    [Header("Percent %")]
    [UnityEngine.Range(0f, 2f)]
    public float value;
    public float duration;
}
public enum ItemEffectType
{
    SurvivalMode,
    AdrenalineRush,
    Doping,
    BerserkState
}