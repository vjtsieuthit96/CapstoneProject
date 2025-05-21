using System.Collections.Generic;
using UnityEngine;

//Node Status
public enum NodeState
{
    SUCCESS,
    FAILURE,
    RUNNING
}


public abstract class Node : MonoBehaviour
{
    protected NodeState state;
    public Node parent;
    protected List<Node> children = new List<Node>();

    public Node()
    {
    }

    public Node(List<Node> children)
    {
        foreach (var child in children)
        {
            Attach(child);
        }
    }

    public void Attach (Node child)
    {
        child.parent = this;
        this.children.Add(child);
    }

    public abstract NodeState Evaluate();  
}
