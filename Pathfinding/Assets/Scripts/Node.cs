using System;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    Open = 0,
    Blocked = 1,
    LightTerrain = 2,
    MediumTerrain = 3,
    HeavyTerrain = 3
}

public class Node : IComparable<Node>
{
    public NodeType NodeType = NodeType.Open;

    public int xIndex = -1;
    public int yIndex = -1;

    public Vector3 position;

    public List<Node> neighbors = new List<Node>();
    public Node previous = null;
    public float distanceTraveled = Mathf.Infinity;
    public float priority;

    public Node(int xIndex, int yIndex, NodeType nodeType)
    {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        NodeType = nodeType;
    }

    public void Reset()
    {
        previous = null;
    }

    public int CompareTo(Node other)
    {
        if (priority < other.priority)
        {
            return -1;
        }

        if (priority > other.priority)
        {
            return 1;
        }

        return 0;
    }
}
