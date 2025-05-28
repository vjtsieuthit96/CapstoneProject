using UnityEngine;

public abstract class MonsterAI : MonoBehaviour
{
    protected Node behaviorTree;
    [Header("-----Target-----")]
    [SerializeField] protected Transform target;

    [Header("-----FOV-----")]   
    [SerializeField] protected float viewRadius = 15f; // Tầm nhìn tối đa
    [SerializeField] protected float viewAngle = 105f; // Góc nhìn của quái vật

    void Start()
    {  
        behaviorTree = CreateBehaviorTree();
        InvokeRepeating("EvaluateBehaviorTree", 2f, 2f);
    }

    void EvaluateBehaviorTree()
    {
        behaviorTree.Evaluate();
    }

    protected abstract Node CreateBehaviorTree();   

    public float GetViewRadius() => viewRadius;
    public float GetViewAngle() => viewAngle;
    public Transform GetTarget() { return target; }
}