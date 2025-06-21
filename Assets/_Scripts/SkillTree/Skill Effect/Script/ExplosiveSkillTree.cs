using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
[CreateAssetMenu(fileName = "Explosive", menuName = "Scriptable Objects/Element/Explosive")]
public class ExplosiveSkillTree : SkillEffect
{
    [Header("Class")]
    [Range(0f, 5f)]
    public int ExplosiveClass = 1;

    [Header("Number Of Hit")]
    public int Hitted = 20;
    public int HitLimit = 1;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        if(configurator.ExplosiveClass <= ExplosiveClass)
        {
            configurator.ExplosiveClass = ExplosiveClass;
        }
    }
    public override void UpdateCondition(CharacterConfigurator configurator)
    {
        var counter = EnemyHitCounter.Instance;
        if (counter == null || Hitted <= 0) return;

        int totalHits = counter.GetEnemyHitCount();

        if (totalHits == 0 || totalHits % Hitted != 0) return;

        counter.StopCount();
        counter.StartCountExplosive();

        int explosiveCount = counter.GetExplosiveCount();

        if (explosiveCount < HitLimit)
        {
            configurator.isExplosive = true;
        }
        else
        {
            configurator.isExplosive = false;
            counter.StopCountExplosive();
            counter.StartCount();
        }
    }


}
