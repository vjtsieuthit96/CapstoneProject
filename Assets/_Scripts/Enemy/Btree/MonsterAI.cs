using UnityEngine;

public abstract class MonsterAI : MonoBehaviour
{   
    [SerializeField] protected Transform target;
    protected Node behaviorTree;

    [SerializeField] protected float rageDuration = 30f; // Thời gian Cuồng Nộ
    [SerializeField] protected float rageCooldown = 600f; // Hồi chiêu Cuồng Nộ
    [SerializeField] protected float lastRageTime = -Mathf.Infinity; // Thời điểm kích hoạt Cuồng Nộ
    [SerializeField] protected bool isEnraged = false; // Trạng thái Cuồng Nộ
   
    [SerializeField] protected float viewRadius = 15f; // Tầm nhìn tối đa
    [SerializeField] protected float viewAngle = 90f; // Góc nhìn của quái vật

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
   
    public float GetRageDuration() => rageDuration;
    public float GetRageCooldown() => rageCooldown;
    public float GetLastRageTime() => lastRageTime;
    public bool GetIsEnraged() => isEnraged;
    public float GetViewRadius() => viewRadius;
    public float GetViewAngle() => viewAngle;
    public Transform GetTarget() { return target; }
    public void SetLastRageTime(float time) => lastRageTime = time;
    public void SetEnraged(bool state) => isEnraged = state;
}