using UnityEngine;

public class RoarNode : Node
{
    private MonsterAudio monsterAudio;
    private MonsterAI monsterAI;
    private float roarCooldown = 30f;
    private float lastRoarTime = -30f;
    private bool isRoaring = false;

    public RoarNode(MonsterAI monsterAI, MonsterAudio monsterAudio)
    {
        this.monsterAI = monsterAI;
        this.monsterAudio = monsterAudio;
    }

    public override NodeState Evaluate()
    {
        //  Nếu AI đang roar, chờ đến khi hoàn thành để cho phép tấn công
        if (isRoaring)
        {
            if (Time.time - lastRoarTime >= 1f)
            {
                isRoaring = false; //  Cho phép AI tấn công sau khi gầm xong
                return NodeState.SUCCESS;
            }
            return NodeState.RUNNING;
        }

        //  Nếu AI chưa roar, thực hiện roar
        if (Time.time - lastRoarTime >= roarCooldown)
        {
            monsterAudio.PlayRoar();
            lastRoarTime = Time.time;
            isRoaring = true; //  Đánh dấu AI đang gầm            
            return NodeState.RUNNING;
        }

        return NodeState.SUCCESS; // Nếu không cần gầm, tiếp tục hành vi bình thường
    }
}