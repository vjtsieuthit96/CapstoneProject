using UnityEngine;

public class DarkTreeDefenseNode : Node
{
    private DarkMagicTree tree;
    private Transform heart;

    public DarkTreeDefenseNode(DarkMagicTree tree, Transform heart)
    {
        this.tree = tree;
        this.heart = heart;
    }

    public override NodeState Evaluate()
    {
        if (tree.isPlayerInDefenseZone)
        {
            tree.isDefending = true;
            if (tree.skillCircles[6] != null)
            {
                tree.skillCircles[6].isTreeSkill = true;  
            }
            state = NodeState.SUCCESS;
            return state;
        }

        tree.isDefending = false;
        if (tree.skillCircles[6] != null)
        {
            tree.skillCircles[6].isTreeSkill = false;
        }
        state = NodeState.FAILURE;
        return state;
    }
}
