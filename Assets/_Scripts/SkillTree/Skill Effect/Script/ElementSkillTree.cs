using UnityEngine;

[CreateAssetMenu(fileName = "PlayerElement", menuName = "Scriptable Objects/Element/PlayerElement")]
public class ElementSkillTree : SkillEffect
{
    [Header("Class")]
    [Range(0, 7)]
    public int Class = 1;
    [Header("Hitted")]
    public int Hitted;
    public int HitLimit;
    
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        if(configurator.PlayerElementClass <= Class)
        {
            configurator.PlayerElementClass = Class;
        }
    }
    public override void UpdateCondition(CharacterConfigurator configurator)
    {
        var counter = EnemyHitCounter.Instance;
        if (counter == null || Hitted <= 0) return;

        counter.StartElementCount();

        int elementHitCount = counter.GetElementEnemyHitCount();
        if (elementHitCount > 0 && elementHitCount % Hitted == 0)
        {
            counter.StopElementCount();
            counter.StartCountElement();

            if (counter.GetElementEnemyCount() <= HitLimit)
            {
                configurator.isEffectMode = true;
            }
            else
            {
                configurator.isEffectMode = false;
                counter.StopCountElement();
                counter.StartElementCount();
            }
        }
    }
}
