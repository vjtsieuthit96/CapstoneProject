using System;
using System.Collections.Generic;

[Serializable]
public class SkillNodeState
{
    public string nodeId;
    public bool isUnlocked;
}

[Serializable]
public class SkillTreeState
{
    public List<SkillNodeState> nodeStates = new List<SkillNodeState>();
}
