using UnityEngine;

public class CooldownNode : Node
{
    private float cooldownTime;
    private float startTime;

    public CooldownNode(float cooldownTime)
    {
        this.cooldownTime = cooldownTime;
    }

    public override NodeState Evaluate()
    {
        if (startTime == 0f)
        {
            startTime = Time.time;
        }

        if (Time.time - startTime < cooldownTime)
        {
            return NodeState.RUNNING; // AI vẫn đang đợi cooldown
        }

        startTime = 0f;
        return NodeState.SUCCESS; // AI nghỉ xong, tiếp tục tuần tra
    }
}