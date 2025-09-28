using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

public class DarkMagicTree : MonsterAI
{
    [Header("Tree Skills")]
    public List<OnTreeSkill> skillCircles = new List<OnTreeSkill>();
    public bool skill7_Active;
    public Transform heartOfTree;
    public EnemySpawner spawner;

    [Header("Defense Zone")]
    public BoxCollider defenseCollider;
    public bool isPlayerInDefenseZone = false;
    public bool isDefending = false;
    [SerializeField] private MaterialSwitcher matterialSwitch;
    public SequentialScaler sequentialScaler;

    protected override void Start()
    {
        behaviorTree = CreateBehaviorTree();
        RepeatEvaluateBehaviorTree(0f, 1.5f);

        if (defenseCollider == null && heartOfTree != null)
        {
            defenseCollider = heartOfTree.gameObject.AddComponent<BoxCollider>();
            defenseCollider.isTrigger = true;
        }
    }
   
    protected override void Update()
    {
        Die();
    }

    public override void Die()
    {
        if (!isDead && monsterStats.GetCurrentHealth() <= 0)
        {
            isDead = true;
            spawner.canSpawn = false;
            sequentialScaler.TheEndOfTree();
        }
    }

    protected override void OnEnable()
    {
        target = PlayerMock.Instance.PlayerTransform;
        isDead = false;
        monsterStats.ResetStatsToInitial();
        ApplyRestart();
        isPlayerInDefenseZone = false;
    }

    protected override Node CreateBehaviorTree()
    {
        return new Selector(new List<Node>
        {
            new DarkTreeDefenseNode(this, heartOfTree),
            new DarkTreeAttackNode(this, monsterStats, target, spawner)
        });
    }

    public override void ApplyDamage(float amount)
    {
        base.ApplyDamage(amount);
        matterialSwitch.HurtofTree();
    }

    public void ActivateNearestCircleSkill()
    {
        OnTreeSkill nearest = GetNearestCircleToPlayer();
        if (nearest != null)
            ResetAllCircleSkills(nearest);
            nearest.isTreeSkill = true;
    }

    public void ActivateNearestCirclesSkill(int count)
    {
        List<OnTreeSkill> nearestCircles = GetNearestCirclesToPlayer(count);

        ResetAllCircleSkills(nearestCircles.ToArray());
        foreach (var circle in nearestCircles)
            circle.isTreeSkill = true;
    }


    public OnTreeSkill GetNearestCircleToPlayer()
    {
        OnTreeSkill nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var c in skillCircles)
        {
            float d = Vector2.Distance(
                new Vector2(c.transform.position.x, c.transform.position.z),
                new Vector2(target.position.x, target.position.z));

            if (d < minDist)
            {
                minDist = d;
                nearest = c;
            }
        }
        return nearest;
    }
    public void ResetAllCircleSkills(OnTreeSkill Tree)
    {
        foreach (var circle in skillCircles)
        {
            if (circle != Tree)
            {
                circle.isTreeSkill = false;
            }
        }       
    }

    public void ResetAllCircleSkills(OnTreeSkill[] treeSkillsToKeep)
    {
        foreach (var circle in skillCircles)
        {
            if (!System.Array.Exists(treeSkillsToKeep, t => t == circle))
            {
                circle.isTreeSkill = false;
            }
        }
    }


    public List<OnTreeSkill> GetNearestCirclesToPlayer(int count)
    {
        List<OnTreeSkill> sorted = new List<OnTreeSkill>(skillCircles);
        sorted.Sort((a, b) =>
        {
            float da = Vector2.Distance(new Vector2(a.transform.position.x, a.transform.position.z),
                                        new Vector2(target.position.x, target.position.z));
            float db = Vector2.Distance(new Vector2(b.transform.position.x, b.transform.position.z),
                                        new Vector2(target.position.x, target.position.z));
            return da.CompareTo(db);
        });

        return sorted.GetRange(0, Mathf.Min(count, sorted.Count));
    }
}
