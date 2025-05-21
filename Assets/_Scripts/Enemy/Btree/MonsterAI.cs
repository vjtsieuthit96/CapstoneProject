using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

public abstract class MonsterAI : MonoBehaviour
{
    public float health;
    public float maxHealth = 100f;
    [SerializeField] private Transform target;

    protected Node behaviorTree;

    protected float rageDuration = 30f; // Thời gian Cuồng Nộ kéo dài
    public float GetRageDuration()
    {
        return rageDuration;
    }

    protected float rageCooldown = 600f; // Thời gian hồi chiêu Cuồng Nộ (10 phút)
    public float GetRageCooldown()
    {
        return rageCooldown;
    }

    protected float lastRageTime = -Mathf.Infinity; // Thời điểm kích hoạt Cuồng Nộ
    public float GetLastRageTime()
    {
        return lastRageTime;
    }
    public void SetLastRageTime(float time)
    {
        lastRageTime = time;
    }

    protected bool isEnraged = false; // Trạng thái Cuồng Nộ
    public bool GetisEnraged()
    {
        return isEnraged;
    }
    public void SetEnraged(bool state)
    {
        isEnraged = state;       
    }

    void Start()
    {
        health = maxHealth;
        behaviorTree = CreateBehaviorTree(); // Gọi phương thức abstract
        InvokeRepeating("EvaluateBehaviorTree", 2f, 2f);
    }

    void EvaluateBehaviorTree()
    {
        behaviorTree.Evaluate();
    }

    protected abstract Node CreateBehaviorTree();
}