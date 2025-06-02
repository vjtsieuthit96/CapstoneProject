using System.Collections.Generic;

public class OrcAI : MonsterAI
{
    protected override Node CreateBehaviorTree()
    {
        return new Selector(new List<Node>
        {
            new Sequence(new List<Node> // Nếu máu thấp, AI retreat ngay lập tức
            {
                new CheckLowHealthNode(this, monsterStats), // Kiểm tra máu đầu tiên
                new RetreatNode(this, _monsterAgent)                
            }),
            new Sequence(new List<Node> // Nếu thấy Player và máu bình thường, AI chiến đấu
            {
                new CheckPlayerInFOVNode(this), // Kiểm tra FOV sau khi biết máu bình thường
                new ChaseNode(this,_monsterAgent), // AI quyết định tấn công
                new Selector(new List<Node> // Chọn hành động chiến đấu
                {
                    new SkillUsageNode(this, skillManager), // Dùng kỹ năng nếu có
                    new MeleeAttackNode(this) // Nếu không có kỹ năng, đánh thường
                })
            }),
            new PatrolNode(this, _monsterAgent) // Nếu không thấy Player, AI tuần tra
        });
    }
}